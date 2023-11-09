using System.Numerics;
using CopperEngine.Components;
using CopperEngine.Resources;
using CopperEngine.Scenes;
using CopperEngine.Utils;
using Silk.NET.OpenGL;
using Color = System.Drawing.Color;
using Shader = CopperEngine.Rendering.Internal.Shader;

namespace CopperEngine;

// TODO: https://github.com/dotnet/Silk.NET/tree/main/examples/CSharp/OpenGL%20Tutorials
// Lighting
internal static class EngineRenderer
{
    private static bool initialized;
    private static GL Gl => EngineWindow.Gl!;
    internal static Shader Shader { get; private set; }

    internal static Action LoadPreModels;
    
    
    internal static Camera Camera = new();

    public static void Initialize()
    {
        if (initialized)
            return;
        initialized = true;
        
        var vertShader = ResourcesLoader.LoadTextResourceDirect("CopperEngine.Resources.Shaders.shader.vert");
        var fragShader = ResourcesLoader.LoadTextResourceDirect("CopperEngine.Resources.Shaders.shader.frag");
        Shader = new Shader(Gl, vertShader, fragShader);
    }
    
    internal static void Render()
    {
        Gl.Enable(EnableCap.DepthTest);
        EngineWindow.Gl?.ClearColor(Color.FromArgb(255, (int) (.45f * 255), (int) (.55f * 255), (int) (.60f * 255)));
        Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        
        Shader.Use();
        Shader.SetUniform("uTexture0", 0);
        
        SceneManager.CurrentSceneGameObjectsRender();
        SceneManager.GameObjectsRender(CopperEngine.Engine.EngineAssets);
        EngineEditor.Render();
    }

    internal static void Close()
    {
        Shader.Dispose();
    }
}