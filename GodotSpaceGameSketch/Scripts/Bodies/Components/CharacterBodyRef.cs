using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.Bodies.Components;

public struct CharacterBodyRef : IComponent
{
    public CharacterBody3D value;

    public CharacterBodyRef(CharacterBody3D value)
    {
        this.value = value;
    }
}