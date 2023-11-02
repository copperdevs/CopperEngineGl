using System.Numerics;
using Silk.NET.Input;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Data;
using VoxelGame.Engine.Info;
using VoxelGame.Engine.Logs;
using VoxelGame.Engine.Scenes;

namespace VoxelGame.Engine;

// TODO: add physics :3
// https://github.com/bepu/bepuphysics2/tree/master
// https://github.com/bepu/bepuphysics2/blob/master/Documentation/GettingStarted.md
// https://github.com/bepu/bepuphysics2/blob/master/Demos/Demos/SimpleSelfContainedDemo.cs
public static class VoxelEngine
{
    private static bool initialized;
    internal static readonly Scene EngineAssets = Scene.CreateScene("Engine Assets");
    internal static VoxelApplication Application;
    
    public static void Initialize<T>() where T : VoxelApplication, new()
    {
        if (initialized)
            return;
        initialized = true;

        Application = new T();
        
        VoxelWindow.Initialize(() =>
        {
            // engine initialize
            VoxelRenderer.Initialize();
            VoxelEditor.Initialize();
            Input.Initialize(VoxelWindow.InputContext?.Keyboards[0]);
            
            // engine assets
            EngineAssets.AddComponent<CameraController>();
            
            // testing stuff
            Input.RegisterInput(Key.Escape, VoxelWindow.Window!.Close, Input.RegisterType.Pressed);
            
            // app stuff
            // VoxelWindow.Window.Load += Application.Load;
            Application.Load();
            VoxelWindow.Window.Update += delta => Application.Update((float)delta);
            VoxelWindow.Window.Render += delta => Application.Render((float)delta);
            VoxelEditor.RenderEditor += Application.EditorRender;
            VoxelWindow.Window.Closing += Application.Close;
            VoxelWindow.Window.Move += position => Application.WindowMove(new Vector2(position.X, position.Y));
            VoxelWindow.Window.Resize += size => Application.WindowResize(new Vector2(size.X, size.Y));
            VoxelWindow.Window.StateChanged += Application.WindowStateChange;
            VoxelWindow.Window.FramebufferResize += size => Application.WindowFrameBufferResize(new Vector2(size.X, size.Y));
        });

        VoxelWindow.Window!.Closing += () =>
        {
            SceneManager.CurrentSceneGameObjectsStop();
            SceneManager.GameObjectsStop(EngineAssets);
            VoxelRenderer.Close();
        };

        VoxelWindow.Window!.Update += delta =>
        {
            Input.CheckInput();
            VoxelWindow.SetTitle($"Voxel Game | Delta Time - {delta} | Size - <{VoxelWindow.Window.Size.X},{VoxelWindow.Window.Size.Y}>");

            SceneManager.CurrentSceneGameObjectsUpdate();
            SceneManager.GameObjectsUpdate(EngineAssets);
        };
    }

    public static void Run()
    {
        VoxelWindow.Run();
        
        VoxelWindow.Window?.Dispose();
    }
}