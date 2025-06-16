using Godot;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Enums;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Turrets;
using GodotSpaceGameSketch.Bodies.Samples;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Core.Utilities;
using GodotSpaceGameSketch.Core.Extensions;

namespace GodotSpaceGameSketch.DataBases.Factories;

public partial class ModuleFactory : Singleton<ModuleFactory>
{
    private PrefabsDb prefabsDb;
    
    private Node3D spawnNode;
    
    public ModuleFactory(PrefabsDb prefabsDb, Node3D spawnNode)
    {
        this.prefabsDb = prefabsDb;
        this.spawnNode = spawnNode;
    }
    
    public TurretView SpawnTurret(TurretType type)
    {
        var sample = prefabsDb.GetWeaponTurretSample(type);
        var turretViewInstance = sample.sample.Instantiate<TurretView>();
        spawnNode.AddChild(turretViewInstance);
        
        Register(turretViewInstance, sample);
        
        return turretViewInstance;
    }
    
    private void Register(TurretView turretView, TurretSample turretSample)
    {
        var world = WorldManager.Instance.World;
        var moduleEntity = world.CreateEntity();
        
        turretView.Initialise(moduleEntity);
        
        moduleEntity.AddComponent(new WeaponModule()
        {
            target = default,
            attackRadius = turretSample.attackRadius,
        });
        
        moduleEntity.AddComponent(new NodeRef(turretView));

        moduleEntity.AddComponent(new WeaponTurret()
        {
            rotationSpeed = turretSample.rotationSpeed,
            projectileSpeed = turretSample.projectileSpeed,
            leadPredictionFactor = turretSample.leadPredictionFactor,
            maxLeadAngle = turretSample.maxLeadAngle,
        });
        moduleEntity.AddComponent(new GimbalController()
        {
            horizontalPivot = turretView.HorizontalPivot,
            hasConstraintsHorizontal = turretSample.hasConstraintsHorizontal,
            minPivotAngleHorizontal = turretSample.minPivotAngleHorizontal,
            maxPivotAngleHorizontal = turretSample.maxPivotAngleHorizontal,
                
            verticalPivot = turretView.VerticalPivot,
            hasConstraintsVertical = turretSample.hasConstraintsVertical,
            minPivotAngleVertical = turretSample.minPivotAngleVertical,
            maxPivotAngleVertical = turretSample.maxPivotAngleVertical,
                
            isHorizontalPivotRest = false,
            isVerticalPivotRest = false,

            axisPidControllerX = new PID(1, 0, 0.01f),
            axisPidControllerY = new PID(1, 0, 0.01f),
        });
        moduleEntity.AddTag<WeaponTurretFindTargetState>();
        
        var weaponUnitsChildren = turretView.GetNodeInChildren<WeaponUnitView>(true);
            
        foreach (var weaponUnit in weaponUnitsChildren)
        {
            weaponUnit.Initialize(turretView);
                
            var entity = world.CreateEntity();
            entity.AddComponent(new NodeChildRef(weaponUnit));
            entity.AddComponent(new CelestialBodyPosition(weaponUnit.GlobalPosition));
            entity.AddComponent(new CelestialBodyRotation(weaponUnit.GlobalBasis));
            entity.AddComponent(new WeaponUnit(turretSample.fireRate, turretSample.flyweightType, turretSample.projectileSpeed));
            moduleEntity.AddChild(entity);
        }
    }
}