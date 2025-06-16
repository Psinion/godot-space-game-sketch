using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Movement.Components;

namespace GodotSpaceGameSketch.Movement.Systems;

public class GodotCelestialBodySyncSystem : QuerySystem<CelestialBodyViewRef, CelestialBodyPosition, CelestialBodyRotation>
{
    public GodotCelestialBodySyncSystem()
    {
        Filter.WithoutAnyTags(Tags.Get<DeathEvent>());
    }
    
    protected override void OnUpdate()
    {
        Query.ForEachEntity((
            ref CelestialBodyViewRef celestialBodyRef, 
            ref CelestialBodyPosition position,
            ref CelestialBodyRotation rotation,
            Entity entity) =>
        {
            celestialBodyRef.value.GlobalPosition = position.value;
            celestialBodyRef.value.GlobalBasis = rotation.basis;
        });
    }
}