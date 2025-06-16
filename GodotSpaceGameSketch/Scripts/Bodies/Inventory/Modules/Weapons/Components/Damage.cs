using Friflo.Engine.ECS;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;

public struct Damage : IComponent
{
    public int value;

    public Damage(int value)
    {
        this.value = value;
    }
}