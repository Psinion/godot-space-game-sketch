using System.Collections.Generic;
using Friflo.Engine.ECS;

namespace GodotSpaceGameSketch.Tracking;

public struct TargetInfo
{
    public Entity entity;
    public int owner;
    public float distance;
}
    
public class ShipToTargetsStorage
{
    private readonly Dictionary<int, List<TargetInfo>> shipToTargets = new ();

    public void Register(int entityId)
    {
        shipToTargets[entityId] = new List<TargetInfo>();
    }
        
    public void Unregister(int entityId)
    {
        shipToTargets.Remove(entityId);
    }
        
    public List<TargetInfo> Get(int entityId)
    {
        return shipToTargets[entityId];
    }
}