using Friflo.Engine.ECS;
using Godot;

namespace GodotSpaceGameSketch.Tracking
{
    /// <summary>
    /// Собственно маркер
    /// </summary>
    public partial class TargetMarkerView : Node2D
    {
        [Export]
        private Vector2 minSize = new(30, 30);
        
        private Entity trackable;
        public Entity Trackable => trackable;

        private TextureRect marker;
        private RichTextLabel distanceLabel;
        private int positionOffset;

        private Area2D area;
        private CollisionShape2D collisionShape;
        
        public override void _EnterTree()
        {
            base._EnterTree();
            marker = GetNode<TextureRect>("Marker");
            distanceLabel = GetNode<RichTextLabel>("DistanceLabel");
            area = GetNode<Area2D>("Area2D");
            collisionShape = area.GetNode<CollisionShape2D>("Collision");
            positionOffset = (int) (256 * marker.Scale.X / 2);

            //area.InputEvent += OnAreaInputEvent;
        }

        public void Attach(Entity trackable)
        {
            this.trackable = trackable;
        }

        public void ChangeColor(Color color)
        {
            marker.Modulate = color;
        }

        public void SetPosition(Vector2 position, float scale)
        {
            position.X -= positionOffset;
            position.Y -= positionOffset;

            Position = position;
        }

        public void SetSize(float size)
        {
            marker.Scale = new Vector2(size, size);
            if (collisionShape.Shape is RectangleShape2D rectShape)
            {
                rectShape.Size = new Vector2(256 * size, 256 * size);
            }
        }

        public void SetDistance(float distance)
        {
            distanceLabel.Text = distance.ToString("##");
            //distanceText.text = (distance * GameConstants.DistanceMod).ToString("##");
        }

        public void ShowDistance()
        {
            distanceLabel.Show();
        }
        
        public void HideDistance()
        {
            distanceLabel.Hide();
        }
        
        private void OnAreaInputEvent(Node viewport, InputEvent @event, long shapeIdx)
        {
            if (@event is InputEventMouseButton mouseEvent && 
                mouseEvent.ButtonIndex == MouseButton.Right && 
                mouseEvent.Pressed)
            {
                GD.Print($"Clicked on entity: {Trackable}");
                GetViewport().SetInputAsHandled(); // Prevent further processing
            }
        }
    }
}
