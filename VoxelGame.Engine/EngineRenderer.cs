using System.Numerics;
using CopperEngine.Components;
using CopperEngine.Rendering;
using CopperEngine.Rendering.Internal.RenderFeatures;
using CopperEngine.Resources;
using CopperEngine.Scenes;
using CopperEngine.Utils;
using Silk.NET.OpenGL;
using Silk.NET.Vulkan;
using Color = System.Drawing.Color;
using Shader = CopperEngine.Rendering.Internal.Shader;

namespace CopperEngine;

// TODO: Ambient Lighting - https://github.com/dotnet/Silk.NET/tree/main/examples/CSharp/OpenGL%20Tutorials/Tutorial%203.1%20-%20Ambient%20Lighting
// TODO: Diffuse Lighting - https://github.com/dotnet/Silk.NET/tree/main/examples/CSharp/OpenGL%20Tutorials/Tutorial%203.2%20-%20Diffuse%20Lighting
// TODO: Specular Lighting - https://github.com/dotnet/Silk.NET/tree/main/examples/CSharp/OpenGL%20Tutorials/Tutorial%203.3%20-%20Specular%20Lighting
// TODO: Materials - https://github.com/dotnet/Silk.NET/tree/main/examples/CSharp/OpenGL%20Tutorials/Tutorial%203.4%20-%20Materials
// TODO: Lighting Maps -  https://github.com/dotnet/Silk.NET/tree/main/examples/CSharp/OpenGL%20Tutorials/Tutorial%203.5%20-%20Lighting%20Maps
public static class EngineRenderer
{
    private static bool initialized;
    private static GL Gl => EngineWindow.Gl!;
    internal static Shader Shader { get; private set; }

    internal static List<RenderFeature> RenderFeatures = new();

    internal static List<Light> Lights = new();
    
    public static Camera Camera { get; internal set; } = new();

    internal static void Initialize()
    {
        if (initialized)
            return;
        initialized = true;
        
        var vertShader = ResourcesLoader.LoadTextResourceDirect("CopperEngine.Resources.Shaders.shader.vert");
        var fragShader = ResourcesLoader.LoadTextResourceDirect("CopperEngine.Resources.Shaders.shader.frag");
        Shader = new Shader(vertShader, fragShader);
        
        AddRenderFeature<SkyboxFeature>(); 
        
        RenderFeatures.ForEach(rf => rf.Start());
    }
    
    internal static void Render()
    {
        Gl.Enable(EnableCap.DepthTest);
        EngineWindow.Gl?.ClearColor(Color.FromArgb(255, (int) (.45f * 255), (int) (.55f * 255), (int) (.60f * 255)));
        Gl.Clear((uint) (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit));
        Shader.Use();
        Shader.SetUniform("uTexture0", 0);

        RenderFeatures.Where(rf => rf.Event == RenderFeatureEvent.BeforeRenderingGameObjects).ToList().ForEach(rf => rf.Render());
        SceneManager.CurrentSceneGameObjectsRender();
        SceneManager.GameObjectsRender(CopperEngine.Engine.EngineAssets);
        EngineEditor.Render();
        RenderFeatures.Where(rf => rf.Event == RenderFeatureEvent.AfterRenderingGameObjects).ToList().ForEach(rf => rf.Render());
        RenderFeatures.Where(rf => rf.Event == RenderFeatureEvent.BeforePostProcessing).ToList().ForEach(rf => rf.Render());
        RenderFeatures.Where(rf => rf.Event == RenderFeatureEvent.PostProcessing).ToList().ForEach(rf => rf.Render());
        RenderFeatures.Where(rf => rf.Event == RenderFeatureEvent.AfterPostProcessing).ToList().ForEach(rf => rf.Render());
        RenderFeatures.Where(rf => rf.Event == RenderFeatureEvent.AfterRendering).ToList().ForEach(rf => rf.Render());
    }

    internal static void Close()
    {
        Shader.Dispose();
    }

    public static void AddRenderFeature(RenderFeature feature) => RenderFeatures.Add(feature);
    public static void AddRenderFeature<T>() where T : RenderFeature, new() => AddRenderFeature(new T());
}