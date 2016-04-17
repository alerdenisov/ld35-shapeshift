using System;
using ShapeTetris.Models;
using UniRx;
using UnityEngine;

namespace ShapeTetris.Behaviours.Rendering
{
    public class VoxelData
    {
        public Voxel[] Voxels { get; private set; }
        public int Width { get; private set; }
        public int Depth { get; private set; }
        public int Height { get; private set; }

        private Subject<Voxel[]> _stream;

        public IObservable<Voxel[]> Stream
        {
            get { return _stream; }
            private set { _stream = value as Subject<Voxel[]>; }
        }

        public VoxelData(int width, int height, int depth)
        {
            Scale(width, height, depth);
            Stream = new Subject<Voxel[]>();
        }

        public void Scale(int width, int height, int depth)
        {
            Width = width;
            Depth = depth;
            Height = height;

            Voxels = new Voxel[Height * Depth * Width];
        }

        public Voxel GetVoxel(int x, int y, int z)
        {
            var index = GetIndex(x, y, z);
            return index != -1 ? Voxels[index] : default(Voxel);
        }

        public void SetVoxel(int x, int y, int z, Color32 color)
        {
            Voxels[GetIndex(x, y, z)] = new Voxel(color);
        }

        private int GetIndex(int x, int y, int z)
        {
            if (x < 0 || x >= Width)
                return -1;
//                throw new ArgumentOutOfRangeException("x", String.Format("X ({0}) is less than 0 or more than width {1}", x, Width));
            if (y < 0 || y >= Height)
                return -1;
//                throw new ArgumentOutOfRangeException("y", String.Format("Y ({0}) is less than 0 or more than height {1}", y, Height));
            if (z < 0 || z >= Depth)
                return -1;
//                throw new ArgumentOutOfRangeException("z", String.Format("Z ({0}) is less than 0 or more than depth {1}", z, Depth));
            return y*Depth*Width + z*Width + x;
        }

        public void CommitChanges()
        {
            _stream.OnNext(Voxels);
        }

        public void Clear()
        {
            for (int i = 0; i < Voxels.Length; i++)
            {
                Voxels[i] = default(Voxel);
            }
        }
    }
}