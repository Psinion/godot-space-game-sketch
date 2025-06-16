using Friflo.Engine.ECS;
using Godot;
using GodotSpaceGameSketch.Battle.Bodies;
using GodotSpaceGameSketch.Battle.Bodies.CameraSystem;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Bodies.Inventory.Modules.Weapons.Components;
using GodotSpaceGameSketch.Core.Enums;
using GodotSpaceGameSketch.Core.Extensions;
using GodotSpaceGameSketch.Movement;
using GodotSpaceGameSketch.Movement.Components;
using GodotSpaceGameSketch.Shared.Interaction.Controllers;
using GodotSpaceGameSketch.Tracking;

namespace GodotSpaceGameSketch.Battle.Interaction.Controllers;

public partial class BattlePlayerController : Controller
{
    private const string ScrollWheelUpInput = "wheel_up";
    private const string ScrollWheelDownInput = "wheel_down";
    private const string MouseLeftInput = "mouse_left";
    private const string MouseRightInput = "mouse_right";
    private const float ZoomWeight = 3f;
    
    private BattleCameraEntity cameraEntity;
    private bool isDragging;
    
    public CelestialBodyView controlledShip;

    [Export]
    public SpaceCursor cursor;
    
    public override void _EnterTree()
    {
        cameraEntity = GetNode<BattleCameraEntity>("SpaceCamera");
    }
    
    public override void _Process(double delta)
    {
        cameraEntity.SmoothZoom(delta);
        
        if (Input.IsActionPressed(MouseLeftInput))
        {
            var mousePosition = GetViewport().GetMousePosition();
            LeftClick(mousePosition);
        }
        else if (Input.IsActionJustReleased(MouseRightInput))
        {
            if (!isDragging)
            {
                var mousePosition = GetViewport().GetMousePosition();
                RightClick(mousePosition);
            }
            else
            {
                isDragging = false;
            }
        }
        else if (Input.IsActionJustPressed(MouseRightInput))
        {
            cameraEntity.StartRotate();
        }   
        else if (Input.IsActionPressed(MouseRightInput) && cameraEntity.CheckCameraRotation())
        {
            isDragging = true;
            cameraEntity.RotateAround();
        }
        
        if (controlledShip?.GlobalPosition.DistanceTo(cursor.GlobalPosition) < 2f)
        {
            cursor.Hide();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        
    }

    public override void _Input(InputEvent @event)
    {
        float zoomDelta = 0;
        
        if (@event.IsActionReleased(ScrollWheelUpInput))
        {
            zoomDelta = ZoomWeight;
        }
        else if (@event.IsActionReleased(ScrollWheelDownInput))
        {
            zoomDelta = -ZoomWeight;
        }

        if (zoomDelta != 0f)
        {
            cameraEntity.AdjustZoom(zoomDelta);
        }
    }

    public override void LeftClick(Vector2 mousePosition)
    {
        
    }

    public override void RightClick(Vector2 mousePosition)
    {
        var results = RaycastExtensions.MakeRaycastPoint(mousePosition, CollisionLayers2D.TargetMarkers);
        foreach (var result in results)
        {
            if (result["collider"].As<Node>() is Area2D area && 
                area.GetParent() is TargetMarkerView markerView)
            {
                StartMoveToTarget(markerView.Trackable);
                return;
            }
        }
        
        StartMoveToPosition(mousePosition);
    }
    
    private void StartMoveToPosition(Vector2 mousePosition)
    {
        var targetPosition = cameraEntity.Camera.ScreenPointToRayWorldPosition(mousePosition);

        targetPosition.Y = GD.RandRange(-1, 1);
        
        var playerEntity = controlledShip.Entity;
        var playerPosition = controlledShip.GlobalPosition;
        var playerBasis = controlledShip.GlobalBasis;
        
        playerEntity.AddComponent(new CommandMove(targetPosition));
        playerEntity.RemoveComponent<CommandTarget>();
        cursor.Move(targetPosition);
        
        MovementFunctions.EngageMovement(ref playerEntity, ref playerPosition, ref playerBasis, ref targetPosition);
    }
    
    private void StartMoveToTarget(Entity target)
    {
        var playerEntity = controlledShip.Entity;
        var playerPosition = controlledShip.GlobalPosition;
        var playerBasis = controlledShip.GlobalBasis;
        
        playerEntity.AddComponent(new CommandTarget(target));
        foreach (var child in playerEntity.ChildEntities)
        {
            if (child.HasComponent<WeaponModule>())
            {
                ref var weaponModule = ref child.GetComponent<WeaponModule>();
                weaponModule.target = target;
            }
        }
        var targetPosition = target.GetComponent<CelestialBodyPosition>();
        
        MovementFunctions.EngageMovement(ref playerEntity, ref playerPosition, ref playerBasis, ref targetPosition.value);
    }
}