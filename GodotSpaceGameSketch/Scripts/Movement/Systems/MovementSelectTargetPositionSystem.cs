using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Godot;
using GodotSpaceGameSketch.AI.Components;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Enums;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Movement.Components;
using GodotSpaceGameSketch.Core.Extensions;

namespace GodotSpaceGameSketch.Movement.Systems;

/// <summary>
/// System responsible for calculating and updating target positions for entities with movement capabilities.
/// - Predicts target position based on target's velocity for leading shots/movement
/// - Applies vertical offsets based on weapon positioning
/// - Generates random spread patterns for evasive maneuvers
/// - Creates movement commands for entities
/// </summary>
public class MovementSelectTargetPositionSystem : QuerySystem<CelestialBodyMoveSelector, CelestialBodyWeaponHandler, CelestialBodyPosition, CommandTarget>
{
    private const float PredictionFactor = 1.2f;
    
    public MovementSelectTargetPositionSystem()
    {
        Filter
            .WithoutAnyTags(Tags.Get<DeathEvent>())
            .AnyTags(Tags.Get<MovementPreparingToMoveTag, MovementMoveToTargetDestinationTag>());
    }
        
    protected override void OnUpdate()
    {
        var buffer = CommandBuffer;
        Query.ForEachEntity((
            ref CelestialBodyMoveSelector moveSelector, 
            ref CelestialBodyWeaponHandler weaponHandler, 
            ref CelestialBodyPosition position, 
            ref CommandTarget target,
            Entity entity) =>
        {
            if (target.target.IsNull)
            {
                buffer.RemoveComponent<CommandTarget>(entity.Id);
                return;
            }

            var deltaTime = Tick.deltaTime;
            if (moveSelector.targetRefreshTimeCurrent > 0)
            {
                moveSelector.targetRefreshTimeCurrent -= deltaTime;
                moveSelector.randomSpreadRefreshTimeCurrent -= deltaTime;
                return;
            }

            moveSelector.targetRefreshTimeCurrent = moveSelector.targetRefreshTimeMax;
            
            var targetEntityPosition = target.target.GetComponent<CelestialBodyPosition>().value;
            var targetEntityRotation = target.target.GetComponent<CelestialBodyRotation>();
                
            var velocity = Vector3.Zero;
            if (target.target.HasComponent<VehicleMovable>())
            {
                velocity = target.target.GetComponent<VehicleMovable>().velocity;
            }
                
            var predictedPosition = CalculatePredictedPosition(
                ref position.value, 
                ref targetEntityPosition, 
                ref velocity,
                40f);
                
            var verticalOffset = GetVerticalOffset(ref weaponHandler, ref targetEntityRotation);

            if (moveSelector.randomSpreadRefreshTimeCurrent <= 0)
            {
                var halfAttackRadius = (double)weaponHandler.attackRadius * 0.5f;
                moveSelector.randomSpread = new Vector3(
                    (float) GD.RandRange(-halfAttackRadius, halfAttackRadius),
                    (float) GD.RandRange(-halfAttackRadius * 0.5f, halfAttackRadius * 0.5f),
                    (float) GD.RandRange(-halfAttackRadius, halfAttackRadius)
                );
                moveSelector.randomSpreadRefreshTimeCurrent = moveSelector.randomSpreadRefreshTimeMax;
            }

            var positionToMove = predictedPosition + verticalOffset + moveSelector.randomSpread;

            if (entity.HasComponent<CommandMove>())
            {
                ref var moveCommand = ref entity.GetComponent<CommandMove>();
                moveCommand.targetPosition = positionToMove;
            }
            else
            {
                buffer.AddComponent(entity.Id, new CommandMove(positionToMove));
            }
            
            //PsiDebug.DrawLine(position.value, positionToMove, Colors.Red, Tick.deltaTime);
        });
    }
    
    private Vector3 CalculatePredictedPosition(ref Vector3 currentPos, ref Vector3 targetPos, ref Vector3 targetVelocity, float radius)
    {
        var directionToTarget = targetPos - currentPos;
        float distance = directionToTarget.Length();
        float predictionTime = Mathf.Clamp(distance / targetVelocity.Length(), 0, 3f);
        
        return targetPos + targetVelocity * predictionTime * PredictionFactor;
    }
    
    private Vector3 GetVerticalOffset(ref CelestialBodyWeaponHandler weaponHandler, ref CelestialBodyRotation rotation)
    {
        return weaponHandler.weaponPosition switch
        {
            WeaponPosition.Top => rotation.basis.Down() * 15,
            WeaponPosition.Bottom => rotation.basis.Up() * 15,
            WeaponPosition.Front => rotation.basis.Back() * 15,
            _ => Vector3.Zero
        };
    }
}