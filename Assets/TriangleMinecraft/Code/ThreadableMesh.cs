using UnityEngine;

namespace TriangleMinecraft
{
    public class ThreadableMesh
    {
        public VoxelMap World { get; set; }
        public Vector3[] vertices { get; set; }
        public int[] triangles { get; set; }
    }
}