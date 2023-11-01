using VoxelGame.Engine;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Logs;
using VoxelGame.Engine.Rendering;
using VoxelGame.Engine.Scenes;

namespace VoxelGame.Testing;

public class TestingApplication : VoxelApplication
{
    public override void Load()
    {
        Log.Info("loading");
        
        var scene = Scene.CreateScene("Test Scene", out var guid);
        scene.Load();

        var gameObject = new GameObject();
        gameObject.AddComponent(new CopperModel("Resources/Images/silk.png", "Resources/Models/cube.obj"));
        scene.AddGameObject(gameObject);
    }
}