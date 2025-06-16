using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;
using GodotSpaceGameSketch.Core.Extensions;

namespace GodotSpaceGameSketch.Bodies.Systems;

public class ModulesDeathSystem : QuerySystem<NodeRef, WeaponModule>
{
    public ModulesDeathSystem()
    {
        Filter.AnyTags(Tags.Get<DeathEvent>());
    }
        
    protected override void OnUpdate()
    {
        var commandBuffer = CommandBuffer;
        Query.ForEachEntity((
            ref NodeRef nodeRef,
            ref WeaponModule weaponModule,
            Entity entityToDeath) =>
        {
            var body = nodeRef.value;
            
            //var explosion = FlyweightFactory.Spawn(FlyweightType.ExplosionMedium, body.Position) as ExplosionFlyweightView;
            
            //var explosionEntity = commandBuffer.CreateEntity();
            //commandBuffer.AddComponent(explosionEntity, new Flyweight(explosion));
            //commandBuffer.AddComponent(explosionEntity, new FlyweightLifeTime(5f));
            
            commandBuffer.DeleteEntityWithChildren(entityToDeath);
            //commandBuffer.DeleteEntity(entityToDeath.Id);

            /*foreach (var childId in entityToDeath.ChildIds)
            {
                commandBuffer.AddTag<DeathEvent>(childId);
            }*/

            /*if (nodeRef.value != null)
            {
                //body.Destroy();
            }*/
        });
    }
}