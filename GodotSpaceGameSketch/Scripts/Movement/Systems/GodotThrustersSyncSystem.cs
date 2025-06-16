using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using GodotSpaceGameSketch.Bodies.Components;

namespace GodotSpaceGameSketch.Movement.Systems;

public class GodotThrustersSyncSystem : QuerySystem<CelestialBodyViewRef, ThrusterEffects>
{
    public GodotThrustersSyncSystem()
    {
        Filter.WithoutAnyTags(Tags.Get<DeathEvent>());
    }
    
    protected override void OnUpdate()
    {
        Query.ForEachEntity((
            ref CelestialBodyViewRef celestialBodyRef, 
            ref ThrusterEffects thrusters,
            Entity entity) =>
        {
            celestialBodyRef.value.thrustersView.UpdateEffects(thrusters.intensity);
        });
    }
}