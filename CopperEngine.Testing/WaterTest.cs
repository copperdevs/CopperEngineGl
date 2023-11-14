using CopperEngine.Components;
using CopperEngine.Rendering;

namespace CopperEngine.Testing;

public class WaterTest : GameComponent
{
    private Model model;
    
    public override void Start()
    {
        model = (Model)GetFirstComponent<Model>();
    }

    public override void Update()
    {
        for (var index = 0; index < model.LoadedMeshes[0].Vertices.Length; index++)
        {
            var vertice = model.LoadedMeshes[0].Vertices[index];
            vertice = Random.Shared.NextSingle();
            model.LoadedMeshes[0].Vertices[index] = vertice;
        }
    }
}