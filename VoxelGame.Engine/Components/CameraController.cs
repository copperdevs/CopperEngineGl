using System.Numerics;
using Silk.NET.Input;
using VoxelGame.Engine.Data;
using VoxelGame.Engine.Info;
using VoxelGame.Engine.Utils;

namespace VoxelGame.Engine.Components;

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
            if(CanMove) VoxelRenderer.Camera.Position += TargetMoveSpeed * VoxelRenderer.Camera.Front;
        }, Input.RegisterType.Down);
        
        // back
        Input.RegisterInput(Key.S, () =>
        {
            if(CanMove) VoxelRenderer.Camera.Position -= TargetMoveSpeed * VoxelRenderer.Camera.Front;
        }, Input.RegisterType.Down);
        
        // left
        Input.RegisterInput(Key.A, () =>
        {
            if(CanMove) VoxelRenderer.Camera.Position -= Vector3.Normalize(Vector3.Cross(VoxelRenderer.Camera.Front, VoxelRenderer.Camera.Up)) * TargetMoveSpeed;
        }, Input.RegisterType.Down);
        
        // right
        Input.RegisterInput(Key.D, () =>
        {
            if(CanMove) VoxelRenderer.Camera.Position += Vector3.Normalize(Vector3.Cross(VoxelRenderer.Camera.Front, VoxelRenderer.Camera.Up)) * TargetMoveSpeed;
        }, Input.RegisterType.Down);
        
        // up
        Input.RegisterInput(Key.Space, () =>
        {
            if(CanMove) VoxelRenderer.Camera.Position += TargetMoveSpeed * VoxelRenderer.Camera.Up;
        }, Input.RegisterType.Down);
        
        // down
        Input.RegisterInput(Key.ControlLeft, () =>
        {
            if(CanMove) VoxelRenderer.Camera.Position -= TargetMoveSpeed * VoxelRenderer.Camera.Up;
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
            VoxelRenderer.Camera.Yaw += xOffset;
            VoxelRenderer.Camera.Pitch -= yOffset;
        }

        //We don't want to be able to look behind us by going over our head or under our feet so make sure it stays within these bounds
        VoxelRenderer.Camera.Pitch = Math.Clamp(VoxelRenderer.Camera.Pitch, -89.0f, 89.0f);

        VoxelRenderer.Camera.Direction.X = MathF.Cos(MathUtil.DegreesToRadians(VoxelRenderer.Camera.Yaw)) * MathF.Cos(MathUtil.DegreesToRadians(VoxelRenderer.Camera.Pitch));
        VoxelRenderer.Camera.Direction.Y = MathF.Sin(MathUtil.DegreesToRadians(VoxelRenderer.Camera.Pitch));
        VoxelRenderer.Camera.Direction.Z = MathF.Sin(MathUtil.DegreesToRadians(VoxelRenderer.Camera.Yaw)) * MathF.Cos(MathUtil.DegreesToRadians(VoxelRenderer.Camera.Pitch));
        VoxelRenderer.Camera.Front = Vector3.Normalize(VoxelRenderer.Camera.Direction);
        
        
    }
}