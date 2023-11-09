using System.Numerics;
using Jitter2.Dynamics;
using Jitter2.LinearMath;

namespace VoxelGame.Engine.Utils;

public static class Extensions
{
    public static Vector4 ToVector(this Quaternion quaternion) =>
        new(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    public static Quaternion ToQuaternion(this Vector4 vector) => new(vector.X, vector.Y, vector.Z, vector.W);
    
    public static Quaternion FromEulerAngles(this Vector3 euler) => MathUtil.FromEulerAngles(euler);
    public static Vector3 ToEulerAngles(this Quaternion quaternion) => MathUtil.ToEulerAngles(quaternion);
    public static Matrix4x4 GetTransformMatrix(this RigidBody rigidBody) => PhysicsUtil.GetTransformMatrix(rigidBody);
    
    public static Vector3 AreaInSphere(this Random random)
    {
        var xVal = (random.NextDouble() * 2) - 1;
        var yVal = (random.NextDouble() * 2) - 1;
        var zVal = (random.NextDouble() * 2) - 1;
        return Vector3.Normalize(new Vector3((float)xVal, (float)yVal, (float)zVal));
    }

    public static Vector3 ToVector(this JVector jVector) => new(jVector.X, jVector.Y, jVector.Z);
    public static JVector ToJVector(this Vector3 vector) => new(vector.X, vector.Y, vector.Z);
    public static Vector3 Scale(this Vector3 vector, float scale)
    {
        return vector.Scale(new Vector3(scale));
    }

    public static Vector3 Scale(this Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.X * vec2.X, vec1.Y * vec2.Y, vec1.Z * vec2.Z);
    }

    public static Vector3 WithX(this Vector3 vector, float value) => vector with { X = value };
    public static Vector3 WithY(this Vector3 vector, float value) => vector with { Y = value };
    public static Vector3 WithZ(this Vector3 vector, float value) => vector with { Z = value };
    public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max) => MathUtil.Clamp(value, min, max);
}