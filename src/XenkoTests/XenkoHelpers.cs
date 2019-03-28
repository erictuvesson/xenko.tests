namespace XenkoTests
{
    using Xenko.Core.Mathematics;
    using Xenko.Engine;

    public static class XenkoHelpers
    {
        public static (Vector3 forward, Vector3 up, Vector3 right) GetDirection(this TransformComponent transform)
        {
            var yawPitchRoll = transform.Rotation.YawPitchRoll;
            var rotation = Matrix.RotationYawPitchRoll(yawPitchRoll.X, yawPitchRoll.Y, 0);
            var forward = Vector3.TransformNormal(-Vector3.UnitZ, rotation);
            var up = Vector3.TransformNormal(Vector3.UnitY, rotation);
            var right = Vector3.Cross(forward, up);
            return (forward, up, right);
        }
    }
}
