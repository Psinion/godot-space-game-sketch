using Friflo.Engine.ECS;

namespace GodotSpaceGameSketch.Bodies.Components;

public struct CommandTarget : IComponent
{
    public Entity target;
    
    public CommandTarget(Entity target)
    {
        this.target = target;
    }
}