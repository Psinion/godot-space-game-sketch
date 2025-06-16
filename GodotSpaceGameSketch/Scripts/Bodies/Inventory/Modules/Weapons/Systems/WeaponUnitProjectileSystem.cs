using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;
using GodotSpaceGameSketch.DataBases.Factories;
using GodotSpaceGameSketch.Movement.Components;
using GodotSpaceGameSketch.Pooling.Components;
using FlyweightFactory = GodotSpaceGameSketch.DataBases.Factories.FlyweightFactory;
using Flyweights_ProjectileFlyweightView = GodotSpaceGameSketch.Pooling.Flyweights.ProjectileFlyweightView;
using ProjectileFlyweightView = GodotSpaceGameSketch.Pooling.Flyweights.ProjectileFlyweightView;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Systems;

public class WeaponUnitProjectileSystem : QuerySystem<CelestialBodyPosition, CelestialBodyRotation, WeaponUnit>
{
    public WeaponUnitProjectileSystem()
    {
        Filter
            .WithoutAnyTags(Tags.Get<DeathEvent>())
            .AnyTags(Tags.Get<WeaponUnitAttackState>());
    }
        
    protected override void OnUpdate()
    {
        var commandBuffer = CommandBuffer;
        Query.ForEachEntity((
            ref CelestialBodyPosition position,
            ref CelestialBodyRotation rotation,
            ref WeaponUnit weaponUnit,
            Entity entity) =>
        {
            weaponUnit.fireRateEstimated -= Tick.deltaTime;
            if (weaponUnit.fireRateEstimated <= 0)
            {
                var projectile = FlyweightFactory.Spawn(weaponUnit.projectileType, position.value, rotation.basis.GetRotationQuaternion()) as Flyweights_ProjectileFlyweightView;
                weaponUnit.fireRateEstimated = weaponUnit.fireRate;

                var projectileEntity = commandBuffer.CreateEntity();
                commandBuffer.AddComponent(projectileEntity, new CharacterBodyRef(projectile));
                commandBuffer.AddComponent(projectileEntity, new CelestialBodyPosition(position.value));
                commandBuffer.AddComponent(projectileEntity, new CelestialBodyRotation(rotation.basis));
                commandBuffer.AddComponent(projectileEntity, new UnControlMovable(weaponUnit.projectileSpeed));
                commandBuffer.AddComponent(projectileEntity, new Projectile());
                commandBuffer.AddComponent(projectileEntity, new Damage(40));
                commandBuffer.AddComponent(projectileEntity, new Flyweight(projectile));
                commandBuffer.AddComponent(projectileEntity, new FlyweightLifeTime(10f));
            }
        });
    }
}