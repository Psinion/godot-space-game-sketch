using Godot;
using GodotSpaceGameSketch.Pooling.Enums;
using GodotSpaceGameSketch.Pooling.Flyweights;
using GodotSpaceGameSketch.Pooling.Flyweights.Base;
using GodotSpaceGameSketch.Pooling.Samples.Base;

namespace GodotSpaceGameSketch.Pooling.Samples;

public class ProjectileSample : FlyweightSample
{
    public ProjectileSample(FlyweightType type, float lifeTime, PackedScene sample)
    {
        this.type = type;
        this.lifeTime = lifeTime;
        this.sample = sample;
    }

    public override IFlyweightView Create()
    {
        var flyweight = sample.Instantiate<ProjectileFlyweightView>();
        flyweight.Sample = this;
        flyweight.Hide();
        return flyweight;
    }
}