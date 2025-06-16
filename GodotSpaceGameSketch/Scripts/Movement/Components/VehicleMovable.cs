using Friflo.Engine.ECS;
using Godot;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Core.Utilities;

namespace GodotSpaceGameSketch.Movement.Components;

public struct VehicleMovable : IComponent
{
    public PID axisPidControllerX;
    public PID axisPidControllerY;
    public PID axisPidControllerZ;
    public float rotationSpeed;
    public float moveSpeedMax;
    public float stopDistance;
    public float accelerationMax;
    public float decelerationMax;
    public float currentAcceleration;
    public Vector3 velocity;

    public VehicleMovable(float rotationSpeed, float moveSpeedMax, float stopDistance, float accelerationMax, float decelerationMax)
    {
        var pid = new PID(0.3f, 0.15f, 0.03f * GlobalConstants.GodotCoordModifier);
        axisPidControllerX = pid;
        axisPidControllerY = pid;
        axisPidControllerZ = pid;
        this.rotationSpeed = rotationSpeed * GlobalConstants.GodotCoordModifier;
        this.moveSpeedMax = moveSpeedMax;
        this.stopDistance = stopDistance;
        this.accelerationMax = accelerationMax;
        this.decelerationMax = decelerationMax;

        currentAcceleration = 0;
        velocity = Vector3.Zero;
    }
}