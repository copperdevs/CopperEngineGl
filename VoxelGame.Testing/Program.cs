using VoxelGame.Engine;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Rendering;
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
        for (var i = 0; i < 5; i++)
        {
            gameObject.AddComponent<GameComponent>();
        }
        // gameObject.AddComponent(new CopperModel("Resources/Images/silk.png", "Resources/Models/cube.obj"));
        scene.AddGameObject(gameObject);
        
        VoxelEngine.Run();
    }
}