using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.Movement.Components;

public struct CommandMove : IComponent
{
    public Vector3 targetPosition;

    public CommandMove(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}