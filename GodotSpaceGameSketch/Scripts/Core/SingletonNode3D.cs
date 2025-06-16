using Godot;

namespace GodotSpaceGameSketch.Core;

public partial class SingletonNode3D<T> : Node3D where T : Node3D
{
    protected static T instance;
    public static T Instance => instance;

    public override void _EnterTree()
    {
        base._EnterTree();
        instance = this as T;
    }
}