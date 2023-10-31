using System.Numerics;

namespace VoxelGame.Engine.Data;

public struct Transform
{
    public Transform() { }
    
    public Vector3 Position { get; set; } = Vector3.Zero;

    public float Scale { get; set; } = 1f;

    public Quaternion Rotation { get; set; } = Quaternion.Identity;

    //Note: The order here does matter.
    public Matrix4x4 ViewMatrix => 
        Matrix4x4.Identity * 
        Matrix4x4.CreateFromQuaternion(Rotation) * 
        Matrix4x4.CreateScale(Scale) * 
        Matrix4x4.CreateTranslation(Position);

    public static implicit operator Matrix4x4(Transform transform) => transform.ViewMatrix;
}