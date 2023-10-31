using System.Numerics;
using Silk.NET.OpenGL;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Data;
using VoxelGame.Engine.Utils;

namespace VoxelGame.Engine.Rendering;

public class CopperModel : GameComponent 
{
    public Matrix4x4 TransformViewMatrix => Transform?.ViewMatrix ?? Matrix4x4.Identity;

    private readonly Shader shader = VoxelRenderer.Shader;
    public Model Model { get; private set; }
    private readonly Texture texture;

    private static readonly GL Gl = VoxelWindow.Gl!;

    public CopperModel(string texturePath, string modelPath)
    {
        texture = new Texture(Gl, texturePath);
        Model = new Model(Gl, modelPath);
        
        VoxelRenderer.Models.Add(this);
    }

    public override void Render()
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

    public override void Stop()
    {
        Dispose();
    }

    private void Dispose()
    {
        Model.Dispose();
        texture.Dispose();
    }
}