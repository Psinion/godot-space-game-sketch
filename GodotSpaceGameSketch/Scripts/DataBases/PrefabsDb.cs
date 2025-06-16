using System.Collections.Generic;
using Godot;
using GodotSpaceGameSketch.Bodies.Enums;
using GodotSpaceGameSketch.Bodies.Samples;
using GodotSpaceGameSketch.Pooling.Enums;
using GodotSpaceGameSketch.Pooling.Samples;
using GodotSpaceGameSketch.Pooling.Samples.Base;
using GodotSpaceGameSketch.Core;

namespace GodotSpaceGameSketch.DataBases;

public class PrefabsDb
{
    public readonly Dictionary<SpaceShipType, SpaceShipSample> spaceShips = new();
    public readonly Dictionary<TurretType, TurretSample> weaponTurrets = new();
    public readonly Dictionary<FlyweightType, FlyweightSample> flyweightsSettings = new();

    public void Initialise()
    {
        InitializeSpaceShips();
        InitializeWeaponTurrets();
        InitializeFlyweights();
    }
    
    public SpaceShipSample GetSpaceShipSample(SpaceShipType type) => spaceShips[type];
    public TurretSample GetWeaponTurretSample(TurretType type) => weaponTurrets[type];
    public FlyweightSample GetFlyweightSample(FlyweightType type) => flyweightsSettings[type];
    
    private void InitializeSpaceShips()
    {
        spaceShips.Add(SpaceShipType.Dolphin, new SpaceShipSample()
        {
            type = SpaceShipType.Dolphin,
            sample = ResourceLoader.Load<PackedScene>("res://Prefabs/Vehicles/Dolphin.tscn"),
            rotationSpeed = 1,
            moveSpeedMax = 10f,
            stopDistance = 2f,
            acceleration = 3f,
            deceleration = 5f,
            health = 100,
            weaponPosition = WeaponPosition.Bottom,
            trackerScanInterval = 2f,
            trackerRadius = 1000,
        });
        
        spaceShips.Add(SpaceShipType.DolphinPlayer, new SpaceShipSample()
        {
            type = SpaceShipType.DolphinPlayer,
            sample = ResourceLoader.Load<PackedScene>("res://Prefabs/Vehicles/Dolphin.tscn"),
            rotationSpeed = 1,
            moveSpeedMax = 10f,
            stopDistance = 2f,
            acceleration = 3f,
            deceleration = 5f,
            health = 10000,
            weaponPosition = WeaponPosition.Bottom,
            trackerScanInterval = 2f,
            trackerRadius = 1000,
        });
    }
    
    private void InitializeWeaponTurrets()
    {
        weaponTurrets.Add(TurretType.Test, new TurretSample(
            WeaponType.Turret,
            TurretType.Test,
            ResourceLoader.Load<PackedScene>("res://Prefabs/Placements/Weapons/Turret.tscn"),
            100,
            2,
            2,
            40f,
            1f,
            25f,
            100,
            minPivotAngleVertical: 180,
            maxPivotAngleVertical: 0
            )
        );
        
        weaponTurrets.Add(TurretType.Test2, new TurretSample(
            WeaponType.Turret,
            TurretType.Test2,
            ResourceLoader.Load<PackedScene>("res://Prefabs/Placements/Weapons/Turret.tscn"),
            80,
            0.5f,
            3,
            40f,
            1f,
            25f,
            100,
            minPivotAngleVertical: 180,
            maxPivotAngleVertical: 0
            )
        );
    }
    
    private void InitializeFlyweights()
    {
        flyweightsSettings.Add(FlyweightType.Projectile, new ProjectileSample(
            FlyweightType.Projectile, 
            15, 
            ResourceLoader.Load<PackedScene>("res://Prefabs/Flyweigths/Projectiles/Projectile.tscn")));
        flyweightsSettings.Add(FlyweightType.ExplosionMedium, new ExplosionSample(
            FlyweightType.ExplosionMedium,
            5, 
            ResourceLoader.Load<PackedScene>("res://Prefabs/Flyweigths/Explosions/ExplosionMedium.tscn")));
    }
}