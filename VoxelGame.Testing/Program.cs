using VoxelGame.Engine;
using VoxelGame.Engine.Scenes;

namespace VoxelGame.Testing;

public static class Program
{
    public static void Main()
    {
        VoxelEngine.Initialize();

        var scene = Scene.CreateScene("Test Scene", out var guid);
        scene.Load();
        
        VoxelEngine.Run();
    }
}