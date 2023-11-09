using System.Numerics;
using CopperEngine.Data;
using CopperEngine.Info;
using CopperEngine.Utils;
using Silk.NET.Input;

namespace CopperEngine.Components;

internal class CameraController : GameComponent
{
    private static Vector2 lastMousePosition;
    public static float NormalMoveSpeed = 5f;
    public static float FastMoveSpeed = 15f;
    
    public static bool MoveFast => Input.IsKeyDown(Key.ShiftLeft);
    public static float TargetMoveSpeed { get; private set; }
    public static bool CanMove { get; private set; } = false;

    public override void Start()
    {
        //forward 
        Input.RegisterInput(Key.W, () =>
        {
            if(CanMove) EngineRenderer.Camera.Position += TargetMoveSpeed * EngineRenderer.Camera.Front;
        }, Input.RegisterType.Down);
        
        // back
        Input.RegisterInput(Key.S, () =>
        {
            if(CanMove) EngineRenderer.Camera.Position -= TargetMoveSpeed * EngineRenderer.Camera.Front;
        }, Input.RegisterType.Down);
        
        // left
        Input.RegisterInput(Key.A, () =>
        {
            if(CanMove) EngineRenderer.Camera.Position -= Vector3.Normalize(Vector3.Cross(EngineRenderer.Camera.Front, EngineRenderer.Camera.Up)) * TargetMoveSpeed;
        }, Input.RegisterType.Down);
        
        // right
        Input.RegisterInput(Key.D, () =>
        {
            if(CanMove) EngineRenderer.Camera.Position += Vector3.Normalize(Vector3.Cross(EngineRenderer.Camera.Front, EngineRenderer.Camera.Up)) * TargetMoveSpeed;
        }, Input.RegisterType.Down);
        
        // up
        Input.RegisterInput(Key.Space, () =>
        {
            if(CanMove) EngineRenderer.Camera.Position += TargetMoveSpeed * EngineRenderer.Camera.Up;
        }, Input.RegisterType.Down);
        
        // down
        Input.RegisterInput(Key.ControlLeft, () =>
        {
            if(CanMove) EngineRenderer.Camera.Position -= TargetMoveSpeed * EngineRenderer.Camera.Up;
        }, Input.RegisterType.Down);
        
        // zoom
        // Input.RegisterInput(scroll => { if(CanMove) VoxelRenderer.Camera.Zoom = Math.Clamp(VoxelRenderer.Camera.Zoom - scroll, 10.0f, 75f); });
    }

    public override void Update()
    {
        TargetMoveSpeed = MoveFast ? FastMoveSpeed * Time.DeltaTime : NormalMoveSpeed * Time.DeltaTime;

        CanMove = Input.IsKeyDown(MouseButton.Right);
        
        // Console.WriteLine(canMove);

        if (CanMove)
        {
            Input.SetCursorState(MouseMode.Disabled);
        }
        else
        {
            Input.SetCursorState(MouseMode.Normal);
            return;
        }
        
        
        const float lookSensitivity = 0.1f;
        if (lastMousePosition == default)
        {
            lastMousePosition = Input.MousePosition;
        }
        else
        {
            var xOffset = (Input.MousePosition.X - lastMousePosition.X) * lookSensitivity;
            var yOffset = (Input.MousePosition.Y - lastMousePosition.Y) * lookSensitivity;
            lastMousePosition = Input.MousePosition;
            EngineRenderer.Camera.Yaw += xOffset;
            EngineRenderer.Camera.Pitch -= yOffset;
        }

        //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
        EngineRenderer.Camera.Pitch = Math.Clamp(EngineRenderer.Camera.Pitch, -89.0f, 89.0f);

        EngineRenderer.Camera.Direction.X = MathF.Cos(MathUtil.DegreesToRadians(EngineRenderer.Camera.Yaw)) * MathF.Cos(MathUtil.DegreesToRadians(EngineRenderer.Camera.Pitch));
        EngineRenderer.Camera.Direction.Y = MathF.Sin(MathUtil.DegreesToRadians(EngineRenderer.Camera.Pitch));
        EngineRenderer.Camera.Direction.Z = MathF.Sin(MathUtil.DegreesToRadians(EngineRenderer.Camera.Yaw)) * MathF.Cos(MathUtil.DegreesToRadians(EngineRenderer.Camera.Pitch));
        EngineRenderer.Camera.Front = Vector3.Normalize(EngineRenderer.Camera.Direction);
        
        
    }
}