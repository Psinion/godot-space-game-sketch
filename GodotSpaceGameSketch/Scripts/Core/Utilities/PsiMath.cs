using Godot;

namespace GodotSpaceGameSketch.Core.Utilities;

public static class PsiMath
{
    public const float Deg2Rad = 0.017453292f;
    public const float Rad2Deg = 57.29578f;
    
    public static float MoveTowards(float current, float target, float maxDelta) => 
        Mathf.Abs(target - current) <= (double) maxDelta ? target : current + Mathf.Sign(target - current) * maxDelta;
    
    public static Vector3 ProjectOnPlane(Vector3 vector, Vector3 planeNormal)
    {
        float num1 = planeNormal.Dot(planeNormal);
        if ((double) num1 < Mathf.Epsilon)
        {
            return vector;
        }
        float num2 = vector.Dot(planeNormal);
        return new Vector3(vector.X - planeNormal.X * num2 / num1, vector.Y - planeNormal.Y * num2 / num1, vector.Z - planeNormal.Z * num2 / num1);
    }

    /// <summary>
    /// Returns a random floating point value between -1.0 and 1.0 (inclusive).
    /// Get from "Artificial Intelligence for games", 3.2.2
    /// </summary>
    /// <returns>A random float number.</returns>
    public static float RandomBinomial()
    {
        return GD.Randf() - GD.Randf();
    }
}