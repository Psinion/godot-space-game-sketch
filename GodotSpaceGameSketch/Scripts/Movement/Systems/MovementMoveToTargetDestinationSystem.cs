using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Godot;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Movement.Components;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Core.Extensions;

namespace GodotSpaceGameSketch.Movement.Systems;

/// <summary>
/// System managing active movement towards target:
/// - Handles velocity-based movement with acceleration/deceleration
/// - Maintains proper orientation during movement
/// - Implements obstacle avoidance (potential improvement area)
/// - Manages movement state transitions
/// </summary>
public class MovementMoveToTargetDestinationSystem : QuerySystem<CelestialBodyPosition, CelestialBodyRotation, VehicleMovable, CommandMove>
{
    public MovementMoveToTargetDestinationSystem()
    {
        Filter
            .WithoutAnyTags(Tags.Get<DeathEvent>())
            .AnyTags(Tags.Get<MovementMoveToTargetDestinationTag>());
    }
        
    protected override void OnUpdate()
    {
        var buffer = CommandBuffer;
        Query.ForEachEntity((
            ref CelestialBodyPosition position, 
            ref CelestialBodyRotation rotation, 
            ref VehicleMovable vehicleMovable,
            ref CommandMove moveCommand, 
            Entity entity) =>
        {
            var targetPosition = moveCommand.targetPosition;
            
            var targetVector = targetPosition - position.value;
            var targetDistance = targetVector.Length();
            if (targetDistance < vehicleMovable.stopDistance && !entity.HasComponent<CommandTarget>())
            {
                vehicleMovable.velocity = Vector3.Zero;
                vehicleMovable.currentAcceleration = 0;
                buffer.RemoveTag<MovementMoveToTargetDestinationTag>(entity.Id);
                buffer.RemoveComponent<CommandTarget>(entity.Id);
                return;
            }

            var deltaTime = Tick.deltaTime;
            var targetDirection = targetVector.Normalized();
            MovementFunctions.Rotate(deltaTime, ref rotation, ref vehicleMovable, ref targetDirection);
                
            //PsiDebug.DrawRay(position.value, rotation.basis.Forward() * 10, Colors.Blue);
            //PsiDebug.DrawRay(position.value, rotation.basis.Right() * 10, Colors.Red);    
            //PsiDebug.DrawRay(position.value, rotation.basis.Up() * 10, Colors.Green); 
            
            // Ускорение
            // При развороте ускорение < 0, чтобы не было выхлопа
            float alignment = targetDirection.Dot(rotation.basis.Forward());
            
            float brakeFactor = Mathf.Clamp(targetDistance / (vehicleMovable.stopDistance * 10f), 0.1f, 1f);
            
            float desiredSpeed = vehicleMovable.moveSpeedMax * brakeFactor;
            float desiredAcceleration = vehicleMovable.accelerationMax * brakeFactor;
            
            var accelerateValue = desiredAcceleration > vehicleMovable.currentAcceleration
                ? vehicleMovable.decelerationMax
                : desiredAcceleration;
            var accelerationRate = Mathf.Lerp(
                vehicleMovable.currentAcceleration,
                accelerateValue * alignment,
                accelerateValue * deltaTime
            );
            //GD.Print("accelerateValue: ", accelerateValue);
                
            var idealVelocity = targetDirection * desiredSpeed;
            var velocityDelta = idealVelocity - vehicleMovable.velocity;
                
            vehicleMovable.velocity += velocityDelta.Normalized() * accelerationRate * deltaTime;
            if (vehicleMovable.velocity.Length() > vehicleMovable.moveSpeedMax)
            {
                vehicleMovable.velocity = vehicleMovable.velocity.Normalized() * vehicleMovable.moveSpeedMax;
                
                // При максимальной скорости обнуляем ускорение, чтобы убрать выхлоп.
                vehicleMovable.currentAcceleration = 0;
            }
            else
            {
                vehicleMovable.currentAcceleration = accelerationRate;
            }
            
            position.value += vehicleMovable.velocity * deltaTime;
        });
    }
}