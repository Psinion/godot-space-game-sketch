using Friflo.Engine.ECS;

namespace GodotSpaceGameSketch.Bodies.Components;

public struct CollisionEvent : IComponent
{
    public int sourceEntityId;
    public int targetEntityId;
}