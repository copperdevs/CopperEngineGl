using System.Numerics;
using Jitter2.Dynamics;

namespace CopperEngine.Utils;

public static class PhysicsUtil
{
    
    // BUG: Something wrong with the returned 4X4 matrix?
    public static Matrix4x4 GetTransformMatrix(RigidBody body)
    {
        var ori = body.Orientation;
        var pos = body.Position;

        return new Matrix4x4
        (
            ori.M11, ori.M12, ori.M13, pos.X,
            ori.M21, ori.M22, ori.M23, pos.Y,
            ori.M31, ori.M32, ori.M33, pos.Z,
            0, 0, 0, 1.0f
            );
    }
}