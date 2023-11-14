using System.Numerics;
using CopperEngine.Components;
using CopperEngine.Data;
using CopperEngine.Resources;
using CopperEngine.Utils;
using Silk.NET.OpenGL;
using Color = CopperEngine.Data.Color;
using PublicModel = CopperEngine.Rendering.Model;
using InternalShader = CopperEngine.Rendering.Internal.Shader;
using InternalTexture = CopperEngine.Rendering.Internal.Texture;
using InternalModel = CopperEngine.Rendering.Internal.Model;
using Silk.NET.OpenGL;

namespace CopperEngine.Rendering.Internal.RenderFeatures;

public class SkyboxFeature : RenderFeature
{
    public override RenderFeatureEvent Event { get; set; } = RenderFeatureEvent.BeforeRenderingGameObjects;

    private static GL Gl => EngineWindow.Gl!;

    private uint handle;
    private InternalModel model;
    
    public Transform Transform = new()
    {
        Position = Vector3.Zero,
        Scale = new Vector3(100),
        Rotation = Quaternion.Identity
    };
    private Matrix4x4 TransformViewMatrix
    {
        get => Transform?.Matrix ?? Matrix4x4.Identity;
        set => Transform.Matrix = value;
    }

    public Color TopColor = new(86, 108, 145);
    public Color MiddleColor = new(211, 238, 241);
    public Color BottomColor = new(105, 99, 94);

    public float HorizonOne = 0.225f;
    public float HorizonTwo = 0.230f;
    
    public override void Start()
    {
        var fragment = LoadShader(ShaderType.FragmentShader,ResourcesLoader.LoadTextResourceDirect("CopperEngine.Resources.Shaders.skybox.frag"));
        var vertex = LoadShader(ShaderType.VertexShader,ResourcesLoader.LoadTextResourceDirect("CopperEngine.Resources.Shaders.shader.vert"));
        
        handle = Gl.CreateProgram();
        
        Gl.AttachShader(handle, fragment);
        Gl.AttachShader(handle, vertex);
        
        Gl.LinkProgram(handle);

        Gl.GetProgram(handle, GLEnum.LinkStatus, out var status);
        
        if (status == 0)
            throw new Exception($"Program failed to link with error: {Gl.GetProgramInfoLog(handle)}");
        
        Gl.DetachShader(handle, fragment);
        Gl.DetachShader(handle, vertex);
        Gl.DeleteShader(fragment);
        Gl.DeleteShader(vertex);

        model = new Model(Gl, "Resources/Models/skybox.obj");
    }

    public override unsafe void Render()
    {
        Transform.Scale = new Vector3((EngineRenderer.Camera.ClippingPlane.Y * 2) - EngineRenderer.Camera.ClippingPlane.Y / 100);
        
        foreach (var mesh in model?.Meshes!)
        {
            Transform.Position = EngineRenderer.Camera.Position;
            
            mesh.Bind();
            Gl.UseProgram(handle);
            
            Gl.Uniform3(Gl.GetUniformLocation(handle, "topColor"), new Vector3(TopColor.R, TopColor.G, TopColor.B)/255);
            Gl.Uniform3(Gl.GetUniformLocation(handle, "middleColor"), new Vector3(MiddleColor.R, MiddleColor.G, MiddleColor.B)/255);
            Gl.Uniform3(Gl.GetUniformLocation(handle, "bottomColor"), new Vector3(BottomColor.R, BottomColor.G, BottomColor.B)/255);
            
            Gl.Uniform1(Gl.GetUniformLocation(handle, "horizon1"), HorizonOne);
            Gl.Uniform1(Gl.GetUniformLocation(handle, "horizon2"), HorizonTwo);
            
            
            var transformViewMatrix = TransformViewMatrix;
            Gl.UniformMatrix4(Gl.GetUniformLocation(handle, "uModel"), 1, false, (float*) &transformViewMatrix);
            Gl.UniformMatrix4(Gl.GetUniformLocation(handle, "modelMatrix"), 1, false, (float*) &transformViewMatrix);
            
            var viewMatrix = EngineRenderer.Camera.ViewMatrix;
            Gl.UniformMatrix4(Gl.GetUniformLocation(handle, "uView"), 1, false, (float*) &viewMatrix);
            
            var projectionMatrix = EngineRenderer.Camera.ProjectionMatrix;
            Gl.UniformMatrix4(Gl.GetUniformLocation(handle, "uProjection"), 1, false, (float*) &projectionMatrix);
            
            Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)mesh.Vertices.Length);
        }
    }

    public override void Stop()
    {
        model.Dispose();
    }
    
    private static uint LoadShader(ShaderType type, string data)
    {
        var shaderHandle = Gl.CreateShader(type);
        Gl.ShaderSource(shaderHandle, data);
        Gl.CompileShader(shaderHandle);
        var infoLog = Gl.GetShaderInfoLog(shaderHandle);
        
        if (!string.IsNullOrWhiteSpace(infoLog))
        {
            throw new Exception($"Error compiling shader of type {type}, failed with error {infoLog}");
        }

        return shaderHandle;
    }
}