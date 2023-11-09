using System.Numerics;
using CopperEngine;
using CopperEngine.Logs;
using CopperEngine.Rendering;
using CopperEngine.Scenes;

namespace VoxelGame.Testing;

public class TestingApplication : GameApplication
{
    public override void Load()
    {
        var scene = Scene.CreateScene("Test Scene");
        scene.Load();

        scene.CreateGameObject();

        var testCube = scene.CreateGameObject();
        testCube.AddComponent(new Model("Resources/Images/silk.png", "Resources/Models/cube.obj"));
        testCube.AddComponent<ReflectionTesting>();


        var chunkScene = Scene.CreateScene("Chunk Scene");
        
        var chunk = new Chunk();

        for (var x = 0; x < Chunk.ChunkSizeX; x++)
        {
            for (var y = 0; y < Chunk.ChunkSizeY; y++)
            {
                for (var z = 0; z < Chunk.ChunkSizeZ; z++)
                {
                    var value = Random.Shared.Next(0, 2);
                    chunk.Blocks[x, y, z] = value;
                    Log.Info($" | Block: {chunk.Blocks[x,y,z]} | Position <{x},{y},{z}> | ");

                    if (value is not 1) 
                        continue;
                    
                    var cube = chunkScene.CreateGameObject();
                    cube.AddComponent(new Model("Resources/Images/silk.png", "Resources/Models/cube.obj"));
                    cube.Transform.Position = new Vector3(x * 2, y * 2, z * 2);
                }
            }
        }
    }
}