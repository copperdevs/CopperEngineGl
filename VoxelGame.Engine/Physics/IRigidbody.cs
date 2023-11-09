using Jitter2.Collision.Shapes;
using Jitter2.Dynamics;

namespace VoxelGame.Engine.Physics;

public interface IRigidbody
{
    public RigidBody JitterRigidbody { get; set; }
    public List<Shape> RigidbodyShape { get; set; }
}