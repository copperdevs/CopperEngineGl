using System.Numerics;
using CopperEngine.Components;
using Silk.NET.OpenGL;
using Shader = CopperEngine.Rendering.Shader;
using Texture = CopperEngine.Rendering.Texture;

namespace CopperEngine.Rendering;

public class CopperModel : GameComponent
{
    internal Guid Id;
    
    private static readonly Dictionary<string, Model> ModelLibrary = new();
    private static readonly Dictionary<string, Texture> TextureLibrary = new();
    
    public Matrix4x4 TransformViewMatrix => Transform?.Matrix ?? Matrix4x4.Identity;
    private readonly Shader? shader = EngineRenderer.Shader;
    internal Model? Model { get; private set; }
    private Texture? texture;
    private static readonly GL Gl = EngineWindow.Gl!;
    

    public CopperModel(string texturePath, string modelPath)
    {
        if (ModelLibrary.TryGetValue(modelPath, out var modelValue))
            Model = modelValue;
        else
        {
            var loadedModel = new Model(Gl, modelPath);
            ModelLibrary.Add(modelPath, loadedModel);
            Model = loadedModel;
        }
        
        if (TextureLibrary.TryGetValue(texturePath, out var textureValue))
            texture = textureValue;
        else
        {
            var loadedTexture = new Texture(Gl, texturePath);
            TextureLibrary.Add(texturePath, loadedTexture);
            texture = loadedTexture;
        }

        Id = Model.Id;
    }

    public override void Render()
    {
        foreach (var mesh in Model?.Meshes!)
        {
            mesh.Bind();
            shader?.Use();
            texture?.Bind();
            shader?.SetUniform("uTexture0", 0);
            shader?.SetUniform("uModel", TransformViewMatrix);
            shader?.SetUniform("uView", EngineRenderer.Camera.ViewMatrix);
            shader?.SetUniform("uProjection", EngineRenderer.Camera.ProjectionMatrix);

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