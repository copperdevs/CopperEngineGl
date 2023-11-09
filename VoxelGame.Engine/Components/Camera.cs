using System.Numerics;
using CopperEngine.Utils;

namespace CopperEngine.Components;

internal class Camera
{
    internal Vector3 Position = new(0.0f, 0.0f, 3.0f);
    internal Vector3 Front = new(0.0f, 0.0f, -1.0f);
    internal Vector3 Up = Vector3.UnitY;
    internal Vector3 Direction = Vector3.Zero;
    internal float Yaw = -90f;
    internal float Pitch = 0f;
    internal float Zoom = 45f;
    
    internal Matrix4x4 ViewMatrix => Matrix4x4.CreateLookAt(Position, Position + Front, Up);
    internal Matrix4x4 ProjectionMatrix => Matrix4x4.CreatePerspectiveFieldOfView(
        MathUtil.DegreesToRadians(Zoom), 
        (float)EngineWindow.Window!.Size.X / (float)EngineWindow.Window.Size.Y, 
        0.1f, 100.0f);
}