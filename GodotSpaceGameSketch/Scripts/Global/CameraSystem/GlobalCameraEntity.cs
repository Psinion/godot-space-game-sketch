using Godot;
using GodotSpaceGameSketch.Core.Utilities;

namespace GodotSpaceGameSketch.Global.CameraSystem;

public partial class GlobalCameraEntity : Node3D
{
	private Node3D swivel;
	private Node3D stick;
	
	private float rotationSpeedX = 0.01f;
	private float rotationSpeedY = 0.01f;

	private float stickMinZoom = 2f;
	private float stickMaxZoom = 17f;
	private float zoomSpeed = 8f;
	private float currentZoom;
	private float targetZoom;
	
	private Vector3 previousPosition;

	private Camera3D camera;
	public Camera3D Camera => camera;
	
	public override void _Ready()
	{
		swivel = GetNode<Node3D>("Swivel");
		stick = GetNode<Node3D>("Swivel/Stick");
		camera = GetNode<Camera3D>("Swivel/Stick/Camera3D");
		currentZoom = targetZoom = stick.Position.Y;
		AdjustZoom(0);
	}

	public Vector3 GetMousePosition()
	{
		var position = GetViewport().GetMousePosition();
		return new Vector3(position.X, position.Y, 0);
	}
	
	public void StartRotate()
	{
		previousPosition = GetMousePosition();
	}
	
	public void RotateAround()
	{
		var direction = previousPosition - GetMousePosition();
		swivel.RotateObjectLocal(Vector3.Left, direction.Y * rotationSpeedY);
		swivel.Basis = swivel.Basis.Rotated(Vector3.Up, direction.X * rotationSpeedX);
		
		previousPosition = GetMousePosition();
	}
	
	public bool CheckCameraRotation()
	{
		var direction = previousPosition - GetMousePosition();
		return direction.Length() > 0.001f;
	}
	
	public void AdjustZoom (float delta) {
		targetZoom = Mathf.Clamp(targetZoom - delta, stickMinZoom, stickMaxZoom);
	}
	
	public void SmoothZoom(double deltaTime)
	{
		var delta = (float) deltaTime;
		if (Mathf.Abs(currentZoom - targetZoom) > 0.0001f)
		{
			currentZoom = PsiMath.MoveTowards(currentZoom, targetZoom, delta * zoomSpeed);
			float distance = Mathf.Clamp(currentZoom, stickMinZoom, stickMaxZoom);
			stick.Position = new Vector3(0f, distance, 0);
		}
	}
}