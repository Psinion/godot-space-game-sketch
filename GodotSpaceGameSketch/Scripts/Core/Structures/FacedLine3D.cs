using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace GodotSpaceGameSketch.Core.Structures;

[Tool]
public partial class FacedLine3D : MeshInstance3D
{
    private ArrayMesh mesh;
    private StandardMaterial3D material;
    
    private readonly List<Vector3> vertices = new();
    private readonly List<int> indices = new();
    private readonly List<Vector3> normals = new();
    private readonly List<Vector2> uvs = new();

    private Camera3D camera;
    private Vector3 cameraPosition;
    
    private Array<Vector3> points = new();
    
    [Export]
    public Array<Vector3> Points
    {
        get => points;
        set
        {
            points = value;
            if (Engine.IsEditorHint())
            {
                UpdateLineMesh();
            }
        }
    }
    
    private Array<float> widths = new();
    
    [Export]
    public Array<float> Widths
    {
        get => widths;
        set
        {
            widths = value;
            if (Engine.IsEditorHint())
            {
                UpdateLineMesh();
            }
        }
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        
        mesh = new ArrayMesh();
        Mesh = mesh;
        
        // Set up a simple material
        material = new StandardMaterial3D();
        material.ShadingMode = StandardMaterial3D.ShadingModeEnum.Unshaded;
        material.AlbedoColor = Colors.Red;
        MaterialOverride = material;
        
        UpdateLineMesh();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        UpdateLineMesh();
    }

    public void UpdateLineMesh()
    {
        if (points.Count < 2 || points.Count != widths.Count)
        {
            return;
        }
        
        mesh.ClearSurfaces();
        vertices.Clear();
        indices.Clear();
        normals.Clear();
        uvs.Clear();

        camera = GetViewport().GetCamera3D();
        cameraPosition = camera.GlobalPosition;

        CreateMeshCorner(points[0], points[1], widths[0]);
        for (int i = 1; i < points.Count; i++)
        {
            CreateMeshSegment(i, points[i - 1], points[i], widths[i]);
        }
        
        var surfaceArray = new Array();
        surfaceArray.Resize((int)Mesh.ArrayType.Max);
        surfaceArray[(int)Mesh.ArrayType.Vertex] = vertices.ToArray();
        surfaceArray[(int)Mesh.ArrayType.TexUV] = uvs.ToArray();
        surfaceArray[(int)Mesh.ArrayType.Index] = indices.ToArray();
        
        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);
    }

    private void CreateMeshCorner(Vector3 p1, Vector3 p2, float width)
    {
        var direction = p2 - p1;
        
        var toCamera = (cameraPosition - p1).Normalized();
        var halfWidthVector = direction.Cross(toCamera).Normalized() * width * 0.5f;

        vertices.Add(p1 + halfWidthVector);
        vertices.Add(p1 - halfWidthVector);
        
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
    }
    
    private void CreateMeshSegment(int index, Vector3 p1, Vector3 p2, float width)
    {
        var direction = p2 - p1;
        
        var toCamera = (cameraPosition - p1).Normalized();
        var halfWidthVector = direction.Cross(toCamera).Normalized() * width * 0.5f;
        
        var pointTop = vertices.Count - 2;
        var pointBottom = vertices.Count - 1;
        
        vertices.Add(p2 + halfWidthVector);
        vertices.Add(p2 - halfWidthVector);

        var uvPosition = index / (float) (points.Count - 1);
        uvs.Add(new Vector2(0, uvPosition));
        uvs.Add(new Vector2(1, uvPosition));
        
        AddQuad(pointTop, pointBottom, vertices.Count - 2, vertices.Count - 1);
    }

    private void AddQuad(int p1, int p2, int p3, int p4)
    {
        AddTriangle(p1, p2, p3);
        AddTriangle(p2, p4, p3);
    }
    
    private void AddTriangle(int p1, int p2, int p3)
    {
        indices.Add(p1);
        indices.Add(p2);
        indices.Add(p3);
    }
}