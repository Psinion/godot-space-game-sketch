using Godot;
using Godot.Collections;
using GodotSpaceGameSketch.Core.Enums;

namespace GodotSpaceGameSketch.Core.Extensions;

public static class RaycastExtensions
{
    public static Array<Dictionary> MakeRaycastPoint(Vector2 position, CollisionLayers2D layers2D = CollisionLayers2D.None)
    {
        var query = new PhysicsPointQueryParameters2D
        {
            Position = position,
            CollideWithAreas = true,
            CollisionMask = (uint)layers2D,
        };
        return WorldManager.Instance.SpaceState2D.IntersectPoint(query);
    }
    
    public static Dictionary MakeRaycast(Vector3 raycastStart, Vector3 raycastEnd, CollisionLayers3D layers3D = CollisionLayers3D.CelestialBody)
    {
        var rayQuery = PhysicsRayQueryParameters3D.Create(raycastStart, raycastEnd, (uint) layers3D);
        //rayQuery.CollideWithBodies = true;
        return WorldManager.Instance.SpaceState3D.IntersectRay(rayQuery);
    }
}