using System.Numerics;
using VoxelGame.Engine.Components;
using VoxelGame.Engine.Data;
using VoxelGame.Engine.Scenes;

namespace VoxelGame.Testing;

public class ReflectionTesting : GameComponent
{
    public float FloatField;
    public int IntField;
    public bool BoolField;
    public Vector2 Vector2Field;
    public Vector3 Vector3Field;
    public Vector4 Vector4Field;
    public Quaternion QuaternionField;
    public Guid GuidField;
    public Scene SceneField = Scene.CreateScene("Scene Field");
    public Transform TransformField = new();
}