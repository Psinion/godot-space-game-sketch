using Friflo.Engine.ECS;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;

public struct WeaponModule : IComponent
{
    public int attackRadius;
    public Entity target;
}