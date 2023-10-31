using System.Drawing;
using System.Numerics;
using ImGuiNET;
using Silk.NET.OpenGL;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Rendering;
using VoxelGame.Engine.Scenes;
using VoxelGame.Engine.Utils;
using Color = System.Drawing.Color;
using Shader = VoxelGame.Engine.Rendering.Shader;
using Texture = VoxelGame.Engine.Rendering.Texture;

namespace VoxelGame.Engine;

internal static class VoxelRenderer
{
    private static bool initialized;
    private static GL Gl => VoxelWindow.Gl!;
    private static Shader Shader;
    internal static Camera Camera = new();
    
    public static void Initialize()
    {
        if (initialized)
            return;
        initialized = true;

        var vertShader = ResourcesLoader.LoadTextResourceDirect("VoxelGame.Engine.Resources.Shaders.shader.vert");
        var fragShader = ResourcesLoader.LoadTextResourceDirect("VoxelGame.Engine.Resources.Shaders.shader.frag");
        Shader = new Shader(Gl, vertShader, fragShader);
        
        Ebo = new BufferObject<uint>(Gl, Indices, BufferTargetARB.ElementArrayBuffer);
        Vbo = new BufferObject<float>(Gl, Vertices, BufferTargetARB.ArrayBuffer);
        Vao = new VertexArrayObject<float, uint>(Gl, Vbo, Ebo);

        Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
        Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);

        Texture = new Texture(Gl, "Resources/Images/silk.png");
    }
    
    internal static void Render()
    {
        Gl.Enable(EnableCap.DepthTest);
        VoxelWindow.Gl?.ClearColor(Color.FromArgb(255, (int) (.45f * 255), (int) (.55f * 255), (int) (.60f * 255)));
        Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        
        Vao.Bind();
        Texture.Bind();
        Shader.Use();
        Shader.SetUniform("uTexture0", 0);
        
        var difference = (float) (VoxelWindow.Window!.Time * 100);
        // var difference = 0;

        var model = Matrix4x4.CreateRotationY(MathUtil.DegreesToRadians(difference)) * Matrix4x4.CreateRotationX(MathUtil.DegreesToRadians(difference));
        var view = Matrix4x4.CreateLookAt(Camera.Position, Camera.Position + Camera.Front, Camera.Up);
        var projection = Matrix4x4.CreatePerspectiveFieldOfView(MathUtil.DegreesToRadians(Camera.Zoom), (float)VoxelWindow.Window.Size.X / VoxelWindow.Window.Size.Y, 0.1f, 100.0f);
        
        Shader.SetUniform("uModel", model);
        Shader.SetUniform("uView", view);
        Shader.SetUniform("uProjection", projection);
        
        Gl.DrawArrays(PrimitiveType.Triangles, 0, 36);
        
        SceneManager.CurrentSceneGameComponentsRender();
        SceneManager.GameComponentsRender(VoxelEngine.EngineAssets);
        VoxelEditor.Render();
    }

    internal static void Close()
    {
        Shader.Dispose();
    }
    
    private static readonly float[] Vertices =
    {
        //X    Y      Z     U   V
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
        0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 0.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

        -0.5f, -0.5f,  0.5f,  0.0f, 1.0f,
        0.5f, -0.5f,  0.5f,  1.0f, 1.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 1.0f,

        -0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,  1.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  1.0f, 1.0f,

        0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 0.0f,
        0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
        0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
        0.5f, -0.5f,  0.5f,  0.0f, 1.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 1.0f,

        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
        0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
        0.5f, -0.5f,  0.5f,  1.0f, 1.0f,
        0.5f, -0.5f,  0.5f,  1.0f, 1.0f,
        -0.5f, -0.5f,  0.5f,  0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

        -0.5f,  0.5f, -0.5f,  0.0f, 0.0f,
        0.5f,  0.5f, -0.5f,  1.0f, 0.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
        -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
        -0.5f,  0.5f, -0.5f,  0.0f, 0.0f
    };
    
    private static BufferObject<float> Vbo;
    private static BufferObject<uint> Ebo;
    private static VertexArrayObject<float, uint> Vao;
    private static Texture Texture;

    private static readonly uint[] Indices =
    {
        0, 1, 3,
        1, 2, 3
    };
}