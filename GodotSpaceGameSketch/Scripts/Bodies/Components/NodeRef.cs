using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.Bodies.Components;

public struct NodeRef : IComponent
{
    public Node3D value;

    public NodeRef(Node3D value)
    {
        this.value = value;
    }
}