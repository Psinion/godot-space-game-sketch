using Godot;

namespace GodotSpaceGameSketch.Shared.Interaction.Controllers;

public abstract partial class Controller : Node3D
{
    public abstract void LeftClick(Vector2 mousePosition);

    public abstract void RightClick(Vector2 mousePosition);
}