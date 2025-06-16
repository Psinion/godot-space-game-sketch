using Godot;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons;

/// <summary>
/// Ствол оружия.
/// </summary>
public partial class WeaponUnitView : Node3D
{
    private WeaponModuleView weaponModule;
    public WeaponModuleView WeaponModule => weaponModule;
        
    public void Initialize(WeaponModuleView weaponModule)
    {
        this.weaponModule = weaponModule;
    }
        
    public virtual void Fire(Battle.Bodies.CelestialBodyView target)
    {
        //Debug.Log("Fire");
    }
}