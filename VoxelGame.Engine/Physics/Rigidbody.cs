using ImGuiNET;
using Jitter2.Collision.Shapes;
using Jitter2.Dynamics;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Data;
using VoxelGame.Engine.Logs;
using VoxelGame.Engine.Utils;
using JitterRigidbody = Jitter2.Dynamics.RigidBody;

namespace VoxelGame.Engine.Physics;

[UniqueComponent]
public class Rigidbody : GameComponent, IRigidbody
{
    public JitterRigidbody JitterRigidbody { get; set; }
    public List<Shape> RigidbodyShape { get; set; }
    
    public Rigidbody(Shape shape) : this(new List<Shape> {shape}) { }
    public Rigidbody(List<Shape> shape)
    {
        RigidbodyShape = shape;
    }
    
    public override void Start()
    {
        JitterRigidbody = ParentScene?.PhysicsWorld.CreateRigidBody()!;
        
    }

    public override void PreFixedUpdate()
    {
        // JitterRigidbody.Position = Transform!.Position.ToJVector();
    }

    public override void PostFixedUpdate()
    {
        Transform.Matrix = JitterRigidbody.GetTransformMatrix();
        Transform.Position = JitterRigidbody.Position.ToVector();
    }

    public override void RenderEditor()
    {
        var rigidBodyPos = JitterRigidbody.Position.ToVector();
        ImGui.DragFloat3("Rigidbody Position", ref rigidBodyPos);

        EditorUtil.DragMatrix3X3("Rigidbody Rotation", JitterRigidbody.Orientation);
        

        EditorUtil.DragMatrix4X4("Transform Matrix", Transform.Matrix);
        EditorUtil.DragMatrix4X4("Rigidbody Matrix", JitterRigidbody.GetTransformMatrix());
    }
}