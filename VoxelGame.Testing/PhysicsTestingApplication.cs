using CopperEngine;
using CopperEngine.Physics;
using CopperEngine.Rendering;
using CopperEngine.Scenes;
using Jitter2.Collision.Shapes;
namespace VoxelGame.Testing;

public class PhysicsTestingApplication : GameApplication
{
    public override void Load()
    {
        var cubeScene = Scene.CreateScene("Cube Scene");

        var cube = cubeScene.CreateGameObject();
        cube.AddComponent(new Model("Resources/Images/silk.png", "Resources/Models/cube.obj"));
        cube.AddComponent(new Rigidbody(new BoxShape(2)));
    }
}