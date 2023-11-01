using System.Numerics;
using ImGuiNET;
using Silk.NET.Input;
using VoxelGame.Engine.Data;
using VoxelGame.Engine.Logs;

namespace VoxelGame.Engine.Info;

public static class Input
{
    private static readonly List<(Key, Action)> PressedInputActions = new();
    private static readonly List<(Key, Action)> DownInputActions = new();
    private static readonly List<Action<int>> MouseScrollInputActions = new();

    public static Vector2 MousePosition { get; private set; }

    private static IKeyboard? primaryKeyboard;
    private static IMouse? primaryMouse;

    public static bool IsKeyDown(Key key) => primaryKeyboard!.IsKeyPressed(key);
    public static bool IsKeyDown(MouseButton mouseButton) => primaryMouse!.IsButtonPressed(mouseButton);
    
    
    private static bool initialized;

    internal static void Initialize(IKeyboard? primaryKeyboard)
    {
        if (initialized)
            return;
        initialized = true;
        
        Log.Info("Initializing Input");

        Input.primaryKeyboard = primaryKeyboard;
        
        if(primaryKeyboard is not null)
            primaryKeyboard.KeyDown += KeyInput;

        
        var mice = VoxelWindow.InputContext?.Mice;

        if (mice is not null)
        {
            primaryMouse = mice[0];
            
            foreach (var mouse in mice)
            {
                mouse.Cursor.CursorMode = CursorMode.Normal;
                mouse.MouseMove += Input.MouseMoveInput;
                mouse.Scroll += Input.MouseScrollInput;
            }
            
            SetCursorState(MouseMode.Disabled);
        }
        
        foreach (var keyboard in VoxelWindow.InputContext?.Keyboards!)
        {
            keyboard.KeyDown += Input.KeyInput;
        }
    }

    public enum RegisterType
    {
        Pressed,
        Down
    }

    public static void RegisterInput(Key key, Action action, RegisterType registerType)
    {
        switch (registerType)
        {
            case RegisterType.Pressed:
                PressedInputActions.Add((key, action));
                break;
            case RegisterType.Down:
                DownInputActions.Add((key, action));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(registerType), registerType, null);
        }
    }

    public static void RegisterInput(Action<int> action)
    {
        MouseScrollInputActions.Add(action);
    }

    private static void KeyInput(IKeyboard keyboard, Key key, int keyCode)
    {
        var valueTuples = PressedInputActions.Where(input => input.Item1 == key);
        foreach (var input in valueTuples)
        {
            input.Item2.Invoke();
        }
    }

    private static void MouseScrollInput(IMouse mouse, ScrollWheel scrollWheel)
    {
        MouseScrollInputActions.ForEach(a => a.Invoke((int)scrollWheel.Y));
        Log.DeepInfo($"Mouse scroll input - {scrollWheel.Y}");
    }

    private static void MouseMoveInput(IMouse mouse, Vector2 position)
    {
        MousePosition = position;
        Log.DeepInfo($"New mouse position - {position}");
    }

    internal static void CheckInput()
    {
        foreach (var inputAction in DownInputActions.Where(inputAction => IsKeyDown(inputAction.Item1)))
        {
            inputAction.Item2.Invoke();
            Log.DeepInfo($"Invoking new input action - Key: {inputAction.Item1.ToString()}");
        }
    }

    public static void SetCursorState(MouseMode mouseMode)
    {
        foreach (var mouse in VoxelWindow.InputContext!.Mice)
        {
            if((CursorMode)mouseMode == mouse.Cursor.CursorMode)
                return;
            
            mouse.Cursor.CursorMode = (CursorMode)mouseMode;
            Log.DeepInfo($"New mouse cursor state - {mouse.Cursor.CursorMode.ToString()}");
        }
    }
}