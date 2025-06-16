using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Tracking.Components;

namespace GodotSpaceGameSketch.Tracking.Systems;

public class ShipTrackingSystem : QuerySystem<CelestialBodyPosition, Tracker>
{
    private SpaceManager spaceManager;
    private ShipToTargetsStorage shipToTargetsStorage;
        
    public ShipTrackingSystem(SpaceManager spaceManager, ShipToTargetsStorage shipToTargetsStorage)
    {
        this.spaceManager = spaceManager;
        this.shipToTargetsStorage = shipToTargetsStorage;
        Filter
            .WithoutAnyTags(Tags.Get<DeathEvent>())
            .AnyTags(Tags.Get<CelestialBodyTag>());
    }
        
    protected override void OnUpdate()
    {
        Query.ForEachEntity((
            ref CelestialBodyPosition position,
            ref Tracker tracker,
            Entity entity) =>
        {
            tracker.scanIntervalEstimated -= Tick.deltaTime;
            if (tracker.scanIntervalEstimated > 0)
            {
                return;
            }

            var oldTargetsList = shipToTargetsStorage.Get(entity.Id);
            oldTargetsList.Clear();
            spaceManager.GetEntitiesInRadiusWithDistance(entity.Id, position.value, tracker.radius, ref oldTargetsList);

            tracker.scanIntervalEstimated = tracker.scanInterval;
        });
    }
}