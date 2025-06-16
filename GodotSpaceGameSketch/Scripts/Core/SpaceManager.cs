using System.Collections.Generic;
using Friflo.Engine.ECS;
using Godot;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Tracking;

namespace GodotSpaceGameSketch.Core;

public partial class SpaceManager : SingletonNode3D<SpaceManager>
{
    private EntityStore entityStore;

    public override void _EnterTree()
    {
        base._EnterTree();
        entityStore = WorldManager.Instance.World;
    }
        
    public void GetEntitiesInRadiusWithDistance(int entityId, Vector3 entityPosition, float radius, ref List<TargetInfo> targetsList)
    {
        var cachedList = targetsList;
            
        var entities = entityStore
            .Query<CelestialBodyPosition, Owner>()
            .WithoutAnyTags(Tags.Get<DeathEvent>())
            .AnyTags(Tags.Get<CelestialBodyTag>());

        entities.ForEachEntity((ref CelestialBodyPosition position, ref Owner owner, Entity entity) =>
        {
            if (entity.Id == entityId)
            {
                return;
            }
            
            float distance = entityPosition.DistanceTo(position.value);

            AddTargetByDistancePriority(entity, distance, owner.id, ref cachedList);
        });
            
        targetsList = cachedList;
    }
        
    private void AddTargetByDistancePriority(Entity entity, float distance, int ownerId, ref List<TargetInfo> targetsList)
    {
        int index = 0;
        while (index < targetsList.Count
               && targetsList[index].distance < distance)
        {
            index++;
        } 
            
        targetsList.Insert(index, new TargetInfo()
        {
            entity = entity,
            owner = ownerId,
            distance = distance
        });
    }
}