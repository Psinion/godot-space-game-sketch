using Godot;
using GodotSpaceGameSketch.Global.CameraSystem;
using GodotSpaceGameSketch.Shared.Interaction.Controllers;

namespace GodotSpaceGameSketch.Global.Interaction;

public partial class GlobalPlayerController : Controller
{
    private const string ScrollWheelUpInput = "wheel_up";
    private const string ScrollWheelDownInput = "wheel_down";
    private const string MouseLeftInput = "mouse_left";
    private const string MouseRightInput = "mouse_right";
    private const float ZoomWeight = 3f;
    
    private GlobalCameraEntity battleCameraEntity;
    private bool isDragging;

    public override void _EnterTree()
    {
        battleCameraEntity = GetNode<GlobalCameraEntity>("SpaceCamera");
    }
    
    public override void _Process(double delta)
    {
        battleCameraEntity.SmoothZoom(delta);
        
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
            battleCameraEntity.StartRotate();
        }   
        else if (Input.IsActionPressed(MouseRightInput) && battleCameraEntity.CheckCameraRotation())
        {
            isDragging = true;
            battleCameraEntity.RotateAround();
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
            battleCameraEntity.AdjustZoom(zoomDelta);
        }
    }

    public override void LeftClick(Vector2 mousePosition)
    {
        
    }

    public override void RightClick(Vector2 mousePosition)
    {

    }
}