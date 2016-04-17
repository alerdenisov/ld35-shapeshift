using UnityEngine;

namespace ShapeTetris.Behaviours.Rendering
{
    public class ThreadableMesh
    {
        public Vector3[] Vertices;
        public int[] Triangles;


        // User-defined conversion from Digit to double
        public static implicit operator Mesh(ThreadableMesh tm)
        {
            var mesh = new Mesh();
            mesh.vertices = tm.Vertices;
            mesh.triangles = tm.Triangles;

            return mesh;
        }
    }
}