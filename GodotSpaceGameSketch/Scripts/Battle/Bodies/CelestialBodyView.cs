using Friflo.Engine.ECS;
using Godot;
using GodotSpaceGameSketch.Bodies;
using GodotSpaceGameSketch.Bodies.Enums;
using GodotSpaceGameSketch.Bodies.Inventory;

namespace GodotSpaceGameSketch.Battle.Bodies;

public partial class CelestialBodyView : CharacterBody3D
{
    [Export]
    private SpaceShipType type;
    public SpaceShipType Type => type;
    
    private Entity entity;
    public Entity Entity => entity;
    
    private WeaponsHandlerView weaponsHandlerView;
    public WeaponsHandlerView WeaponsHandlerView => weaponsHandlerView;
    
    [Export]
    private int shipOwnerId = 0;
    public int ShipOwnerId => shipOwnerId;

    [Export]
    public ThrustersView thrustersView;
    
    public void Initialise(SpaceShipType type, Entity entity, int shipOwnerId, WeaponsHandlerView weaponsHandlerView)
    {
        this.type = type;
        this.entity = entity;
        this.shipOwnerId = shipOwnerId;
        this.weaponsHandlerView = weaponsHandlerView;
    }

    public override void _EnterTree()
    {
        thrustersView = GetNode<ThrustersView>("Thrusters");
    }

    public override void _Ready()
    {
        if (weaponsHandlerView != null)
        {
            weaponsHandlerView.Initialise(this);
        }
    }
    
    public void Destroy()
    {
        QueueFree();
    }
    
    public void DestroyWithEntity()
    {
        Entity.DeleteEntity();
        QueueFree();
    }
}