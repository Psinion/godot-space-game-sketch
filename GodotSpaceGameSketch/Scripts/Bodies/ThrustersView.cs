using Godot;
using GodotSpaceGameSketch.Core.Extensions;

namespace GodotSpaceGameSketch.Bodies;

public partial class ThrustersView : Node3D
{
    [Export]
    private GpuParticles3D[] thrustersList;

    public override void _EnterTree()
    {
        base._EnterTree();
        thrustersList = this.GetNodeInChildren<GpuParticles3D>();
        
        foreach (var thruster in thrustersList)
        {
            var material = thruster.ProcessMaterial;
            if (material != null)
            {
                var newMaterial = material.Duplicate() as ParticleProcessMaterial;
                thruster.ProcessMaterial = newMaterial;
            }
        }
    }

    public void UpdateEffects(float intensity)
    {
        foreach (var thruster in thrustersList)
        {
            var material = (ParticleProcessMaterial)thruster.ProcessMaterial;
            material.InitialVelocityMax = Mathf.Lerp(0.8f, 8f, intensity);
            
            thruster.ProcessMaterial = material;
        }
    }
}