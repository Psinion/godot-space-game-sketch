using Friflo.Engine.ECS;

namespace GodotSpaceGameSketch.Movement.Components;

public struct UnControlMovable : IComponent
{
    public float acceleration;

    public UnControlMovable(float acceleration)
    {
        this.acceleration = acceleration;
    }
}