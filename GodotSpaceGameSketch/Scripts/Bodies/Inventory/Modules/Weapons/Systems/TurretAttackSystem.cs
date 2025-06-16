using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Godot;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;
using GodotSpaceGameSketch.Core.Extensions;
using GodotSpaceGameSketch.Core.Utilities;
using GodotSpaceGameSketch.Movement.Components;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Systems;

public class TurretAttackSystem : QuerySystem<NodeRef, WeaponModule, GimbalController, WeaponTurret>
{
    // Угол по отношении турели к цели, когда ей имеет смысл стрелять
    private const float AngleToFire = 30f * PsiMath.Deg2Rad;
    
    public TurretAttackSystem()
    {
        Filter
            .WithoutAnyTags(Tags.Get<DeathEvent>())
            .AnyTags(Tags.Get<WeaponTurretAttackState>());
    }
        
    protected override void OnUpdate()
    {
        var commandBuffer = CommandBuffer;
        Query.ForEachEntity((
            ref NodeRef turretNode,
            ref WeaponModule weaponModule,
            ref GimbalController gimbalController, 
            ref WeaponTurret weaponTurret, 
            Entity entity) =>
        {
            if (weaponModule.target.IsNull)
            {
                commandBuffer.RemoveTag<WeaponTurretAttackState>(entity.Id);
                commandBuffer.AddTag<WeaponTurretFindTargetState>(entity.Id);

                foreach (var weaponUnitEntityId in entity.ChildIds)
                {
                    commandBuffer.RemoveTag<WeaponUnitAttackState>(weaponUnitEntityId);
                }
                return;
            }

            var deltaTime = Tick.deltaTime;
            ref var targetPosition = ref weaponModule.target.GetComponent<CelestialBodyPosition>();
            ref var targetMovable = ref weaponModule.target.GetComponent<VehicleMovable>();
            
            var predictedTargetPosition = CalculateLeadPosition(
                turretNode.value.GlobalPosition,
                targetPosition.value,
                targetMovable.velocity,
                weaponTurret.projectileSpeed,
                weaponTurret.leadPredictionFactor,
                weaponTurret.maxLeadAngle
                );
            
            //PsiDebug.DrawLine(turretNode.value.GlobalPosition, predictedTargetPosition, Colors.Blue, deltaTime);
            
            if (entity.Id == 0
                || entity.Id != 0
                && turretNode.value.GlobalPosition.DistanceTo(predictedTargetPosition) >
                weaponModule.attackRadius)
            {
                commandBuffer.RemoveTag<WeaponTurretAttackState>(entity.Id);
                commandBuffer.AddTag<WeaponTurretFindTargetState>(entity.Id);
                    
                foreach (var weaponUnitEntityId in entity.ChildIds)
                {
                    commandBuffer.RemoveTag<WeaponUnitAttackState>(weaponUnitEntityId);
                }
                return;
            }

            if (CanFire(turretNode.value, ref gimbalController, predictedTargetPosition))
            {
                var unitIds = entity.ChildIds;
                foreach (var unitId in unitIds)
                {
                    commandBuffer.AddTag<WeaponUnitAttackState>(unitId);
                }
            }
            else
            {
                var unitIds = entity.ChildIds;
                foreach (var unitId in unitIds)
                {
                    commandBuffer.RemoveTag<WeaponUnitAttackState>(unitId);
                    commandBuffer.AddTag<WeaponTurretFindTargetState>(entity.Id);
                        
                    foreach (var weaponUnitEntityId in entity.ChildIds)
                    {
                        commandBuffer.RemoveTag<WeaponUnitAttackState>(weaponUnitEntityId);
                    }
                }
            }
                
            GimbalControllerFunctions.RotateToPosition(predictedTargetPosition, deltaTime, turretNode.value, ref gimbalController, ref weaponTurret);
        });
    }
        
    private bool CanTurretSeeTarget(Entity target, ref GimbalController gimbalController)
    {
        var gimbalTransform = gimbalController.verticalPivot;
        var turretPosition = gimbalTransform.GlobalPosition;
        var turretForward = gimbalTransform.GlobalTransform.Forward();

        var result = RaycastExtensions.MakeRaycast(turretPosition, turretForward * 1000);
        if (result.Count > 0 && result["collider"].Obj is Battle.Bodies.CelestialBodyView celestialBody)
        {
            return celestialBody.Entity == target;
        }

        return false;
    }
    
    private Vector3 CalculateLeadPosition(Vector3 turretPosition, Vector3 targetPosition, Vector3 targetVelocity, 
        float projectileSpeed, float predictionFactor, float maxLeadAngle)
    {
        var direction = targetPosition - turretPosition;
        var distance = direction.Length();
        
        // Время полёта снаряда
        var timeToTarget = Mathf.Clamp(distance / projectileSpeed, 0f, 2f) * predictionFactor;
        
        // Упреждение
        var leadDirection = targetVelocity * timeToTarget;
        /*if (direction.AngleTo(leadDirection) > maxLeadAngle)
        {
            leadDirection = direction.Normalized() * leadDirection.Length();
        }*/
        
        return targetPosition + leadDirection;
    }
    
    private bool CanFire(Node3D turretNode, ref GimbalController gimbalController, Vector3 predictedPos)
    {
        var targetDir = (predictedPos - turretNode.GlobalPosition).Normalized();
        float angleToTarget = gimbalController.verticalPivot.GlobalTransform.Forward().AngleTo(targetDir);
        
        //PsiDebug.DrawRay(turretNode.GlobalPosition, targetDir, Colors.Green);
        //PsiDebug.DrawRay(turretNode.GlobalPosition, gimbalController.verticalPivot.GlobalBasis.Forward(), Colors.Brown);
        bool isAimed = angleToTarget <= AngleToFire;

        return isAimed;
    }
}