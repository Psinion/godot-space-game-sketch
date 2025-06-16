using Friflo.Engine.ECS;

namespace GodotSpaceGameSketch.Pooling.Components;

public struct FlyweightLifeTime : IComponent
{
    public float lifeTime;
    public float lifeTimeCurrent;

    public FlyweightLifeTime(float lifeTime)
    {
        this.lifeTime = lifeTime;
        this.lifeTimeCurrent = lifeTime;
    }
}