using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using GodotSpaceGameSketch.Pooling.Components;

namespace GodotSpaceGameSketch.Pooling.Systems;

public class FlyweightLifeTimeSystem : QuerySystem<FlyweightLifeTime>
{
    public FlyweightLifeTimeSystem()
    {
        Filter.WithoutAnyTags(Tags.Get<FlyweightReturnToPool>());
    }
        
    protected override void OnUpdate()
    {
        var commandBuffer = CommandBuffer;
        Query.ForEachEntity((
            ref FlyweightLifeTime flyweightLifeTime,
            Entity projectileEntity) =>
        {
            var deltaTime = Tick.deltaTime;
            
            if (flyweightLifeTime.lifeTimeCurrent <= 0)
            {
                commandBuffer.AddTag<FlyweightReturnToPool>(projectileEntity.Id);
                return;
            }
            flyweightLifeTime.lifeTimeCurrent -= deltaTime;
        });
    }
}