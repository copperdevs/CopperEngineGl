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

        var testCube = new GameObject();
        testCube.AddComponent(new CopperModel("Resources/Images/silk.png", "Resources/Models/cube.obj"));
        testCube.AddComponent<ReflectionTesting>();
        scene.AddGameObject(testCube);

        var testGizmo = new GameObject();
        testGizmo.AddComponent(new CopperModel("Resources/Images/silk.png", "Resources/Models/bounding_box.obj"));
        scene.AddGameObject(testGizmo);
    }
}