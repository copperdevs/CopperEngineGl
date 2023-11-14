using CopperEngine.Components;
using CopperEngine.Rendering;
using Jitter2.Collision.Shapes;
using Jitter2.Dynamics;

namespace CopperEngine.Physics;

[UniqueComponent, RequireComponent(typeof(Model))]
public class MeshRigidbody : GameComponent, IRigidbody
{
    public RigidBody JitterRigidbody { get; set; }
    public List<Shape> RigidbodyShape { get; set; }
}