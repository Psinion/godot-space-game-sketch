using Friflo.Engine.ECS;
using Godot;
using GodotSpaceGameSketch.Pooling.Enums;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;

public struct WeaponUnit : IComponent
{
    public float fireRate;
    public float fireRateEstimated;
    
    public FlyweightType projectileType;
    public float projectileSpeed;

    public WeaponUnit(float fireRate, FlyweightType projectileType, float projectileSpeed)
    {
        this.fireRate = fireRate;
        this.fireRateEstimated = GD.Randf() * fireRate;
        this.projectileType = projectileType;
        this.projectileSpeed = projectileSpeed;
    }
}