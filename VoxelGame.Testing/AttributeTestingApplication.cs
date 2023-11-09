using CopperEngine;
using CopperEngine.Scenes;

namespace VoxelGame.Testing;

public class AttributeTestingApplication : GameApplication
{
    public override void Load()
    {
        var scene = Scene.CreateScene("Test Scene");
        scene.Load();

        {
            var normalGameObject = scene.CreateGameObject();
            for (var i = 0; i < 5; i++)
            {
                normalGameObject.AddComponent<NormalComponent>();
            }
        }
        
        {
            var uniqueGameObject = scene.CreateGameObject();
            for (var i = 0; i < 5; i++)
            {
                uniqueGameObject.AddComponent<UniqueComponent>();
            }
        }
    }
}