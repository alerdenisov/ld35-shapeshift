using System;
using System.Collections.Generic;
using Extensions;
using ShapeTetris.Models;
using UnityEngine;

namespace ShapeTetris.Behaviours.Rendering
{
    public enum CullingFaces : byte
    {
        None = 0,

        East            = 1 << 0, // 1
        West            = 1 << 1, // 2
        North           = 1 << 2, // 4
        South           = 1 << 3, // 8
        Up              = 1 << 4, // 16
        Down            = 1 << 5, // 32

        All = East | West | North | South | Up | Down
    }

    public static class VoxelBuilder
    {
        public static ThreadableMesh Build(VoxelData voxels)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            int triIndex = 0;
            for (int y = 0; y < voxels.Height; y++)
            {
                for (int z = 0; z < voxels.Depth; z++)
                {
                    for (int x = 0; x < voxels.Width; x++)
                    {
                        var voxel = voxels.GetVoxel(x, y, z);
                        if (voxel.Color.a == 0)
                            continue;

                        // build
                        var cullingFaces = CalcCullingFaces(voxels, x, y, z);

                        if(cullingFaces == CullingFaces.All)
                            continue;

                        vertices.AddRange(MakeVertices(cullingFaces, x, y, z));
                        triangles.AddRange(MakeTriangles(cullingFaces, ref triIndex));
                    }
                }
            }

            var mesh = new ThreadableMesh();
            mesh.Vertices = vertices.ToArray();
            mesh.Triangles = triangles.ToArray();

            return mesh;
        }

        private static int[] MakeTriangles(CullingFaces face, ref int start)
        {
            var count = 6 * (6 - ((byte)face).PopCount());
            var triangles = new int[count];
            var index = 0;
            foreach (var f in Enum.GetValues(typeof(CullingFaces)))
            {
                if (((byte)(CullingFaces)f).PopCount() != 1) continue;
                if (!face.HasFlag((CullingFaces)f))
                {
                    Array.Copy(MakeFaceTriangles((CullingFaces)f, start), 0, triangles, index, 6);
                    index += 6;
                    start += 4;
                }
            }

            return triangles;
        }

        private static int[] MakeFaceTriangles(CullingFaces face, int i)
        {
            return new int[]
            {
                i + 0,
                i + 1,
                i + 2,
                i + 3,
                i + 2,
                i + 1
            };
        }

        private static Vector3[] MakeVertices(CullingFaces cullingFaces, int x, int y, int z)
        {
            var count = 4*(6 - ((byte) cullingFaces).PopCount());

            var vertices = new Vector3[count];
            var index = 0;

            foreach (var face in Enum.GetValues(typeof(CullingFaces)))
            {
                if (((byte)(CullingFaces)face).PopCount() != 1) continue;
                if (!cullingFaces.HasFlag((CullingFaces) face))
                {
                    Array.Copy(
                        MakeFaceVerts((CullingFaces) face, x, y, z),
                        0,
                        vertices,
                        index,
                        4);
                    index += 4;
                }

            }

            return vertices;
        }

        private static Vector3[] MakeFaceVerts(CullingFaces face, int x, int y, int z)
        {
            var o = new Vector3(x,y,z);
            var q = GetRotation(face);
            var r = new []
            {
                q*new Vector3(-.5f,  .5f, -.5f) + o,
                q*new Vector3( .5f,  .5f, -.5f) + o,
                q*new Vector3(-.5f, -.5f, -.5f) + o,
                q*new Vector3( .5f, -.5f, -.5f) + o,
            };
            return r;
        }

        private static Quaternion GetRotation(CullingFaces face)
        {
            switch (face)
            {
                case CullingFaces.West:
                    return Quaternion.Euler(0, 90, 0);
                case CullingFaces.North:
                    return Quaternion.Euler(0, 180, 0);
                case CullingFaces.East:
                    return Quaternion.Euler(0, 270, 0);
                case CullingFaces.Up:
                    return Quaternion.Euler(90, 0, 0);
                case CullingFaces.Down:
                    return Quaternion.Euler(-90, 0, 0);
            }

            return Quaternion.identity;
        }

        private static CullingFaces CalcCullingFaces(VoxelData voxels, int x, int y, int z)
        {
            var r = CullingFaces.All;
            if (!voxels.GetVoxel(x + 1, y, z).IsSolid)
                r -= CullingFaces.East;
            if (!voxels.GetVoxel(x - 1, y, z).IsSolid)
                r -= CullingFaces.West;
            if (!voxels.GetVoxel(x, y, z - 1).IsSolid)
                r -= CullingFaces.South;
            if (!voxels.GetVoxel(x, y, z + 1).IsSolid)
                r -= CullingFaces.North;
            if (!voxels.GetVoxel(x, y - 1, z).IsSolid)
                r -= CullingFaces.Down;
            if (!voxels.GetVoxel(x, y + 1, z).IsSolid)
                r -= CullingFaces.Up;

            return r;
        }
    }
}