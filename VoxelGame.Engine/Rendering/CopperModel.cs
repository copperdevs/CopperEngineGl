using System.Numerics;
using Silk.NET.OpenGL;
using VoxelGame.Engine.Data;
using VoxelGame.Engine.Utils;

namespace VoxelGame.Engine.Rendering;

public struct CopperModel
{
    public Transform Transform { get; private set; }= new()
    {
        Position = Vector3.Zero,
        Scale = 1,
        Rotation = Quaternion.Identity
    };

    public readonly Matrix4x4 TransformViewMatrix => Transform.ViewMatrix;
    
    private readonly Shader shader = VoxelRenderer.Shader;
    public Model Model { get; private set; }
    private readonly Texture texture;

    private static readonly GL Gl = VoxelWindow.Gl!;

    public CopperModel(string texturePath, string modelPath)
    {
        texture = new Texture(Gl, texturePath);
        Model = new Model(Gl, modelPath);
        
        VoxelRenderer.Models.Add(this);

        Transform = new Transform
        {
            Position = Vector3.Zero,
            Scale = 1,
            Rotation = Quaternion.Identity
        };
    }

    internal void Render()
    {
        foreach (var mesh in Model.Meshes)
        {
            mesh.Bind();
            shader.Use();
            texture.Bind();
            shader.SetUniform("uTexture0", 0);
            shader.SetUniform("uModel", TransformViewMatrix);
            shader.SetUniform("uView", VoxelRenderer.ViewMatrix);
            shader.SetUniform("uProjection", VoxelRenderer.ProjectionMatrix);

            Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)mesh.Vertices.Length);
        }
    }
    internal void Dispose()
    {
        Model.Dispose();
        texture.Dispose();
    }
}