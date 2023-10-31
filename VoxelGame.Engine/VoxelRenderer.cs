using System.Drawing;
using System.Numerics;
using ImGuiNET;
using Silk.NET.OpenGL;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Data;
using VoxelGame.Engine.Rendering;
using VoxelGame.Engine.Scenes;
using VoxelGame.Engine.Utils;
using Color = System.Drawing.Color;
using Shader = VoxelGame.Engine.Rendering.Shader;
using Texture = VoxelGame.Engine.Rendering.Texture;

namespace VoxelGame.Engine;

// TODO: https://github.com/dotnet/Silk.NET/tree/main/examples/CSharp/OpenGL%20Tutorials
// Lighting
internal static class VoxelRenderer
{
    private static bool initialized;
    private static GL Gl => VoxelWindow.Gl!;
    internal static Shader Shader { get; private set; }

    internal static readonly List<CopperModel> Models = new();
    
    internal static Camera Camera = new();
    internal static Matrix4x4 ViewMatrix;
    internal static Matrix4x4 ProjectionMatrix;

    public static void Initialize()
    {
        if (initialized)
            return;
        initialized = true;
        
        var vertShader = ResourcesLoader.LoadTextResourceDirect("VoxelGame.Engine.Resources.Shaders.shader.vert");
        var fragShader = ResourcesLoader.LoadTextResourceDirect("VoxelGame.Engine.Resources.Shaders.shader.frag");
        Shader = new Shader(Gl, vertShader, fragShader);
        
        var model = new CopperModel("Resources/Images/silk.png", "Resources/Models/cube.obj");
    }
    
    internal static void Render()
    {
        Gl.Enable(EnableCap.DepthTest);
        VoxelWindow.Gl?.ClearColor(Color.FromArgb(255, (int) (.45f * 255), (int) (.55f * 255), (int) (.60f * 255)));
        Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        
        Shader.Use();
        Shader.SetUniform("uTexture0", 0);
        
        var camera = VoxelRenderer.Camera;
        ViewMatrix = Matrix4x4.CreateLookAt(camera.Position, camera.Position + camera.Front, camera.Up);
        ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathUtil.DegreesToRadians(camera.Zoom), (float)VoxelWindow.Window!.Size.X / (float)VoxelWindow.Window.Size.Y, 0.1f, 100.0f);

        Models.ForEach(model => model.Render());
        
        SceneManager.CurrentSceneGameComponentsRender();
        SceneManager.GameComponentsRender(VoxelEngine.EngineAssets);
        VoxelEditor.Render();
    }

    internal static void Close()
    {
        Shader.Dispose();
        Models.ForEach(model => model.Dispose());
    }
}