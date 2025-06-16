using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using GodotSpaceGameSketch.DataBases.Factories;
using GodotSpaceGameSketch.Pooling.Components;

namespace GodotSpaceGameSketch.Pooling.Systems;

public class FlyweightReturnToPoolSystem : QuerySystem<Flyweight>
{
    public FlyweightReturnToPoolSystem()
    {
        Filter.AnyTags(Tags.Get<FlyweightReturnToPool>());
    }
        
    protected override void OnUpdate()
    {
        var commandBuffer = CommandBuffer;
        Query.ForEachEntity((
            ref Flyweight flyweight,
            Entity projectileEntity) =>
        {
            FlyweightFactory.ReturnToPool(flyweight.flyweightLink);
            commandBuffer.DeleteEntity(projectileEntity.Id);
        });
    }
}