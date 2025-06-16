using Friflo.Engine.ECS;
using GodotSpaceGameSketch.Bodies.Components;

namespace GodotSpaceGameSketch.Core.Extensions;

public static class FrifloECSExtensions
{
    public static void DeleteEntityWithChildren(this CommandBuffer commandBuffer, Entity entity)
    {
        foreach (var child in entity.ChildEntities)
        {
            commandBuffer.DeleteEntityWithChildren(child);
        }
        
        commandBuffer.DeleteEntity(entity.Id);
    }
    
    public static void AddDeathTagWithChildren(this CommandBuffer commandBuffer, Entity entity)
    {
        foreach (var child in entity.ChildEntities)
        {
            commandBuffer.AddDeathTagWithChildren(child);
        }
        
        commandBuffer.AddTag<DeathEvent>(entity.Id);
    }
}