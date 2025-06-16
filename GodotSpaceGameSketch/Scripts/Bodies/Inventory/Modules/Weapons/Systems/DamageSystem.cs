using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;
using GodotSpaceGameSketch.Core.Extensions;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Systems;

public class DamageSystem : QuerySystem<CollisionEvent>
{
    protected override void OnUpdate()
    {
        var commandBuffer = CommandBuffer;
        Query.ForEachEntity((
            ref CollisionEvent collisionEvent,
            Entity eventId) =>
        {
            var entityStore = commandBuffer.EntityStore;
            var sourceEntity = entityStore.GetEntityById(collisionEvent.sourceEntityId);
            var targetEntity = entityStore.GetEntityById(collisionEvent.targetEntityId);
                
            if (sourceEntity.TryGetComponent(out Damage damageComponent)
                && targetEntity.HasComponent<Health>())
            {
                ref var healthComponent = ref targetEntity.GetComponent<Health>();
                    
                healthComponent.value -= damageComponent.value;

                if (healthComponent.value <= 0)
                {
                    commandBuffer.AddDeathTagWithChildren(targetEntity);
                    //commandBuffer.AddTag<DeathEvent>(targetEntity.Id);
                }
            }
                
            commandBuffer.DeleteEntity(eventId.Id);
        });
    }
}