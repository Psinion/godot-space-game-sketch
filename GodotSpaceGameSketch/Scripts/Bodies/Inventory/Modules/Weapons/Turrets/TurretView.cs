using Godot;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Turrets;

/// <summary>
/// Оружейный модуль, который умеет поворачиваться в сторону цели.
/// </summary>
public partial class TurretView : WeaponModuleView
{
    [Export]
    private Node3D horizontalPivot;
    public Node3D HorizontalPivot => horizontalPivot;
    
    [Export]
    private Node3D verticalPivot;
    public Node3D VerticalPivot => verticalPivot;
}