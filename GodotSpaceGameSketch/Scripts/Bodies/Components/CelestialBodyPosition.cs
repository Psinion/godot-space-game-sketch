using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.Bodies.Components;

public struct CelestialBodyPosition : IComponent
{
    public Vector3 value;

    public CelestialBodyPosition(Vector3 value)
    {
        this.value = value;
    }
}