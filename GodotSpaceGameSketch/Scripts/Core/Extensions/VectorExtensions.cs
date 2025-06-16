using System;
using Godot;

namespace GodotSpaceGameSketch.Core.Extensions;

public static class VectorExtensions
{
    public static Quaternion RotateTowards(this Vector3 current, Vector3 target, Vector3 up, float maxDegrees = 360)
    {
        var dot = current.Dot(target);

        if (dot > 0.9999f)
        {
            return Quaternion.Identity;
        }
        if (dot < -0.9999f)
        {
            return new Quaternion(up, 180f);
        }
        
        var cross = current.Cross(target).Normalized();

        var angle = Math.Clamp(Mathf.Acos(dot), 0, maxDegrees);
        return new Quaternion(cross, angle);
    }
    
    public static Quaternion RotateTowards(this Vector3 current, Vector3 target, float maxDegrees = 360)
    {
        return current.RotateTowards(target, Vector3.Up, maxDegrees);
    }
    
    // Gradually changes a vector towards a desired goal over time.
    public static Vector3 SmoothDamp(
        this Vector3 current,
        Vector3 target,
        ref Vector3 velocity,
        float smoothTime,
        float deltaTime)
    {
        // Based on Game Programming Gems 4 Chapter 1.10
        
        // Расчёт параметра затухания (Жёсткость пружины, которая тянет камеру к цели.)
        float omega = 2f / smoothTime;
        float x = omega * deltaTime;
        
        // Аппроксимация экспоненциального затухания через полином
        float exp = 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);
        Vector3 delta = current - target;
        
        // Временная корректировка скорости с учётом ускорения
        Vector3 temp = (velocity + omega * delta) * deltaTime;
        
        // Обновление скорости с затуханием
        velocity = (velocity - omega * temp) * exp;
        return target + (delta + temp) * exp;
    }
}