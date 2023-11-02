using System.Numerics;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Data;
using VoxelGame.Engine.Rendering;

namespace VoxelGame.Engine.Collider;

public class CubeCollider : GameComponent, ICollider
{
    public CopperModel ColliderModel { get; set; } = new("Resources/Images/Colors/green.png", "Resources/Models/bounding_box.obj");
    public Transform ColliderSize = new();

    public override void Start()
    {
        ColliderSize.Scale = Vector3.One;
    }

    public override void Update()
    {
        ColliderModel.Transform = ColliderSize;
    }
}