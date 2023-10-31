
using System.Drawing;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using VoxelGame.Engine.Info;

namespace VoxelGame.Engine;

internal static class VoxelWindow
{
    private static bool initialized;

    internal static IWindow? Window;
    internal static GL? Gl;
    internal static IInputContext? InputContext;
    
    internal static void Initialize(Action loadAction)
    {
        if (initialized)
            return;
        initialized = true;
        
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(650, 400);
        options.Title = "VoxelWindow.Testing";
        
        Window = Silk.NET.Windowing.Window.Create(options);

        // window.Load += OnLoad;
        // window.Update += OnUpdate;
        // window.Render += OnRender;

        Window!.Load += () =>
        {
            Gl = Window.CreateOpenGL();
            InputContext = Window.CreateInput();
            loadAction.Invoke();
        };
        
        Window.FramebufferResize += s =>
        {
            Gl?.Viewport(s);
        };
        
        Window.Render += delta =>
        {
            Time.DeltaTime = (float) delta;
            Time.TotalTime = (float) Window.Time;
            VoxelEditor.Update(delta);
            VoxelRenderer.Render();
        };

        Window.Closing += () =>
        {
            VoxelEditor.imGuiController?.Dispose();
            Gl?.Dispose();
            InputContext?.Dispose();
        };
    }

    internal static void Run() => Window!.Run();

    public static void SetTitle(string title) => Window!.Title = title;
}