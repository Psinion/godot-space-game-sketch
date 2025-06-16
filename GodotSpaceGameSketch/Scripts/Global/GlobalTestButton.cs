using Godot;
using GodotSpaceGameSketch.Shared.Managers;

namespace GodotSpaceGameSketch.Global;

public partial class GlobalTestButton : Button
{
    public override void _Ready()
    {
        Pressed += OnButtonPressed;
    }

    private async void OnButtonPressed()
    {
        SceneManager.Instance.LoadScene(SceneManager.GameSceneCode.GlobalMap);
    }
}