using Friflo.Engine.ECS;
using Godot;
using GodotSpaceGameSketch.Bodies.Components;
using GodotSpaceGameSketch.Core;

namespace GodotSpaceGameSketch.Tracking.HUD
{
    /// <summary>
    /// Отображение меток задетекченных целей на экране.
    /// </summary>
    public partial class TrackerHUD : Node3D
    {
        [Export]
        private Camera3D cachedCamera;
        
        private PackedScene defaultTargetMarker;
        
        private Godot.Collections.Array<TargetMarkerView> targetBoxPool = new();
        
        private EntityStore entityStore;

        public override void _EnterTree()
        {
            base._EnterTree();
            defaultTargetMarker = ResourceLoader.Load<PackedScene>("res://Prefabs/UI/TargetMarker.tscn");
            entityStore = WorldManager.Instance.World;
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            var player = WorldManager.Instance.playerController.controlledShip;
            var playerTracker = player.Entity.GetComponent<Components.Tracker>();

            var targets = WorldManager.Instance.ShipToTargetsStorage.Get(player.Entity.Id);

            var targetBoxPoolCount = targetBoxPool.Count;
            var targetsCount = targets.Count;
            if (targetBoxPoolCount < targetsCount)
            {
                for (int i = targetBoxPoolCount; i < targetsCount; i++)
                {
                    var targetMarker = defaultTargetMarker.Instantiate<TargetMarkerView>();
                    AddChild(targetMarker);
                    targetBoxPool.Add(targetMarker);
                }
            }
            
            int usedTargetBoxIndex = 0;
            for (int i = 0; i < targets.Count; i++)
            {
                var targetInfo = targets[i];
                var targetEntity = targetInfo.entity;
                if (targetEntity.IsNull)
                {
                    continue;
                }
                var targetPosition = targetEntity.GetComponent<CelestialBodyPosition>();
                
                /*if (!target.Trackable.Visible)
                {
                    continue;
                }*/

                var trackerPosition = targetPosition.value;
                
                if (cachedCamera.IsPositionInFrustum(trackerPosition))
                {
                    var centeredViewportPos = WorldToViewportPosition(trackerPosition);
                    
                    var targetBox = targetBoxPool[usedTargetBoxIndex];

                    var distance = player.Position.DistanceTo(trackerPosition);
                    if (distance < 50)
                    {
                        targetBox.HideDistance();
                    }
                    else if (distance > playerTracker.radius)
                    {
                        targetBox.Hide();
                        continue;
                    }
                    else
                    {
                        //var distanceToCamera = Vector3.Distance(cachedCamera.transform.position, trackerPosition);
                        targetBox.SetDistance(distance);
                        targetBox.ShowDistance();
                    }

                    targetBox.Attach(targetEntity);
                    targetBox.ChangeColor(targetInfo.owner == player.ShipOwnerId ? Colors.Green : Colors.Red);

                    var boxScale = 1 - Mathf.Lerp(0, 0.7f, distance / 250);
                    targetBox.SetPosition(centeredViewportPos, boxScale);
                    
                    //targetBox.SetSize(boxScale);

                    targetBox.Show();

                    usedTargetBoxIndex++;
                }
            }

            for (; usedTargetBoxIndex < targetBoxPool.Count; usedTargetBoxIndex++)
            {
                var targetBox = targetBoxPool[usedTargetBoxIndex];
                targetBox.Hide();
            }
        }
        
        private Vector2 WorldToViewportPosition(Vector3 position)
        {
            var pos = cachedCamera.UnprojectPosition(position);
            return pos;
        }
    }
}