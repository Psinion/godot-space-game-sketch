using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Godot;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;
using GodotSpaceGameSketch.Movement.Components;
using GodotSpaceGameSketch.Pooling.Components;
using GodotSpaceGameSketch.Bodies;
using GodotSpaceGameSketch.Core.Extensions;
using Bodies_CelestialBodyView = GodotSpaceGameSketch.Battle.Bodies.CelestialBodyView;
using CelestialBodyView = GodotSpaceGameSketch.Battle.Bodies.CelestialBodyView;

namespace GodotSpaceGameSketch.Movement.Systems;

public class ProjectilesMovementSystem : QuerySystem<CharacterBodyRef, Projectile, UnControlMovable>
{
    public ProjectilesMovementSystem()
    {
        Filter.WithoutAnyTags(Tags.Get<FlyweightReturnToPool>());
    }
        
    protected override void OnUpdate()
    {
        var commandBuffer = CommandBuffer;
        Query.ForEachEntity((
            ref CharacterBodyRef characterBody,
            ref Projectile projectile,
            ref UnControlMovable unControlMovable, 
            Entity projectileEntity) =>
        {
            var deltaTime = Tick.deltaTime;
            var velocityVector = characterBody.value.GlobalBasis.Forward() * unControlMovable.acceleration;
            var moveDistance = velocityVector * deltaTime;

            var collision = characterBody.value.MoveAndCollide(moveDistance);
            if (collision != null && collision.GetCollider() is Bodies_CelestialBodyView celestialBody)
            {
                var hitEntity = celestialBody.Entity;
                
                var collisionEventId = commandBuffer.CreateEntity();
                commandBuffer.AddComponent(collisionEventId, new CollisionEvent()
                {
                    sourceEntityId = projectileEntity.Id,
                    targetEntityId = hitEntity.Id,
                });
                    
                commandBuffer.AddTag<FlyweightReturnToPool>(projectileEntity.Id);
            }
        });
    }
}