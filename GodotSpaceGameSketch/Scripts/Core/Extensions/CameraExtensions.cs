using Godot;
using Godot.Collections;

namespace GodotSpaceGameSketch.Core.Extensions;

public static class CameraExtensions
{
    public static Dictionary ScreenPointToRay(this Camera3D camera, Vector2 mousePosition, float rayDistance)
    {
        if (WorldManager.Instance == null || WorldManager.Instance.SpaceState3D == null)
        {
            GD.Print("Space 3D is not set yet.");
            return new Dictionary();
        }
        
        var raycastStart = camera.ProjectRayOrigin(mousePosition);
        var raycastEnd = raycastStart + camera.ProjectRayNormal(mousePosition) * rayDistance;
        return RaycastExtensions.MakeRaycast(raycastStart, raycastEnd);
    }
    
    public static Vector3 ScreenPointToRayWorldPosition(this Camera3D camera, Vector2 mousePosition)
    {
        if (WorldManager.Instance == null || WorldManager.Instance.SpaceState3D == null)
        {
            GD.Print("Space 3D is not set yet.");
            return Vector3.Zero;
        }
        
        var origin = camera.ProjectRayOrigin(mousePosition);
        var direction = camera.ProjectRayNormal(mousePosition);
        
        if (direction.Y == 0)
        {
            return Vector3.Zero;
        }

        var distance = -origin.Y / direction.Y;
        return origin + direction * distance;
    }

    public static Vector3 MouseToWorldPosition(this Camera3D camera, Vector2 mousePosition, float rayDistance)
    {
        return camera.ProjectPosition(mousePosition, rayDistance);
    }
}