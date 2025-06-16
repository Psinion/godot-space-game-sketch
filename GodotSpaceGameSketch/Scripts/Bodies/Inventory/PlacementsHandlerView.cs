using Godot;
using Godot.Collections;

namespace GodotSpaceGameSketch.Bodies.Inventory;

public partial class PlacementsHandlerView : Node3D
{
    public Array<PlacementView> placements = new();
    
    public override void _EnterTree()
    {
        base._Ready();
        var children = GetChildren();
        foreach (var child in children)
        {
            if (child is PlacementView placement)
            {
                placements.Add(placement);
            }
        }
    }
}