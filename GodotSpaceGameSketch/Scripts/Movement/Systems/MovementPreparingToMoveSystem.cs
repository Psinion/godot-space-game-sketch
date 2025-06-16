using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Godot;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Movement.Components;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Core.Extensions;

namespace GodotSpaceGameSketch.Movement.Systems;

/// <summary>
/// System handling the preparation phase before movement:
/// - Rotates entity to face target direction
/// - Manages acceleration ramp-up
/// - Transitions to active movement state when properly aligned
/// </summary>
public class MovementPreparingToMoveSystem : QuerySystem<CelestialBodyPosition, CelestialBodyRotation, VehicleMovable, CommandMove>
{
    public MovementPreparingToMoveSystem()
    {
        Filter
            .WithoutAnyTags(Tags.Get<DeathEvent>())
            .AnyTags(Tags.Get<MovementPreparingToMoveTag>());
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
            
            //PsiDebug.DrawLine(position.value, targetPosition, Colors.Blue, 3f);
                
            vehicleMovable.currentAcceleration = 0.2f;
            
            var targetDirection = (targetPosition - position.value).Normalized();
            var forward = rotation.basis.Forward();
                
            float cosAngle = targetDirection.Dot(forward);
            if (targetDirection.Length() <= 6.5f ? cosAngle > 0.99f : cosAngle > 0.8f)
            {
                buffer.RemoveTag<MovementPreparingToMoveTag>(entity.Id);
                buffer.AddTag<MovementMoveToTargetDestinationTag>(entity.Id);
                return;
            }

            MovementFunctions.Rotate(Tick.deltaTime, ref rotation, ref vehicleMovable, ref targetDirection);
        });
    }
}