using System.Numerics;

namespace VoxelGame.Engine.Utils;

public static class Extensions
{
    public static Vector4 ToVector(this Quaternion quaternion) =>
        new Vector4(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    public static Quaternion ToQuaternion(this Vector4 vector) =>
        new Quaternion(vector.X, vector.Y, vector.Z, vector.W);
    
    public static Quaternion FromEulerAngles(this Vector3 euler) => MathUtil.FromEulerAngles(euler);
    public static Vector3 ToEulerAngles(this Quaternion quaternion) => MathUtil.ToEulerAngles(quaternion);
}