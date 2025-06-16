using Godot;
using GodotSpaceGameSketch.AI.Components;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Enums;
using GodotSpaceGameSketch.Bodies.Inventory;
using GodotSpaceGameSketch.Bodies.Samples;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Movement.Components;
using GodotSpaceGameSketch.Tracking.Components;
using GodotSpaceGameSketch.Bodies;
using GodotSpaceGameSketch.Core.Utilities;
using Bodies_CelestialBodyView = GodotSpaceGameSketch.Battle.Bodies.CelestialBodyView;
using CelestialBodyView = GodotSpaceGameSketch.Battle.Bodies.CelestialBodyView;

namespace GodotSpaceGameSketch.DataBases.Factories;

public partial class SpaceShipFactory : Singleton<SpaceShipFactory>
{
    private PrefabsDb prefabsDb;
    
    private Node3D spawnNode;
    
    public SpaceShipFactory(PrefabsDb prefabsDb, Node3D spawnNode)
    {
        this.prefabsDb = prefabsDb;
        this.spawnNode = spawnNode;
    }
    
    public Bodies_CelestialBodyView Spawn(
        SpaceShipType type, 
        Vector3 position, 
        Quaternion rotation, 
        int ownerId = 0, 
        bool controlByAi = true)
    {
        var sample = prefabsDb.GetSpaceShipSample(type);
        var spaceShipInstance = sample.sample.Instantiate<Bodies_CelestialBodyView>();
        
        spawnNode.AddChild(spaceShipInstance);

        spaceShipInstance.GlobalPosition = position;
        spaceShipInstance.Quaternion = rotation;
        
        Register(spaceShipInstance, sample, ownerId, controlByAi);
        
        return spaceShipInstance;
    }
    
    public Bodies_CelestialBodyView Spawn(
        SpaceShipType type, 
        Vector3 position = default, 
        int ownerId = 0, 
        bool controlByAi = true
        )
    {
        return Spawn(type, position, Quaternion.Identity, ownerId, controlByAi);
    }

    public void SafeRegister(Bodies_CelestialBodyView celestialBodyView)
    {
        if (!celestialBodyView.Entity.IsNull)
        {
            return;
        }
        
        var sample = prefabsDb.GetSpaceShipSample(celestialBodyView.Type);
        Register(celestialBodyView, sample, celestialBodyView.ShipOwnerId);
    }
    
    public void Register(
        Bodies_CelestialBodyView celestialBodyView, 
        SpaceShipSample spaceShipSample,
        int ownerId = 0,
        bool controlByAi = true
        )
    {
        var weaponsHandlerView = celestialBodyView.GetNode("WeaponsHandler") as WeaponsHandlerView;
        
        var world = WorldManager.Instance.World;
        var spaceShipEntity = world.CreateEntity();
        
        celestialBodyView.Initialise(spaceShipSample.type, spaceShipEntity, ownerId, weaponsHandlerView);
        
        spaceShipEntity.AddComponent(new CelestialBodyViewRef()
        {
            value = celestialBodyView,
        });
        spaceShipEntity.AddComponent(new CelestialBodyPosition()
        {
            value = celestialBodyView.GlobalPosition,
        });
        spaceShipEntity.AddComponent(new CelestialBodyRotation()
        {
            basis = celestialBodyView.GlobalBasis,
        });
        spaceShipEntity.AddComponent(new VehicleMovable(
            spaceShipSample.rotationSpeed,
            spaceShipSample.moveSpeedMax,
            spaceShipSample.stopDistance,
            spaceShipSample.acceleration,
            spaceShipSample.deceleration));
        spaceShipEntity.AddComponent(new ThrusterEffects());
        spaceShipEntity.AddComponent(new CelestialBodyWeaponHandler(spaceShipSample.weaponPosition));
        spaceShipEntity.AddComponent(new Health()
        {
            value = spaceShipSample.health,
        });
        spaceShipEntity.AddComponent(new Tracker(spaceShipSample.trackerScanInterval, spaceShipSample.trackerRadius));
        if (controlByAi)
        {
            spaceShipEntity.AddComponent(new CelestialBodyAI(5f, 50));
            spaceShipEntity.AddTag<ControlledByAI>();
            spaceShipEntity.AddComponent(new CelestialBodyMoveSelector(2f));
        }
        else
        {
            spaceShipEntity.AddComponent(new CelestialBodyMoveSelector(0.1f));
        }
        spaceShipEntity.AddComponent(new Owner()
        {
            id = ownerId
        });
        spaceShipEntity.AddTag<CelestialBodyTag>();
        
        WorldManager.Instance.ShipToTargetsStorage.Register(spaceShipEntity.Id);
    }
}