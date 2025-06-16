using Godot;
using GodotSpaceGameSketch.Pooling.Enums;
using GodotSpaceGameSketch.Pooling.Flyweights;
using GodotSpaceGameSketch.Pooling.Flyweights.Base;
using GodotSpaceGameSketch.Pooling.Samples.Base;

namespace GodotSpaceGameSketch.Pooling.Samples;

public class ExplosionSample : FlyweightSample
{
    public ExplosionSample(FlyweightType type, float lifeTime, PackedScene sample)
    {
        this.type = type;
        this.lifeTime = lifeTime;
        this.sample = sample;
    }

    public override IFlyweightView Create()
    {
        var flyweight = sample.Instantiate<ExplosionFlyweightView>();
        flyweight.Sample = this;
        flyweight.Hide();
        return flyweight;
    }
}