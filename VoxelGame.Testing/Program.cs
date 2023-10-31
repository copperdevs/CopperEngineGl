using VoxelGame.Engine;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Scenes;

namespace VoxelGame.Testing;

public static class Program
{
    public static void Main()
    {
        VoxelEngine.Initialize();

        var scene = Scene.CreateScene("Test Scene", out var guid);
        scene.Load();

        var gameObject = new GameObject();
        gameObject.AddComponent<GameComponent>();
        gameObject.RemoveComponents<GameComponent>();
        scene.AddGameObject(gameObject);
        
        VoxelEngine.Run();
    }
}