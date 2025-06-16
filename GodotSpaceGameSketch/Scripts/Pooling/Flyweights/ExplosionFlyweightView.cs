using Godot;
using GodotSpaceGameSketch.Pooling.Flyweights.Base;
using GodotSpaceGameSketch.Pooling.Samples.Base;

namespace GodotSpaceGameSketch.Pooling.Flyweights;

public partial class ExplosionFlyweightView : Node3D, IFlyweightView
{
    private GpuParticles3D explosionEffect;
    
    public Node3D Node => this;
    public FlyweightSample Sample { get; set; }

    public override void _EnterTree()
    {
        base._EnterTree();
        explosionEffect = GetNode("Explosion Effect") as GpuParticles3D;
    }

    public void OnTakeFromPool()
    {
        explosionEffect.Restart();
    }

    public void Destroy()
    {
        QueueFree();
    }
}