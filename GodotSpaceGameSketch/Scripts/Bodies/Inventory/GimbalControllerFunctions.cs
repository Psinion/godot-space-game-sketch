using Godot;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Core.Utilities;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Core.Extensions;

namespace GodotSpaceGameSketch.Bodies.Inventory;

public static class GimbalControllerFunctions
{
    private const float IdleRotationEpsilon = 1.0E-2f;
    
    // Повернуть объект лицом к заданным координатам за deltaTime.
    public static void RotateToPosition(
        Vector3 targetPosition, 
        float deltaTime, 
        Node3D turretNode, 
        ref GimbalController gimbalController, 
        ref WeaponTurret weaponTurret)
    {
        // Horizontal Rotation
        var turretTransformUp = turretNode.Transform.Up();
        
        var vectorToTarget = (targetPosition - turretNode.GlobalPosition).Normalized();

        var projectedVectorToBase = PsiMath.ProjectOnPlane(vectorToTarget, turretTransformUp);
        var horizontalPivotRotation = gimbalController.horizontalPivot.Rotation.Y;
        
        var horizontalTargetRotation = turretNode.GlobalTransform.Forward().SignedAngleTo(projectedVectorToBase, turretTransformUp);
        if (gimbalController.hasConstraintsHorizontal)
        {
            horizontalTargetRotation = Mathf.Clamp(horizontalTargetRotation, -gimbalController.minPivotAngleHorizontal, gimbalController.maxPivotAngleHorizontal);
            //DrawHorizontalLimits(ref gimbalController, turretTransform.GlobalPosition, deltaTime);
        }

        RotateByHorizontal(horizontalPivotRotation, horizontalTargetRotation, deltaTime, ref gimbalController, ref weaponTurret);

        var localTargetPos = turretNode.ToLocal(targetPosition - gimbalController.verticalPivot.Position);
        var flattenedVecForBarrels = PsiMath.ProjectOnPlane(localTargetPos, Vector3.Up);

        var verticalPivotRotation = gimbalController.verticalPivot.Rotation.X;
        var verticalTargetRotation = flattenedVecForBarrels.AngleTo(localTargetPos);
        verticalTargetRotation *= Mathf.Sign(localTargetPos.Y);

        if (gimbalController.hasConstraintsVertical)
        {
            verticalTargetRotation = Mathf.Clamp(verticalTargetRotation, -gimbalController.minPivotAngleVertical, gimbalController.maxPivotAngleVertical);
            //DrawVerticalLimits(ref gimbalController, deltaTime);
        }

        RotateByVertical(verticalPivotRotation, verticalTargetRotation, deltaTime, ref gimbalController, ref weaponTurret);
    }
        
    public static void RotateToIdle(float deltaTime, ref GimbalController gimbalController, ref WeaponTurret weaponTurret)
    {
        var idleTarget = Quaternion.Identity.GetEuler();
            
        var horizontalPivotRotation = gimbalController.horizontalPivot.Rotation.Y;
        var horizontalTargetRotation = idleTarget.Y;
        RotateByHorizontal(horizontalPivotRotation, horizontalTargetRotation, deltaTime, ref gimbalController, ref weaponTurret);

        gimbalController.isHorizontalPivotRest = Mathf.Abs(horizontalPivotRotation - horizontalTargetRotation) < IdleRotationEpsilon;
            
        var verticalPivotRotation = gimbalController.verticalPivot.Rotation.X;
        var verticalTargetRotation = idleTarget.X;
        RotateByVertical(verticalPivotRotation, verticalTargetRotation, deltaTime, ref gimbalController, ref weaponTurret);

        gimbalController.isVerticalPivotRest = Mathf.Abs(verticalPivotRotation - verticalTargetRotation) < IdleRotationEpsilon;
    }
        
    private static void RotateByHorizontal(float currentAngle, float targetAngle, float deltaTime, ref GimbalController gimbalController, ref WeaponTurret weaponTurret)
    {
        float torqueCorrection = gimbalController.axisPidControllerY.ProcessByRad(
            currentAngle,
            targetAngle, 
            deltaTime);
            
        torqueCorrection = Mathf.Clamp(torqueCorrection, -weaponTurret.rotationSpeed, weaponTurret.rotationSpeed);
        
        gimbalController.horizontalPivot.Rotate(Vector3.Up, torqueCorrection);
    }

    private static void RotateByVertical(float currentAngle, float targetAngle, float deltaTime, ref GimbalController gimbalController, ref WeaponTurret weaponTurret)
    {
        float torqueCorrection = gimbalController.axisPidControllerX.ProcessByRad(
            currentAngle,
            targetAngle, 
            deltaTime);
            
        torqueCorrection = Mathf.Clamp(torqueCorrection, -weaponTurret.rotationSpeed, weaponTurret.rotationSpeed);
        
        gimbalController.verticalPivot.Rotate(Vector3.Right, torqueCorrection);
    }
    
    private static void DrawHorizontalLimits(ref GimbalController gimbal, Vector3 position, float deltaTime)
    {
        var horizontalPivot = gimbal.horizontalPivot;
        var basis = horizontalPivot.GlobalTransform.Basis;
        
        Vector3 localY = basis.Up();
        Vector3 originalForward = basis.Forward().Rotated(localY, -horizontalPivot.Rotation.Y);
    
        // Минимальный угол
        Vector3 minDir = originalForward.Rotated(localY, -gimbal.minPivotAngleHorizontal).Normalized();
        PsiDebug.DrawRay(position, minDir * 10f, Colors.Red, deltaTime);
    
        // Максимальный угол
        Vector3 maxDir = originalForward.Rotated(localY, gimbal.maxPivotAngleHorizontal).Normalized();
        PsiDebug.DrawRay(position, maxDir * 10f, Colors.Red, deltaTime);
    }

    private static void DrawVerticalLimits(ref GimbalController gimbal, float deltaTime)
    {
        var verticalPivot = gimbal.verticalPivot;
        var basis = verticalPivot.GlobalTransform.Basis;
        Vector3 position = verticalPivot.GlobalPosition;
        
        Vector3 localX = basis.Right();
        Vector3 originalForward = -basis.Forward().Rotated(localX, -verticalPivot.Rotation.X);
    
        // Минимальный угол (вниз)
        Vector3 minDir = originalForward.Rotated(localX, -gimbal.minPivotAngleVertical).Normalized();
        PsiDebug.DrawRay(position, minDir * 10f, Colors.Blue, deltaTime);
    
        // Максимальный угол (вверх)
        Vector3 maxDir = originalForward.Rotated(localX, gimbal.maxPivotAngleVertical).Normalized();
        PsiDebug.DrawRay(position, maxDir * 10f, Colors.Blue, deltaTime);
    }
}