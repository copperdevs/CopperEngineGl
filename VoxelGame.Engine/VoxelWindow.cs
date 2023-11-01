using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using VoxelGame.Engine.Info;
using VoxelGame.Engine.Logs;

namespace VoxelGame.Engine;

internal static class VoxelWindow
{
    private static bool initialized;
    internal static bool WindowLoaded { get; private set; }

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
            VoxelEditor.Update(delta);
            VoxelRenderer.Render();
            Log.DeepInfo($"Window render - {delta}");
        };

        Window.Closing += () =>
        {
            VoxelEditor.ImGuiController?.Dispose();
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
        Log.DeepInfo($"Updating the window title = '{title}'");
        Window!.Title = title;
    }
}