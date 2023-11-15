using System.Numerics;
using CopperEngine.Mathematics;
using ImGuiNET;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace CopperEngine.Editor;

internal static class SceneView
{
    internal static bool IsOpen = true;
    private static bool initialized;
    
    private static uint framebuffer;
    private static uint texture;

    private static GL Gl => EngineWindow.Gl!;
    
    internal static void Initialize()
    {
        if (initialized)
            return;
        initialized = true;
    }

    internal static void Update()
    {
        
    }

    internal static void Render()
    {
        if (!IsOpen)
            return;

        if (ImGui.Begin("Scene", ref IsOpen, ImGuiWindowFlags.None))
        {
            if (ObjectBrowserTab.currentObjectBrowserTarget is not null && ObjectBrowserTab.currentObjectBrowserTargetTransformOpen)
                Gizmo.Draw(ref ObjectBrowserTab.currentObjectBrowserTarget);
            
            ImGui.End();
        }
    }

    private static void Resize(Vector2D<int> vector2D)
    {
        
    }
}