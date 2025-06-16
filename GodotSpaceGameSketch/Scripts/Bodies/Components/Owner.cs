using Friflo.Engine.ECS;

namespace GodotSpaceGameSketch.Bodies.Components;

public struct Owner : IIndexedComponent<int>
{
    public int id;
    public int GetIndexedValue() => id;
}