using Friflo.Engine.ECS;
using Friflo.Engine.ECS.Systems;
using Godot;
using GodotSpaceGameSketch.AI.Systems;
using GodotSpaceGameSketch.Battle.Bodies;
using GodotSpaceGameSketch.Battle.Bodies.CameraSystem;
using GodotSpaceGameSketch.Battle.Interaction.Controllers;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Enums;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Systems;
using GodotSpaceGameSketch.Bodies.Systems;
using GodotSpaceGameSketch.DataBases;
using GodotSpaceGameSketch.DataBases.Factories;
using GodotSpaceGameSketch.Movement.Systems;
using GodotSpaceGameSketch.Pooling.Systems;
using GodotSpaceGameSketch.Tracking;
using GodotSpaceGameSketch.Tracking.Systems;
using GodotSpaceGameSketch.Core.Extensions;

namespace GodotSpaceGameSketch.Core;

public partial class WorldManager : SingletonNode3D<WorldManager>
{
    private EntityStore world;
    public EntityStore World => world;
    
    private SystemRoot systemRoot;
    private SystemRoot fixedSystemRoot;

    private PhysicsDirectSpaceState3D spaceState3D;
    public PhysicsDirectSpaceState3D SpaceState3D => spaceState3D;
    
    private PhysicsDirectSpaceState2D spaceState2D;
    public PhysicsDirectSpaceState2D SpaceState2D => spaceState2D;
    
    private ShipToTargetsStorage shipToTargetsStorage;
    public ShipToTargetsStorage ShipToTargetsStorage => shipToTargetsStorage;

    private PrefabsDb prefabsDb = new();
    public PrefabsDb PrefabsDb => prefabsDb;
    
    [Export]
    public BattleCameraEntity camera;
    
    [Export]
    public BattlePlayerController playerController;
    
    [Export]
    public SpaceManager spaceManager;
    
    [Export] private Node3D mapNode;
    [Export] private Node3D interactablesNode;
    [Export] private Node3D flyweightsNode;

    public override void _EnterTree()
    {
        base._EnterTree();
        
        prefabsDb.Initialise();

        new SpaceShipFactory(prefabsDb, interactablesNode);
        new ModuleFactory(prefabsDb, mapNode);
        new FlyweightFactory(prefabsDb, flyweightsNode);
        
        shipToTargetsStorage = new ShipToTargetsStorage();
        
        world = new EntityStore();
        systemRoot = new SystemRoot(world)
        {
            new MovementSelectTargetPositionSystem(),
            new MovementPreparingToMoveSystem(),
            new MovementMoveToTargetDestinationSystem(),
            new ThrustersSystem(),
            new ShipTrackingSystem(spaceManager, shipToTargetsStorage),
            new TurretTargetFindingSystem(shipToTargetsStorage),
            new TurretAttackSystem(),
            new WeaponUnitProjectileSystem(),
            new DamageSystem(),
            new FlyweightLifeTimeSystem(),
            
            new AITargetSelectionSystem(shipToTargetsStorage),
            new AITargetMovementSystem(),
            
            // Синхронизация с движком
            new GodotCelestialBodySyncSystem(),
            new GodotTransformChildrenSyncSystem(),
            new GodotThrustersSyncSystem(),
            
            // Очищалки
            new FlyweightReturnToPoolSystem(),
            new BodiesDeathSystem(),
            new ModulesDeathSystem(),
        };
        fixedSystemRoot = new SystemRoot(world)
        {
            new ProjectilesMovementSystem(),
        };
    }

    public override void _Ready()
    {
        base._Ready();

        var bodies = interactablesNode.GetNodeInChildren<CelestialBodyView>();
        foreach (var body in bodies)
        {
            SpaceShipFactory.Instance.SafeRegister(body);
            
            var turret2 = ModuleFactory.Instance.SpawnTurret(TurretType.Test);
            body.WeaponsHandlerView.Equip(turret2, 0);
        }
        
        var playerShip = SpaceShipFactory.Instance.Spawn(SpaceShipType.DolphinPlayer, controlByAi: false);
        playerShip.Entity.AddTag<ControlledByPlayer>();
        
        playerController.controlledShip = playerShip;
        camera.SetTarget(playerShip);

        var turret = ModuleFactory.Instance.SpawnTurret(TurretType.Test2);
        playerShip.WeaponsHandlerView.Equip(turret, 0);
        
        /*var enemyShip = SpaceShipFactory.Instance.Spawn(
            SpaceShipType.Dolphin, 
            new Vector3(GD.RandRange(-100, 100), GD.RandRange(-300, 300), GD.RandRange(-100, 100)), 
            1);
            
        var turret3 = ModuleFactory.Instance.SpawnTurret(TurretType.Test);
        enemyShip.WeaponsHandlerView.Equip(turret3, 0);*/
        
        for (int i = 0; i < 50; i++)
        {
            var enemyShip = SpaceShipFactory.Instance.Spawn(
                SpaceShipType.Dolphin, 
                new Vector3(GD.RandRange(-100, 100), GD.RandRange(-100, 100), GD.RandRange(-100, 100)), 
                GD.RandRange(0, 1));
            
            var turret3 = ModuleFactory.Instance.SpawnTurret(TurretType.Test);
            enemyShip.WeaponsHandlerView.Equip(turret3, 0);
        }
    }

    public override void _Process(double delta)
    {
        systemRoot.Update(new UpdateTick((float)delta, 0));
    }
    
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        spaceState2D = GetViewport().GetWorld2D().DirectSpaceState;
        spaceState3D = GetWorld3D().DirectSpaceState;
        fixedSystemRoot.Update(new UpdateTick((float)delta, 0));
    }
}