using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.AI.Components;

public struct CelestialBodyAI : IComponent
{
    public float targetRefreshTimeMax;
    public float targetRefreshTimeCurrent;
    public int attackRadius;

    public CelestialBodyAI(float targetRefreshTimeMax, int attackRadius)
    {
        this.targetRefreshTimeMax = targetRefreshTimeMax;
        this.targetRefreshTimeCurrent = GD.Randf() * targetRefreshTimeMax;
        this.attackRadius = attackRadius;
    }
}