using System;
using ShapeTetris.Models;
using UniRx;
using UnityEngine;

namespace ShapeTetris.Behaviours.Rendering
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public class VoxelVolume : MonoBehaviour
    {
        #region Dependencies

        private MeshFilter _filter;
        private MeshRenderer _renderer;

        protected MeshFilter Filter
        {
            get
            {
                if (!_filter) _filter = GetComponent<MeshFilter>();
                return _filter;
            }
        }

        protected MeshRenderer Renderer
        {
            get
            {
                if (!_renderer) _renderer = GetComponent<MeshRenderer>();
                return _renderer;
            }
        }

        #endregion

        private VoxelData _data;

        public VoxelData Data
        {
            get { return _data; }
            set { SetNewData(value); }
        }

        private void SetNewData(VoxelData value)
        {
            _data = value;
            _data.Stream.Subscribe(OnNewData);
        }

        private void OnNewData(Voxel[] voxels)
        {
//            ChangeMesh(BuildVoxels(Data));
            Observable.Start(() => BuildVoxels(Data)).ObserveOnMainThread().Subscribe(ChangeMesh);
        }

        private void ChangeMesh(ThreadableMesh mesh)
        {
            Mesh unityMesh = mesh;
            unityMesh.RecalculateNormals();
            Filter.mesh = unityMesh;
        }

        private ThreadableMesh BuildVoxels(VoxelData voxels)
        {
            return VoxelBuilder.Build(voxels);
        }
    }
}