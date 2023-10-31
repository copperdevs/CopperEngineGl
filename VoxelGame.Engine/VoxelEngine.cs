using Silk.NET.Input;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Data;
using VoxelGame.Engine.Info;
using VoxelGame.Engine.Scenes;

namespace VoxelGame.Engine;

public static class VoxelEngine
{
    private static bool initialized;
    internal static readonly Scene EngineAssets = Scene.CreateScene("Engine Assets");
    
    public static void Initialize()
    {
        if (initialized)
            return;
        initialized = true;
        
        VoxelWindow.Initialize(() =>
        {
            // engine initialize
            VoxelRenderer.Initialize();
            VoxelEditor.Initialize();
            Input.Initialize(VoxelWindow.InputContext?.Keyboards[0]);
            
            // engine assets
            EngineAssets.AddComponent(new CameraController());
            
            // testing stuff
            Input.RegisterInput(Key.Escape, VoxelWindow.Window!.Close, Input.RegisterType.Pressed);
        });

        VoxelWindow.Window!.Closing += () =>
        {
            SceneManager.CurrentSceneGameComponentsStop();
            SceneManager.GameComponentsStop(EngineAssets);
        };

        VoxelWindow.Window!.Update += delta =>
        {
            Input.CheckInput();
            VoxelWindow.SetTitle($"Voxel Game | Delta Time - {delta} | Size - <{VoxelWindow.Window.Size.X},{VoxelWindow.Window.Size.Y}>");

            // Console.WriteLine($"Mouse Scroll - {Input.MouseScroll}");
            // Console.WriteLine($"Mouse Position - <{Input.MousePosition.X},{Input.MousePosition.Y}>");
            // Console.WriteLine($"Mouse Delta - <{Input.MouseDelta}>");
            
            SceneManager.CurrentSceneGameComponentsUpdate();
            SceneManager.GameComponentsUpdate(EngineAssets);
        };
    }

    public static void Run()
    {
        VoxelWindow.Run();
        
        VoxelWindow.Window?.Dispose();
    }
}