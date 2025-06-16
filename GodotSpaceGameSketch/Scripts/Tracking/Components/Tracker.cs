using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.Tracking.Components;

public struct Tracker : IComponent
{
    public float scanInterval;
    public float scanIntervalEstimated;
    public int radius;

    public Tracker(float scanInterval, int radius)
    {
        this.scanInterval = scanInterval;
        this.scanIntervalEstimated = scanInterval - GD.Randf() * scanInterval;
        this.radius = radius;
    }
}