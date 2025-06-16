using Godot;
using GodotSpaceGameSketch.Bodies.Enums;

namespace GodotSpaceGameSketch.Bodies.Samples;

public class SpaceShipSample
{
    public SpaceShipType type;
    public PackedScene sample;
    
    public float rotationSpeed;
    public float moveSpeedMax;
    public float stopDistance;
    public float acceleration;
    public float deceleration;

    public int health;

    public WeaponPosition weaponPosition;

    public float trackerScanInterval;
    public int trackerRadius;
}