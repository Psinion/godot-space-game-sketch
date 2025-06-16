using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using GodotSpaceGameSketch.Bodies.Components;

namespace GodotSpaceGameSketch.Movement.Systems;

public class GodotTransformChildrenSyncSystem : QuerySystem<NodeChildRef, CelestialBodyPosition, CelestialBodyRotation>
{
    public GodotTransformChildrenSyncSystem()
    {
        Filter.WithoutAnyTags(Tags.Get<DeathEvent>());
    }
    
    protected override void OnUpdate()
    {
        Query.ForEachEntity((
            ref NodeChildRef nodeRef, 
            ref CelestialBodyPosition position,
            ref CelestialBodyRotation rotation,
            Entity entity) =>
        {
            position.value = nodeRef.value.GlobalPosition;
            rotation.basis = nodeRef.value.GlobalBasis;
        });
    }
}