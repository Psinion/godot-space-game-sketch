using Godot;
using GodotSpaceGameSketch.Pooling.Samples.Base;
using GodotSpaceGameSketch.Pooling.Samples;

namespace GodotSpaceGameSketch.Pooling.Flyweights.Base;

public interface IFlyweightView
{
    Node3D Node { get; }
    FlyweightSample Sample { get; set; }
    
    void OnTakeFromPool();

    void Show();
    void Hide();
    
    void Destroy();
}