using System.Numerics;
using CopperEngine.Info;
using CopperEngine.Logs;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using SilkWindow = Silk.NET.Windowing.IWindow;
using SilkGl = Silk.NET.OpenGL.GL;
using SilkInput = Silk.NET.Input.IInputContext;

namespace CopperEngine;

public static class EngineWindow
{
    private static bool initialized;
    internal static bool WindowLoaded { get; private set; }

    internal static SilkWindow? Window;
    internal static SilkGl? Gl;
    internal static SilkInput? InputContext;

    public static Vector2 WindowSize => Window is null ? Vector2.Zero : new Vector2(Window.Size.X, Window.Size.Y);

    internal static void Initialize(Action loadAction)
    {
        if (initialized)
            return;
        initialized = true;
        
        
        
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(650, 400);
        options.Title = "CopperEngine";
        options.VSync = false;
        
        Window = Silk.NET.Windowing.Window.Create(options);

        // window.Load += OnLoad;
        // window.Update += OnUpdate;
        // window.Render += OnRender;

        Window!.Load += () =>
        {
            Gl = Window.CreateOpenGL();
            InputContext = Window.CreateInput();
            loadAction.Invoke();
            Log.Info("Window loaded");
            WindowLoaded = true;
        };
        
        Window.FramebufferResize += s =>
        {
            Gl?.Viewport(s);
            Log.DeepInfo($"Window frame buffer resize - <{s.X},{s.Y}>");
        };
        
        Window.Render += delta =>
        {
            Time.DeltaTime = (float) delta;
            Time.TotalTime = (float) Window.Time;
            EngineEditor.Update(delta);
            EngineRenderer.Render();
            Log.DeepInfo($"Window render - {delta}");
        };

        Window.Closing += () =>
        {
            EngineEditor.ImGuiController?.Dispose();
            Gl?.Dispose();
            InputContext?.Dispose();
            Log.Info("Closing Window");
            WindowLoaded = false;
        };
        
        Window.Move += position =>
        {
            Log.DeepInfo($"Window move position - <{position.X},{position.Y}>");
        };

        Window.Resize += size =>
        {
            Log.DeepInfo($"Window resized - <{size.X},{size.Y}>");
        };

        Window.StateChanged += state =>
        {
            Log.DeepInfo($"Window state changed - {state.ToString()}");
        };
    }

    internal static void Run()
    {
        Window!.Run();
        Log.Info("Running the window");
    }

    public static void SetTitle(string title)
    {
        if (Window is null) 
            return;
        
        Log.DeepInfo($"Updating the window title = '{title}'");
        Window.Title = title;
    }
}