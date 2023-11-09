using System.Numerics;
using CopperEngine.Utils;

namespace CopperEngine.Components;

public class Camera
{
    public Vector3 Position = new(0.0f, 0.0f, 3.0f);
    public Vector3 Front = new(0.0f, 0.0f, -1.0f);
    public Vector3 Up = Vector3.UnitY;
    public Vector3 Direction = Vector3.Zero;
    public float Yaw = -90f;
    public float Pitch = 0f;
    public float Zoom = 45f;
    
    public Matrix4x4 ViewMatrix => Matrix4x4.CreateLookAt(Position, Position + Front, Up);
    
    
    public Matrix4x4 ProjectionMatrix => Matrix4x4.CreatePerspectiveFieldOfView(
        MathUtil.DegreesToRadians(Zoom), 
        (float)EngineWindow.Window!.Size.X / (float)EngineWindow.Window.Size.Y, 
        0.1f, 100.0f);
}