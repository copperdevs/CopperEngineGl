using Jitter2.Collision.Shapes;
using VoxelGame.Engine;
using VoxelGame.Engine.Physics;
using VoxelGame.Engine.Rendering;
using VoxelGame.Engine.Scenes;

namespace VoxelGame.Testing;

public class PhysicsTestingApplication : VoxelApplication
{
    public override void Load()
    {
        var cubeScene = Scene.CreateScene("Cube Scene");

        var cube = cubeScene.CreateGameObject();
        cube.AddComponent(new CopperModel("Resources/Images/silk.png", "Resources/Models/cube.obj"));
        cube.AddComponent(new Rigidbody(new BoxShape(2)));
    }
}