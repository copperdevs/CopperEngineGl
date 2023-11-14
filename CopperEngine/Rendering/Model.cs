using System.Numerics;
using CopperEngine.Components;
using CopperEngine.Data;
using CopperEngine.Rendering.Internal;
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
    private static InternalShader? Shader => EngineRenderer.Shader;
    internal InternalModel? LoadedModel { get; private set; }
    private readonly InternalTexture? texture;
    private static readonly GL Gl = EngineWindow.Gl!;
    public List<Mesh> LoadedMeshes
    {
        get => LoadedModel?.Meshes ?? Array.Empty<Mesh>().ToList();
        set => LoadedModel!.Meshes = value;
    }

    private readonly string texturePath;
    private readonly string modelPath;
    
    /// <summary>
    /// Loads a new model
    /// </summary>
    /// <param name="texturePath">Path of the texture</param>
    /// <param name="modelPath">Path of the .obj model</param>
    public Model(string texturePath, string modelPath)
    {
        this.texturePath = texturePath;
        this.modelPath = modelPath;
        
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
        RenderModel(TransformViewMatrix);
    }

    protected void RenderModel(Transform transform) => RenderModel(transform.Matrix);

    protected void RenderModel(Matrix4x4 modelMatrix)
    {
        foreach (var mesh in LoadedModel?.Meshes!)
        {
            mesh.Bind();
            Shader?.Use();
            texture?.Bind();
            Shader?.SetUniform("uTexture0", 0);
            Shader?.SetUniform("uModel", modelMatrix);
            Shader?.SetUniform("uView", EngineRenderer.Camera.ViewMatrix);
            Shader?.SetUniform("uProjection", EngineRenderer.Camera.ProjectionMatrix);

            Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)mesh.Vertices.Length);
        }  
    }

    public override void RenderEditor()
    {
        ImGui.LabelText("Model Name", Path.GetFileName(modelPath));
        ImGui.LabelText("Model Path", modelPath);
        
        ImGui.Text("");
        
        ImGui.LabelText("Texture Name", Path.GetFileName(texturePath));
        ImGui.LabelText("Texture Path", texturePath);
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