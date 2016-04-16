using UnityEngine;

namespace TriangleMinecraft.Render.Helpers
{
    public static class VerticesHelper
    {
        public static Vector3[] Make(VoxelPoints type, bool spin90 = false)
        {
            Vector3[] result = MakeGenericTriangle();
            // Magic happend here

            switch (type)
            {
                case VoxelPoints.PointAB:
                    ApplyMatrix(ref result, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(180f, 180f, 0), Vector3.one));
                    break;
                case VoxelPoints.PointBA:
                    ApplyMatrix(ref result, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(180f, 0, 0), Vector3.one));
                    break;
                case VoxelPoints.PointBB:
                    ApplyMatrix(ref result, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 180f, 0), Vector3.one));
                    break;
            }

            if (spin90)
            {
                ApplyMatrix(ref result, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 90f, 0), Vector3.one));
            }

            return result;
        }

        private static void ApplyMatrix(ref Vector3[] result, Matrix4x4 mtx)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = mtx*result[i];
            }
        }

        public static Vector3[] MakeGenericTriangle()
        {
            return new[]
            {
                new Vector3(-1, -1, -1), new Vector3( 1, -1,  1), new Vector3(-1, -1,  1), // FOOT
                new Vector3(-1,  1,  1), new Vector3( 1, -1,  1), new Vector3(-1, -1, -1), // FACE
                new Vector3(-1, -1, -1), new Vector3(-1, -1,  1), new Vector3(-1,  1,  1), // RIGHT HAND
                new Vector3( 1, -1,  1), new Vector3(-1,  1,  1), new Vector3(-1, -1,  1), // LEFT HAND
            };
        }
    }
}