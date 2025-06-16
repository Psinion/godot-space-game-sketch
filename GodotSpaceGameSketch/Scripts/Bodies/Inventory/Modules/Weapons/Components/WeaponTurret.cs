using Friflo.Engine.ECS;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;

public struct WeaponTurret : IComponent
{
    public float rotationSpeed;
    public float projectileSpeed;
    public float leadPredictionFactor;
    public float maxLeadAngle;
}