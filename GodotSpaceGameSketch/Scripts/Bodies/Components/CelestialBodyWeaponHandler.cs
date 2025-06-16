using Friflo.Engine.ECS;
using GodotSpaceGameSketch.Bodies.Enums;

namespace GodotSpaceGameSketch.Bodies.Components;

public struct CelestialBodyWeaponHandler : IComponent 
{
    public WeaponPosition weaponPosition;
    public float attackRadius = 0f;

    public CelestialBodyWeaponHandler(WeaponPosition weaponPosition)
    {
        this.weaponPosition = weaponPosition;
    }
}