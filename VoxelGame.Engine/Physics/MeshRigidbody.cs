using Jitter2.Collision.Shapes;
using Jitter2.Dynamics;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Rendering;

namespace VoxelGame.Engine.Physics;

[UniqueComponent, RequireComponent(typeof(CopperModel))]
public class MeshRigidbody : GameComponent, IRigidbody
{
    public RigidBody JitterRigidbody { get; set; }
    public List<Shape> RigidbodyShape { get; set; }
}