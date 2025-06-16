using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Godot;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Movement.Components;

namespace GodotSpaceGameSketch.Movement.Systems;

public class ThrustersSystem : QuerySystem<VehicleMovable, ThrusterEffects>
{
    public ThrustersSystem()
    {
        Filter.WithoutAnyTags(Tags.Get<DeathEvent>());
    }
    
    protected override void OnUpdate()
    {
        Query.ForEachEntity((
            ref VehicleMovable movable,
            ref ThrusterEffects thrusters,
            Entity entity) =>
        {
            var t = Mathf.Abs(movable.currentAcceleration / movable.accelerationMax);
            thrusters.intensity = t;
        });
    }
}