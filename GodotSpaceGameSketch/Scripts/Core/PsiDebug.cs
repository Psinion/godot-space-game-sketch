using System.Collections.Generic;
using Godot;

namespace GodotSpaceGameSketch.Core;

public partial class PsiDebug : SingletonNode3D<PsiDebug>
{
    private struct Point
    {
        public Vector3 Position { get; }
        public Color Color { get; }
        public float Size { get; }
        public float Duration { get; }
        public float StartTime { get; }

        public Point(Vector3 position, Color color, float size, float duration)
        {
            Position = position;
            Color = color;
            Size = size;
            Duration = duration;
            StartTime = Time.GetTicksMsec() * 0.001f;
        }
    }
    
    private struct Line
    {
        public Vector3 Start { get; }
        public Vector3 End { get; }
        public Color Color { get; }
        public float Duration { get; }
        public float StartTime { get; }

        public Line(Vector3 start, Vector3 end, Color color, float duration)
        {
            Start = start;
            End = end;
            Color = color;
            Duration = duration;
            StartTime = Time.GetTicksMsec() * 0.001f;
        }
    }
    
    private float deltaTime;
    private ImmediateMesh mesh;
    private MeshInstance3D meshInstance;

    private readonly List<Line> lines = [];
    private readonly List<Point> points = [];
    
    public override void _Ready()
    {
        mesh = new ImmediateMesh();
        meshInstance = new MeshInstance3D();
        
        var material = new StandardMaterial3D {
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
            VertexColorUseAsAlbedo = true,
            NoDepthTest = true
        };
        
        AddChild(meshInstance);
        meshInstance.Mesh = mesh;
        meshInstance.CastShadow = GeometryInstance3D.ShadowCastingSetting.Off;
        meshInstance.MaterialOverride = material;
    }
    
    public override void _Process(double delta)
    {
        deltaTime = (float)delta;
        RemoveExpired();
        DrawAll();
    }
    
    private void RemoveExpired()
    {
        float currentTime = Time.GetTicksMsec() * 0.001f;

        lines.RemoveAll(line =>
            line.Duration > 0 && currentTime - line.StartTime >= line.Duration);
        points.RemoveAll(point =>
            point.Duration > 0 && currentTime - point.StartTime >= point.Duration);
    }

    private void DrawAll()
    {
        mesh.ClearSurfaces();

        foreach (var line in lines)
        {
            DrawLineImmediate(line.Start, line.End, line.Color);
        }

        foreach (var point in points)
        {
            DrawPointImmediate(point.Position, point.Color, point.Size);
        }
    }

    private void DrawLineImmediate(Vector3 start, Vector3 end, Color color)
    {
        mesh.SurfaceBegin(Mesh.PrimitiveType.Lines);
        mesh.SurfaceSetColor(color);
        mesh.SurfaceAddVertex(start);
        mesh.SurfaceAddVertex(end);
        mesh.SurfaceEnd();
    }

    private void DrawPointImmediate(Vector3 position, Color color, float size)
    {
        float halfSize = size / 2;
        mesh.SurfaceBegin(Mesh.PrimitiveType.Lines);
        mesh.SurfaceSetColor(color);

        // X-axis
        mesh.SurfaceAddVertex(position - Vector3.Right * halfSize);
        mesh.SurfaceAddVertex(position + Vector3.Right * halfSize);
        // Y-axis
        mesh.SurfaceAddVertex(position - Vector3.Up * halfSize);
        mesh.SurfaceAddVertex(position + Vector3.Up * halfSize);
        // Z-axis
        mesh.SurfaceAddVertex(position - Vector3.Back * halfSize);
        mesh.SurfaceAddVertex(position + Vector3.Back * halfSize);

        mesh.SurfaceEnd();
    }
    
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0)
    {
        Instance.lines.Add(new Line(start, end, color, duration == 0 ? Instance.deltaTime : duration));
    }

    public static void DrawRay(Vector3 start, Vector3 direction, Color color, float duration = 0)
    {
        DrawLine(start, start + direction, color, duration);
    }

    public static void DrawPoint(Vector3 position, Color color, float size = 1f, float duration = 0)
    {
        Instance.points.Add(new Point(position, color, size, duration == 0 ? Instance.deltaTime : duration));
    }
}