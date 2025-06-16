using Godot;
using GodotSpaceGameSketch.Pooling.Enums;
using GodotSpaceGameSketch.Pooling.Flyweights.Base;

namespace GodotSpaceGameSketch.Pooling.Samples.Base;

public abstract class FlyweightSample
{
    protected FlyweightType type;
    public FlyweightType Type => type;
        
    public PackedScene sample;
    public float lifeTime;

    public abstract IFlyweightView Create();
        
    public virtual void OnGet(IFlyweightView flyweightView) => flyweightView.Show();
    public virtual void OnRelease(IFlyweightView flyweightView) => flyweightView.Hide();
    public virtual void OnDestroyPoolObject(IFlyweightView f) => f.Destroy();
}