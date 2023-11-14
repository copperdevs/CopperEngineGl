using System.Numerics;
using CopperEngine.Components;
using CopperEngine.Rendering.Internal;
using CopperEngine.Rendering.Internal.RenderFeatures;
using ImGuiNET;
using Silk.NET.OpenGL;
using InternalShader = CopperEngine.Rendering.Internal.Shader;
using InternalTexture = CopperEngine.Rendering.Internal.Texture;
using InternalModel = CopperEngine.Rendering.Internal.Model;

namespace CopperEngine.Rendering;

public class Model : GameComponent
{
    internal Guid Id;
    
    private static readonly Dictionary<string, InternalModel> ModelLibrary = new();
    private static readonly Dictionary<string, InternalTexture> TextureLibrary = new();

    private Matrix4x4 TransformViewMatrix => Transform?.Matrix ?? Matrix4x4.Identity;
    private readonly InternalShader? shader = EngineRenderer.Shader;
    internal InternalModel? LoadedModel { get; private set; }
    private readonly InternalTexture? texture;
    private static readonly GL Gl = EngineWindow.Gl!;

    private string TexturePath;
    private string ModelPath;
    
    /// <summary>
    /// Loads a new model
    /// </summary>
    /// <param name="texturePath">Path of the texture</param>
    /// <param name="modelPath">Path of the .obj model</param>
    public Model(string texturePath, string modelPath)
    {
        TexturePath = texturePath;
        ModelPath = modelPath;
        
        if (ModelLibrary.TryGetValue(modelPath, out var modelValue))
            LoadedModel = modelValue;
        else
        {
            var loadedModel = new InternalModel(Gl, modelPath);
            ModelLibrary.Add(modelPath, loadedModel);
            LoadedModel = loadedModel;
        }
        
        if (TextureLibrary.TryGetValue(texturePath, out var textureValue))
            texture = textureValue;
        else
        {
            var loadedTexture = new InternalTexture(Gl, texturePath);
            TextureLibrary.Add(texturePath, loadedTexture);
            texture = loadedTexture;
        }

        Id = LoadedModel.Id;
    }

    public override void Render()
    {
        foreach (var mesh in LoadedModel?.Meshes!)
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

    public override void RenderEditor()
    {
        ImGui.LabelText("Model Name", Path.GetFileName(ModelPath));
        ImGui.LabelText("Model Path", ModelPath);
        
        ImGui.Text("");
        
        ImGui.LabelText("Texture Name", Path.GetFileName(TexturePath));
        ImGui.LabelText("Texture Path", TexturePath);
    }

    public override void Stop()
    {
        Dispose();
    }

    private void Dispose()
    {
        LoadedModel?.Dispose();
        texture?.Dispose();
    }
}