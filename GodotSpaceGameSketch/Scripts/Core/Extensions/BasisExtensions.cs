using Godot;

namespace GodotSpaceGameSketch.Core.Extensions;

public static class BasisExtensions
{
    public static Vector3 Right(this Basis basis)
    {
        return basis.X.Normalized();
    }

    public static Vector3 Left(this Basis basis)
    {
        return -basis.X.Normalized();
    }

    public static Vector3 Up(this Basis basis)
    {
        return basis.Y.Normalized();
    }

    public static Vector3 Down(this Basis basis)
    {
        return -basis.Y.Normalized();
    }

    public static Vector3 Forward(this Basis basis)
    {
        return -basis.Z.Normalized();
    }

    public static Vector3 Back(this Basis basis)
    {
        return basis.Z.Normalized();
    }
}