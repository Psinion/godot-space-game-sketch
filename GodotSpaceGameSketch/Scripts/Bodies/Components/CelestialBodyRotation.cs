using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.Bodies.Components;

public struct CelestialBodyRotation : IComponent
{
    public Basis basis;

    public CelestialBodyRotation(Basis basis)
    {
        this.basis = basis;
    }
}