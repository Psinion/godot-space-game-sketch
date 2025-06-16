using Godot;

namespace GodotSpaceGameSketch.Core.Extensions;

public static class TransformExtensions
{
    public static Vector3 Right(this Transform3D transform)
    {
        return transform.Basis.X.Normalized();
    }

    public static Vector3 Left(this Transform3D transform)
    {
        return -transform.Basis.X.Normalized();
    }

    public static Vector3 Up(this Transform3D transform)
    {
        return transform.Basis.Y.Normalized();
    }

    public static Vector3 Down(this Transform3D transform)
    {
        return -transform.Basis.Y.Normalized();
    }

    public static Vector3 Forward(this Transform3D transform)
    {
        return -transform.Basis.Z.Normalized();
    }

    public static Vector3 Back(this Transform3D transform)
    {
        return transform.Basis.Z.Normalized();
    }

    public static Vector2 Right(this Transform2D transform)
    {
        return transform.X.Normalized();
    }

    public static Vector2 Left(this Transform2D transform)
    {
        return -transform.X.Normalized();
    }

    public static Vector2 Up(this Transform2D transform)
    {
        return transform.Y.Normalized();
    }

    public static Vector2 Down(this Transform2D transform)
    {
        return -transform.Y.Normalized();
    }
}