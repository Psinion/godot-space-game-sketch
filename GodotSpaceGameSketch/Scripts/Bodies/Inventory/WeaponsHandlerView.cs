using Godot;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;

namespace GodotSpaceGameSketch.Bodies.Inventory;

public partial class WeaponsHandlerView : Node3D
{
    private Battle.Bodies.CelestialBodyView parent;
        
    public PlacementsHandlerView placements;

    public override void _EnterTree()
    {
        base._EnterTree();
        parent = GetParent() as Battle.Bodies.CelestialBodyView;
        placements = GetNode("../ModelWrapper/WeaponPlacements") as PlacementsHandlerView;
    }

    public void Initialise(Battle.Bodies.CelestialBodyView bodyView)
    {
        parent = bodyView;
    }

    public void Equip(WeaponModuleView module, int placementIndex)
    {
        var placement = placements.placements[placementIndex];

        ref var weaponHandler = ref parent.Entity.GetComponent<CelestialBodyWeaponHandler>();
        weaponHandler.attackRadius = module.ModuleEntity.GetComponent<WeaponModule>().attackRadius;
        
        module.Reparent(placement);
        
        module.Equip(placement, this);
        
        module.GlobalPosition = placement.GlobalPosition;
        module.GlobalRotation = placement.GlobalRotation;

        parent.Entity.AddChild(module.ModuleEntity);

        placement.Module = module;
    }
}