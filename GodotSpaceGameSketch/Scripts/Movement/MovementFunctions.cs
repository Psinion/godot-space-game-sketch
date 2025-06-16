using Friflo.Engine.ECS;
using Godot;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Core.Utilities;
using GodotSpaceGameSketch.Movement.Components;
using GodotSpaceGameSketch.Core.Extensions;

namespace GodotSpaceGameSketch.Movement;

public static class MovementFunctions
{
    public static void EngageMovement(ref Entity entity, ref Vector3 entityPosition, ref Basis entityBasis, ref Vector3 targetPosition)
    {
        var shipPosition = entityPosition;
        var direction = (targetPosition - shipPosition).Normalized();
        var forwardVector = entityBasis.Forward();
        float cosAngle = direction.Dot(forwardVector);
        float angle = Mathf.Acos(cosAngle) * PsiMath.Rad2Deg;
        
        if (angle >= 60f)
        {
            entity.AddTag<MovementPreparingToMoveTag>();
        }
        else
        {
            entity.AddTag<MovementMoveToTargetDestinationTag>();
        }
    }
    
    public static void Rotate(float deltaTime, ref CelestialBodyRotation rotation, ref VehicleMovable vehicleMovable, ref Vector3 targetDirection)
    { 
        var targetRotation = Basis.LookingAt(targetDirection, Vector3.Up);

        var modelRotationRad = rotation.basis.GetEuler();
        var targetRotationQuaternionRad = targetRotation.GetEuler();

        float xTorqueCorrection = vehicleMovable.axisPidControllerX.ProcessByRad(
            modelRotationRad.X,
            targetRotationQuaternionRad.X,
            deltaTime);

        float yTorqueCorrection = vehicleMovable.axisPidControllerY.ProcessByRad(
            modelRotationRad.Y,
            targetRotationQuaternionRad.Y,
            deltaTime);

        float zTorqueCorrection = vehicleMovable.axisPidControllerZ.ProcessByRad(
            modelRotationRad.Z,
            targetRotationQuaternionRad.Z,
            deltaTime);

        var rotationThrust = vehicleMovable.rotationSpeed;
        xTorqueCorrection = Mathf.Clamp(xTorqueCorrection, -rotationThrust, rotationThrust);
        yTorqueCorrection = Mathf.Clamp(yTorqueCorrection, -rotationThrust, rotationThrust);
        zTorqueCorrection = Mathf.Clamp(zTorqueCorrection, -rotationThrust, rotationThrust);

        var newBasis = Basis.Identity
            .Rotated(Vector3.Up, yTorqueCorrection)
            .Rotated(Vector3.Right, xTorqueCorrection)
            .Rotated(Vector3.Back, zTorqueCorrection);
        
        rotation.basis *= newBasis;
    }
}