using Friflo.Engine.ECS;
using GodotSpaceGameSketch.Pooling.Flyweights.Base;

namespace GodotSpaceGameSketch.Pooling.Components;

public struct Flyweight : IComponent
{
    public IFlyweightView flyweightLink;

    public Flyweight(IFlyweightView flyweightLink)
    {
        this.flyweightLink = flyweightLink;
    }
}