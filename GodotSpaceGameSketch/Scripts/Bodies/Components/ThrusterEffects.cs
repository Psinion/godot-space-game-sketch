using Friflo.Engine.ECS;

namespace GodotSpaceGameSketch.Bodies.Components;

public struct ThrusterEffects : IComponent {
    public float intensity;
    
    public ThrusterEffects()
    {
        intensity = 0;
    }
}