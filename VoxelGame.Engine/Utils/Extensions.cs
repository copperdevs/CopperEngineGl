using System.Numerics;

namespace VoxelGame.Engine.Utils;

public static class Extensions
{
    public static Vector4 ToVector(this Quaternion quaternion)
    {
        return new Vector4(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
    }
}