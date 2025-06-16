namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons;

/// <summary>
/// Репрезентация установленного в корабле оружия. Заведует поиском цели для стрельбы.
/// </summary>
public partial class WeaponModuleView : ModuleView
{
    protected WeaponsHandlerView weaponsHandlerView;
        
    public WeaponsHandlerView WeaponsHandlerView => weaponsHandlerView;
        
    public virtual void Equip(PlacementView parent, WeaponsHandlerView weaponsHandlerView)
    {
        base.Equip(parent);
        this.weaponsHandlerView = weaponsHandlerView;
    }
}