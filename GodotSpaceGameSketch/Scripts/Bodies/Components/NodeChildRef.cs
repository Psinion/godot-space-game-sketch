using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.Bodies.Components;

public struct NodeChildRef : IComponent
{
    public Node3D value;

    public NodeChildRef(Node3D value)
    {
        this.value = value;
    }
}