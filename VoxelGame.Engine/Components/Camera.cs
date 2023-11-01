using System.Numerics;

namespace VoxelGame.Engine.Components;

internal class Camera
{
    internal Vector3 Position = new(0.0f, 0.0f, 3.0f);
    internal Vector3 Front = new(0.0f, 0.0f, -1.0f);
    internal Vector3 Up = Vector3.UnitY;
    internal Vector3 Direction = Vector3.Zero;
    internal float Yaw = -90f;
    internal float Pitch = 0f;
    internal float Zoom = 45f;
    
    internal Matrix4x4 ViewMatrix;
    internal Matrix4x4 ProjectionMatrix;
}