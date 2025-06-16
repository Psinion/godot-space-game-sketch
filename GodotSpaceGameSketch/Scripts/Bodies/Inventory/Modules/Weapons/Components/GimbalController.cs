using Friflo.Engine.ECS;
using Godot;
using GodotSpaceGameSketch.Core.Utilities;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;

public struct GimbalController : IComponent
{
    public Node3D horizontalPivot;
    public Node3D verticalPivot;
        
    public bool hasConstraintsHorizontal;
    public float minPivotAngleHorizontal;
    public float maxPivotAngleHorizontal;

    public bool hasConstraintsVertical;
    public float minPivotAngleVertical;
    public float maxPivotAngleVertical;

    public bool isHorizontalPivotRest;
    public bool isVerticalPivotRest;

    public PID axisPidControllerX;
    public PID axisPidControllerY;
}