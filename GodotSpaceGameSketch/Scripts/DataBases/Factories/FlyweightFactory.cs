using System.Collections.Generic;
using Godot;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Pooling;
using GodotSpaceGameSketch.Pooling.Enums;
using GodotSpaceGameSketch.Pooling.Flyweights.Base;
using GodotSpaceGameSketch.Pooling.Samples.Base;

namespace GodotSpaceGameSketch.DataBases.Factories;

public partial class FlyweightFactory : Singleton<FlyweightFactory>
{
    private Node3D spawnNode;
    
    private PrefabsDb prefabsDb;
    
    private readonly Dictionary<FlyweightType, GenericObjectPool<IFlyweightView>> pools = new();

    public FlyweightFactory(PrefabsDb prefabsDb, Node3D spawnNode)
    {
        this.prefabsDb = prefabsDb;
        this.spawnNode = spawnNode;
    }

    public static IFlyweightView Spawn(FlyweightSample sample, Vector3 position = default,
        Quaternion rotation = default)
    {
        return instance.GetPoolFor(sample)?.GetItem(position, rotation);
    }
    
    public static IFlyweightView Spawn(FlyweightType type, Vector3 position, Quaternion rotation)
    {
        var sample = WorldManager.Instance.PrefabsDb.GetFlyweightSample(type);
        return instance.GetPoolFor(sample)?.GetItem(position, rotation);
    }
    
    public static IFlyweightView Spawn(FlyweightType type, Vector3 position = default)
    {
        return Spawn(type, position, Quaternion.Identity);
    }
        
    public static void ReturnToPool(IFlyweightView f) => instance.GetPoolFor(f.Sample)?.Recycle(f);
        
    public IFlyweightView Create(FlyweightSample sample, Vector3 position = default, Quaternion rotation = default)
    {
        return GetPoolFor(sample).GetItem(position, rotation);
    }
    
    public IFlyweightView Create(FlyweightType type, Vector3 position = default, Quaternion rotation = default)
    {
        var sample = prefabsDb.GetFlyweightSample(type);
        return GetPoolFor(sample).GetItem(position, rotation);
    }
        
    private GenericObjectPool<IFlyweightView> GetPoolFor(FlyweightSample sample) {
        GenericObjectPool<IFlyweightView> pool;

        if (pools.TryGetValue(sample.Type, out pool))
        {
            return pool;
        }

        pool = new GenericObjectPool<IFlyweightView>(
            spawnNode,
            sample.Create,
            sample.OnGet,
            sample.OnRelease,
            sample.OnDestroyPoolObject
        );
        pools.Add(sample.Type, pool);
        return pool;
    }
}