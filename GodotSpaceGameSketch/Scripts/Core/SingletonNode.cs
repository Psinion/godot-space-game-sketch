using Godot;

namespace GodotSpaceGameSketch.Core;

public partial class SingletonNode<T> : Node where T : Node
{
    protected static T instance;
    public static T Instance => instance;

    public override void _EnterTree()
    {
        base._EnterTree();
        instance = this as T;
    }
}