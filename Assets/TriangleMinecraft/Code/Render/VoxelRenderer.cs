using System;
using System.Collections.Generic;
using System.Threading;
using TriangleMinecraft.Render.Helpers;
using UnityEngine;

namespace TriangleMinecraft.Render
{
    public class VoxelRenderer {

        public Vector3[][] CachedVertices;

        public VoxelRenderer()
        {
            PremakeVertices();
        }

        private void PremakeVertices()
        {
            CachedVertices = new Vector3[8][];
            CachedVertices[0] = VerticesHelper.Make(VoxelPoints.PointAA);
            CachedVertices[1] = VerticesHelper.Make(VoxelPoints.PointAB);
            CachedVertices[2] = VerticesHelper.Make(VoxelPoints.PointBA);
            CachedVertices[3] = VerticesHelper.Make(VoxelPoints.PointBB);

            // rotated
            CachedVertices[4] = VerticesHelper.Make(VoxelPoints.PointAA, true);
            CachedVertices[5] = VerticesHelper.Make(VoxelPoints.PointAB, true);
            CachedVertices[6] = VerticesHelper.Make(VoxelPoints.PointBA, true);
            CachedVertices[7] = VerticesHelper.Make(VoxelPoints.PointBB, true);
        }


        /// <summary>
        /// TODO: Add world reference to render 
        /// </summary>
        public ThreadableMesh Render()
        {
            var mesh = new ThreadableMesh();
            var tmpMap = new VoxelMap();
            List<int> triangles = new List<int>();
            List<Vector3> vertices = new List<Vector3>();
            var index = 0;
            for (int y = 0; y < 1; y++)
            {
                for (int z = 0; z < tmpMap.Dimensions; z++)
                {
                    for (int x = 0; x < tmpMap.Dimensions; x++)
                    {
                        var voxel = tmpMap.GetVoxel(x, y, z);
                        vertices.AddRange(MakeVertices(voxel, x, y, z));
                        triangles.AddRange(MakeTriangles(ref index, voxel));
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            return mesh;
        }

        private int[] MakeTriangles(ref int index, Voxel voxel)
        {
            List<int> triangles = new List<int>();
            triangles.AddRange(RenderPointTriangles(ref index, voxel.PointAA, VoxelPoints.PointAA));
            triangles.AddRange(RenderPointTriangles(ref index, voxel.PointAB, VoxelPoints.PointAB));
            triangles.AddRange(RenderPointTriangles(ref index, voxel.PointBA, VoxelPoints.PointBA));
            triangles.AddRange(RenderPointTriangles(ref index, voxel.PointBB, VoxelPoints.PointBB));

            return triangles.ToArray();
        }
        private Vector3[] MakeVertices(Voxel voxel, int x, int y, int z)
        {
            List<Vector3> vertices = new List<Vector3>();
            vertices.AddRange(RenderPointVertices(voxel.PointAA, VoxelPoints.PointAA, x, y, z));
            vertices.AddRange(RenderPointVertices(voxel.PointAB, VoxelPoints.PointAB, x, y, z));
            vertices.AddRange(RenderPointVertices(voxel.PointBA, VoxelPoints.PointBA, x, y, z));
            vertices.AddRange(RenderPointVertices(voxel.PointBB, VoxelPoints.PointBB, x, y, z));

            return vertices.ToArray();
        }

        private Vector3[] RenderPointVertices(byte point, VoxelPoints type, int x, int y, int z)
        {
            if (point == 0) return new Vector3[0];
            Vector3[] vertices = new Vector3[12];
            var odd = (x + z)%2 != 0;
            Array.Copy(CachedVertices[(int)type + (odd ? 4 : 0)], vertices, 12);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].x += x * 2;
                vertices[i].y += y * 2;
                vertices[i].z -= z * 2;
            }

            return vertices;
        }

        private int[] RenderPointTriangles(ref int i, byte point, VoxelPoints type)
        {
            if(point == 0) return new int[0];
            var r = new int[12];
            for (int index = 0; index < r.Length; index++)
            {
                r[index] = i + index;
            }

            i += 12;

            return r;
        }


    }
}
