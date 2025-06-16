using System;

namespace GodotSpaceGameSketch.Core.Enums;

[Flags]
public enum CollisionLayers3D
{
    None = 0,
    CelestialBody = 1,
    Projectile = 512,
}