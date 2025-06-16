using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.AI.Components;

public struct CelestialBodyMoveSelector : IComponent
{
    public float targetRefreshTimeMax;
    public float targetRefreshTimeCurrent;
    
    public float randomSpreadRefreshTimeMax;
    public float randomSpreadRefreshTimeCurrent;
    public Vector3 randomSpread = Vector3.Zero;

    public CelestialBodyMoveSelector(float time)
    {
        targetRefreshTimeMax = time;
        targetRefreshTimeCurrent = GD.Randf() * targetRefreshTimeMax;

        randomSpreadRefreshTimeMax = 15f;
        randomSpreadRefreshTimeCurrent = GD.Randf() * randomSpreadRefreshTimeMax;
    }
}