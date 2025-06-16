using System;
using Godot;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Core.Extensions;

namespace GodotSpaceGameSketch.Shared.Managers;

public partial class SceneManager : SingletonNode<SceneManager>
{
    [Export]
    private Node sceneRoot;
    
    public enum GameSceneCode
    {
        MainMenu,
        GlobalMap,
        Battle
    }
    
    private GameSceneCode currentSceneCode;

    public override void _Ready()
    {
        LoadScene(GameSceneCode.MainMenu);
    }

    public void LoadScene(GameSceneCode sceneCode)
    {
        var scenePath = GetScenePath(sceneCode);
        
        var packedScene = ResourceLoader.Load<PackedScene>(scenePath);
        var scene = packedScene.Instantiate();

        if (sceneRoot.TryGetChild(0, out var currentScene))
        {
            currentScene.QueueFree();
        }
        
        sceneRoot.AddChild(scene);

        currentSceneCode = sceneCode;
    }
    
    private string GetScenePath(GameSceneCode state)
    {
        return state switch
        {
            GameSceneCode.MainMenu => "res://Scenes/MainMenu.tscn",
            GameSceneCode.GlobalMap => "res://Scenes/GlobalMap.tscn",
            GameSceneCode.Battle => "res://Scenes/LocalBattle.tscn",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}