using Godot;
using GodotSpaceGameSketch.Pooling.Flyweights.Base;
using GodotSpaceGameSketch.Pooling.Samples.Base;

namespace GodotSpaceGameSketch.Pooling.Flyweights;

public partial class ProjectileFlyweightView : CharacterBody3D, IFlyweightView
{
    public Node3D Node => this;
    public FlyweightSample Sample { get; set; }

    public void OnTakeFromPool()
    {
    }

    public void Destroy()
    {
        QueueFree();
    }
}