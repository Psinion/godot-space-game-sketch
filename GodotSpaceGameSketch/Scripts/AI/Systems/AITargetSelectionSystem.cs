using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Godot;
using GodotSpaceGameSketch.AI.Components;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Movement.Components;
using GodotSpaceGameSketch.Tracking;
using GodotSpaceGameSketch.Tracking.Components;

namespace GodotSpaceGameSketch.AI.Systems;

/// <summary>
/// AI system for target selection:
/// - Periodically scans for valid targets using ShipToTargetsStorage
/// - Filters targets by ownership and distance
/// - Issues target commands to AI entities
/// </summary>
public class AITargetSelectionSystem : QuerySystem<CelestialBodyAI, Owner, Tracker>
{
    private readonly ShipToTargetsStorage shipToTargetsStorage;
    
    public AITargetSelectionSystem(ShipToTargetsStorage shipToTargetsStorage)
    {
        this.shipToTargetsStorage = shipToTargetsStorage;
        
        Filter
            .WithoutAnyTags(Tags.Get<DeathEvent>())
            .AnyTags(Tags.Get<CelestialBodyTag, ControlledByAI>());
    }
        
    protected override void OnUpdate()
    {
        var commandBuffer = CommandBuffer;
        Query.ForEachEntity((
            ref CelestialBodyAI celestialBodyAI,
            ref Owner owner,
            ref Tracker tracker,
            Entity spaceShip) =>
        {
            var deltaTime = Tick.deltaTime;
            
            if (celestialBodyAI.targetRefreshTimeCurrent <= 0)
            {
                var possibleTargets = shipToTargetsStorage.Get(spaceShip.Id);
                if (possibleTargets.Count > 0)
                {
                    foreach (var target in possibleTargets)
                    {
                        if (!target.entity.IsNull && target.owner != owner.id && target.distance <= tracker.radius)
                        {
                            commandBuffer.AddComponent(spaceShip.Id, new CommandTarget(target.entity));
                            commandBuffer.AddTag<MovementMoveToTargetDestinationTag>(spaceShip.Id);
                            return;
                        }
                    }
                }
                celestialBodyAI.targetRefreshTimeCurrent = celestialBodyAI.targetRefreshTimeMax;
                return;
            }
            celestialBodyAI.targetRefreshTimeCurrent -= deltaTime;
        });
    }
}