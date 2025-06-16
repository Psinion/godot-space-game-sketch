using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.Bodies.Components;

public struct CelestialBodyViewRef : IComponent
{
    public Battle.Bodies.CelestialBodyView value;

    public CelestialBodyViewRef(Battle.Bodies.CelestialBodyView value)
    {
        this.value = value;
    }
}