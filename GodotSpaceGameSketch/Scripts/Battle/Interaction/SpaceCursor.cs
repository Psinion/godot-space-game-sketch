using Godot;

namespace GodotSpaceGameSketch.Battle.Interaction;

public partial class SpaceCursor : Node3D
{
    public void Move(Vector3 position)
    {
        GlobalPosition = position;
        Show();
    }
}