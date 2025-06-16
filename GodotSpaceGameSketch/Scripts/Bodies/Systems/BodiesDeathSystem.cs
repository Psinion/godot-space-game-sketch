using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.DataBases.Factories;
using GodotSpaceGameSketch.Pooling.Components;
using GodotSpaceGameSketch.Pooling.Enums;
using GodotSpaceGameSketch.Pooling.Flyweights;

namespace GodotSpaceGameSketch.Bodies.Systems;

public class BodiesDeathSystem : QuerySystem<CelestialBodyViewRef>
{
    public BodiesDeathSystem()
    {
        Filter.AnyTags(Tags.Get<DeathEvent>());
    }
        
    protected override void OnUpdate()
    {
        var commandBuffer = CommandBuffer;
        Query.ForEachEntity((
            ref CelestialBodyViewRef celestialBodyRef,
            Entity entityToDeath) =>
        {
            var body = celestialBodyRef.value;
            
            var explosion = FlyweightFactory.Spawn(FlyweightType.ExplosionMedium, body.Position) as ExplosionFlyweightView;
            
            var explosionEntity = commandBuffer.CreateEntity();
            commandBuffer.AddComponent(explosionEntity, new Flyweight(explosion));
            commandBuffer.AddComponent(explosionEntity, new FlyweightLifeTime(5f));
            
            WorldManager.Instance.ShipToTargetsStorage.Unregister(entityToDeath.Id);
            commandBuffer.DeleteEntity(entityToDeath.Id);
            
            body.Destroy();
        });
    }
}