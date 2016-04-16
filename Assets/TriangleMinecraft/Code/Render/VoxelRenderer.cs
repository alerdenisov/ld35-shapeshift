using System;
using System.Collections.Generic;
using System.Threading;
using Extensions;
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
            mesh.World = tmpMap;
            List<int> triangles = new List<int>();
            List<Vector3> vertices = new List<Vector3>();
            var index = 0;
            var px = 2;
            var pz = 2;
            for (int y = 0; y < tmpMap.Height; y++)
            {
                for (int z = 0; z < 10; z++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        var voxel = tmpMap.GetVoxel(px + x, y, pz + z);
                        var neighbors = tmpMap.GetNeighbors(px + x, y, pz + z);
                        if (x == 0)
                            neighbors[(int)VoxelNeibor.West] = default(Voxel);
                        if (x == 9)
                            neighbors[(int)VoxelNeibor.East] = default(Voxel);
                        if (z == 0)
                            neighbors[(int)VoxelNeibor.North] = default(Voxel);
                        if (z == 9)
                            neighbors[(int)VoxelNeibor.South] = default(Voxel);

                        vertices.AddRange(MakeVertices(voxel, neighbors, px + x, y, pz + z));
                        triangles.AddRange(MakeTriangles(ref index, voxel, neighbors));
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            return mesh;
        }

        private int[] MakeTriangles(ref int index, Voxel voxel, Voxel[] neibors)
        {
            List<int> triangles = new List<int>();
            triangles.AddRange(RenderPointTriangles(ref index, voxel.PointAA, Faces(voxel, neibors, VoxelPoints.PointAA), VoxelPoints.PointAA));
            triangles.AddRange(RenderPointTriangles(ref index, voxel.PointAB, Faces(voxel, neibors, VoxelPoints.PointAB), VoxelPoints.PointAB));
            triangles.AddRange(RenderPointTriangles(ref index, voxel.PointBA, Faces(voxel, neibors, VoxelPoints.PointBA), VoxelPoints.PointBA));
            triangles.AddRange(RenderPointTriangles(ref index, voxel.PointBB, Faces(voxel, neibors, VoxelPoints.PointBB), VoxelPoints.PointBB));

            return triangles.ToArray();
        }
        private Vector3[] MakeVertices(Voxel voxel, Voxel[] neibors, int x, int y, int z)
        {
            List<Vector3> vertices = new List<Vector3>();
            vertices.AddRange(RenderPointVertices(voxel.PointAA, Faces(voxel, neibors, VoxelPoints.PointAA), VoxelPoints.PointAA, x, y, z));
            vertices.AddRange(RenderPointVertices(voxel.PointAB, Faces(voxel, neibors, VoxelPoints.PointAB), VoxelPoints.PointAB, x, y, z));
            vertices.AddRange(RenderPointVertices(voxel.PointBA, Faces(voxel, neibors, VoxelPoints.PointBA), VoxelPoints.PointBA, x, y, z));
            vertices.AddRange(RenderPointVertices(voxel.PointBB, Faces(voxel, neibors, VoxelPoints.PointBB), VoxelPoints.PointBB, x, y, z));

            return vertices.ToArray();
        }

        private FaceCulling Faces(Voxel voxel, Voxel[] neibors, VoxelPoints type)
        {
            FaceCulling culling = FaceCulling.None;

            switch (type)
            {
                case VoxelPoints.PointAA:
                    culling = FaceCulling.All;
                    if (voxel.PointAA == 0 || voxel.PointBB == 0 || voxel.PointAB == 0 || voxel.PointBA == 0)
                        culling -= FaceCulling.Face;
                    if(neibors[(int)VoxelNeibor.North].PointBA == 0 || neibors[(int)VoxelNeibor.North].PointBB == 0)
                        culling -= FaceCulling.Left;
                    if (neibors[(int) VoxelNeibor.West].PointAB == 0 || neibors[(int) VoxelNeibor.West].PointBB == 0)
                        culling -= FaceCulling.Right;
                    if(neibors[(int) VoxelNeibor.Down].PointBA == 0 || neibors[(int) VoxelNeibor.Down].PointAB == 0)
                        culling -= FaceCulling.Foot;
//                    culling = FaceCulling.None;
                    break;
                case VoxelPoints.PointAB:
                    culling = FaceCulling.All;
                    if (voxel.PointAA == 0 || voxel.PointBB == 0 || voxel.PointAB == 0 || voxel.PointBA == 0)
                        culling -= FaceCulling.Face;
                    if (neibors[(int)VoxelNeibor.East].PointBA == 0 || neibors[(int)VoxelNeibor.East].PointAA == 0)
                        culling -= FaceCulling.Right;
                    if (neibors[(int)VoxelNeibor.North].PointBA == 0 || neibors[(int)VoxelNeibor.North].PointBB == 0)
                        culling -= FaceCulling.Left;
                    if (neibors[(int)VoxelNeibor.Top].PointAA == 0 || neibors[(int)VoxelNeibor.Top].PointBB == 0)
                        culling -= FaceCulling.Foot;
//                    culling = FaceCulling.None;
                    break;
                case VoxelPoints.PointBA:
                    culling = FaceCulling.All;
                    if (voxel.PointAA == 0 || voxel.PointBB == 0 || voxel.PointAB == 0 || voxel.PointBA == 0)
                        culling -= FaceCulling.Face;
                    if (neibors[(int)VoxelNeibor.West].PointAB == 0 || neibors[(int)VoxelNeibor.West].PointBB == 0)
                        culling -= FaceCulling.Right;
                    if (neibors[(int)VoxelNeibor.South].PointAA == 0 || neibors[(int)VoxelNeibor.South].PointAB == 0)
                        culling -= FaceCulling.Left;
                    if (neibors[(int)VoxelNeibor.Top].PointAA == 0 || neibors[(int)VoxelNeibor.Top].PointBB == 0)
                        culling -= FaceCulling.Foot;
//                    culling = FaceCulling.None;
                    break;
                case VoxelPoints.PointBB:
                    culling = FaceCulling.All;
                    if (voxel.PointAA == 0 || voxel.PointBB == 0 || voxel.PointAB == 0 || voxel.PointBA == 0)
                        culling -= FaceCulling.Face;
                    if (neibors[(int)VoxelNeibor.East].PointBA == 0 || neibors[(int)VoxelNeibor.East].PointAA == 0)
                        culling -= FaceCulling.Right;
                    if (neibors[(int)VoxelNeibor.South].PointAA == 0 || neibors[(int)VoxelNeibor.South].PointAB == 0)
                        culling -= FaceCulling.Left;
                    if (neibors[(int)VoxelNeibor.Down].PointAB == 0 || neibors[(int)VoxelNeibor.Down].PointBA == 0)
                        culling -= FaceCulling.Foot;
//                    culling = FaceCulling.None;

                    break;
            }

            return culling;
        }

        private Vector3[] RenderPointVertices(byte point, FaceCulling culling, VoxelPoints type, int x, int y, int z)
        {
            if (point == 0) return new Vector3[0];
            Vector3[] vertices = new Vector3[12];
            var odd = (x + (y-1) + z)%2 != 0;
            odd = false;
            // destination index (increment after Array Copy)
            var di = 0;
            if (!culling.HasFlag(FaceCulling.Foot))
            {
                Array.Copy(CachedVertices[(int) type + (odd ? 4 : 0)], 0, vertices, di, 3);
                di += 3;
            }

            if (!culling.HasFlag(FaceCulling.Face))
            {
                Array.Copy(CachedVertices[(int) type + (odd ? 4 : 0)], 3, vertices, di, 3);
                di += 3;
            }

            if (!culling.HasFlag(FaceCulling.Right))
            {
                Array.Copy(CachedVertices[(int) type + (odd ? 4 : 0)], 6, vertices, di, 3);
                di += 3;
            }

            if (!culling.HasFlag(FaceCulling.Left))
            {
                Array.Copy(CachedVertices[(int) type + (odd ? 4 : 0)], 9, vertices, di, 3);
                di += 3;
            }

            var final = new Vector3[di];
            for (int i = 0; i < final.Length; i++)
            {
                final[i].x = vertices[i].x + x * 2;
                final[i].y = vertices[i].y + y * 2;
                final[i].z = vertices[i].z - z * 2;
            }

            return final;
        }

        private int[] RenderPointTriangles(ref int i, byte point, FaceCulling culling, VoxelPoints type)
        {
            if(point == 0) return new int[0];
            var r = new int[12];
            var index = 0;
            if (!culling.HasFlag(FaceCulling.Foot))
            {
                r[index++] = i++; // FOOT
                r[index++] = i++;
                r[index++] = i++;
            }
            if (!culling.HasFlag(FaceCulling.Face))
            {
                r[index++] = i++; // Face
                r[index++] = i++;
                r[index++] = i++;
            }
            if (!culling.HasFlag(FaceCulling.Right))
            {
                r[index++] = i++; // Right
                r[index++] = i++;
                r[index++] = i++;
            }

            if (!culling.HasFlag(FaceCulling.Left))
            {
                r[index++] = i++; // Left
                r[index++] = i++;
                r[index++] = i++;
            }

            var final = new int[index];
            for (int j = 0; j < final.Length; j++)
                final[j] = r[j];

            return final;
        }


    }
}
