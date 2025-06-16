using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Godot;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;
using GodotSpaceGameSketch.Tracking;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Systems;

public class TurretTargetFindingSystem : QuerySystem<WeaponModule, GimbalController, WeaponTurret>
{
    private ShipToTargetsStorage shipToTargetsStorage;
        
    public TurretTargetFindingSystem(ShipToTargetsStorage shipToTargetsStorage)
    {
        this.shipToTargetsStorage = shipToTargetsStorage;
            
        Filter
            .WithoutAnyTags(Tags.Get<DeathEvent>())
            .AnyTags(Tags.Get<WeaponTurretFindTargetState>());
    }
        
    protected override void OnUpdate()
    {
        var commandBuffer = CommandBuffer;
        Query.ForEachEntity((
            ref WeaponModule weaponModule,
            ref GimbalController gimbalController, 
            ref WeaponTurret weaponTurret, 
            Entity entity) =>
        {
            if (gimbalController.isHorizontalPivotRest && gimbalController.isVerticalPivotRest)
            {
                GimbalControllerFunctions.RotateToIdle(Tick.deltaTime, ref gimbalController, ref weaponTurret);
            }

            if (entity.Parent.HasComponent<CommandTarget>())
            {
                var target = entity.Parent.GetComponent<CommandTarget>();
                if (!target.target.IsNull)
                {
                    ref var shipPosition = ref entity.Parent.GetComponent<CelestialBodyPosition>();
                    ref var shipOwner = ref entity.Parent.GetComponent<Owner>();
                
                    ref var targetPosition = ref target.target.GetComponent<CelestialBodyPosition>();
                    ref var targetOwner = ref target.target.GetComponent<Owner>();
                
                    TryAttack(ref commandBuffer, ref weaponModule, ref entity, shipOwner.id, 
                        shipPosition.value.DistanceTo(targetPosition.value), weaponModule.attackRadius, ref target.target, targetOwner.id);
                    return;
                }
            }
                
            var possibleTargets = shipToTargetsStorage.Get(entity.Parent.Id);
            if (possibleTargets.Count > 0)
            {
                ref var shipOwner = ref entity.Parent.GetComponent<Owner>();
                
                var nearestObject = possibleTargets[0];
                TryAttack(ref commandBuffer, ref weaponModule, ref entity, shipOwner.id, nearestObject.distance, 
                    weaponModule.attackRadius, ref nearestObject.entity, nearestObject.owner);
            }
        });
    }

    private void TryAttack(
        ref CommandBuffer commandBuffer,
        ref WeaponModule weaponModule,
        ref Entity shipEntity, 
        int shipOwner,
        float distanceToTarget,
        float turretAttackRadius, 
        ref Entity target, 
        int targetOwner)
    {
        if (!target.IsNull
            && targetOwner != shipOwner
            && distanceToTarget <= turretAttackRadius)
        {
            weaponModule.target = target;
            commandBuffer.RemoveTag<WeaponTurretFindTargetState>(shipEntity.Id);
            commandBuffer.AddTag<WeaponTurretAttackState>(shipEntity.Id);
        }
    }
}