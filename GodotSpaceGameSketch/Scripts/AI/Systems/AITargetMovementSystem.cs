using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Godot;
using GodotSpaceGameSketch.AI.Components;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Movement;
using GodotSpaceGameSketch.Movement.Components;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Core.Extensions;
using GodotSpaceGameSketch.DataBases.Factories;
using GodotSpaceGameSketch.Pooling.Enums;

namespace GodotSpaceGameSketch.AI.Systems;

/// <summary>
/// AI system for target engagement:
/// - Initiates movement towards selected targets
/// - Coordinates with movement systems
/// - Acts as bridge between AI logic and movement systems
/// </summary>
public class AITargetMovementSystem : QuerySystem<CelestialBodyPosition, CelestialBodyRotation, CelestialBodyAI, CommandTarget>
{
    public AITargetMovementSystem()
    {
        Filter
            .WithoutAnyTags(Tags.Get<DeathEvent, MovementPreparingToMoveTag, MovementMoveToTargetDestinationTag>())
            .AnyTags(Tags.Get<CelestialBodyTag, ControlledByAI>())
            ;
    }
        
    protected override void OnUpdate()
    {
        Query.ForEachEntity((
            ref CelestialBodyPosition celestialBodyPosition,
            ref CelestialBodyRotation CelestialBodyRotation,
            ref CelestialBodyAI celestialBodyAI,
            ref CommandTarget celestialBodyTarget,
            Entity spaceShip) =>
        {
            var targetPosition = celestialBodyTarget.target.GetComponent<CelestialBodyPosition>();
            MovementFunctions.EngageMovement(
                ref spaceShip, 
                ref celestialBodyPosition.value,
                ref CelestialBodyRotation.basis, 
                ref targetPosition.value);
        });
    }
}