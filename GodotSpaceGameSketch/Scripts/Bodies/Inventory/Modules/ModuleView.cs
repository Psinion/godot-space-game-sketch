using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.Bodies.Inventory.Modules;

public partial class ModuleView : Node3D
{
    protected Entity moduleEntity;
    public Entity ModuleEntity => moduleEntity;
        
    [Export]
    protected PlacementView parent;

    public PlacementView Parent => parent;

    public virtual void Initialise(Entity entity)
    {
        moduleEntity = entity;
    }
    
    public virtual void Equip(PlacementView parent)
    {
        this.parent = parent;
    }
}