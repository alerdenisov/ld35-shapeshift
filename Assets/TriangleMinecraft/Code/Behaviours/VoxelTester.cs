using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TriangleMinecraft.Render;
using UniRx;
using UnityEngine;

namespace TriangleMinecraft.Behaviours
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class VoxelTester : MonoBehaviour
    {
        public bool UpdateNow;

        private VoxelRenderer _renderer;

        private MeshFilter _filter;
        private MeshRenderer _meshRenderer;

        protected MeshFilter Filter
        {
            get
            {
                if (!_filter) _filter = gameObject.GetComponent<MeshFilter>();
                return _filter;
            }
        }


        IEnumerator ContinuesUpdate(IObserver<ThreadableMesh> observer, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (UpdateNow)
                {
                    observer.OnNext(_renderer.Render());
                }

                yield return Observable.Timer(TimeSpan.FromSeconds(0.1f)).ToYieldInstruction();
            }

            observer.OnCompleted();
        }

        IObservable<ThreadableMesh> GetObservableMesh()
        {
            return Observable.FromCoroutine<ThreadableMesh>(ContinuesUpdate);
        }

        void Start()
        {
            _renderer = new VoxelRenderer();
            GetObservableMesh().ObserveOnMainThread().Subscribe(OnRenderer);
        }

        private void OnRenderer(ThreadableMesh raw)
        {
            DebugGrid(raw.World);
            UpdateNow = false;
            var mesh = new Mesh();
            mesh.vertices = raw.vertices;
            mesh.triangles = raw.triangles;
            mesh.RecalculateNormals();
            Filter.mesh = mesh;

        }

        private void DebugGrid(VoxelMap world)
        {
            var px = 2;
            var pz = 2;
            for (int y = 0; y < world.Height; y++)
            {
                for (int z = 0; z < 10; z++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        var voxel = world.GetVoxel(px + x, y, pz + z);
                        if (voxel.PointAA > 0 || voxel.PointAB > 0 || voxel.PointBA > 0 || voxel.PointBB > 0)
                        {

                            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            go.name = string.Format("Debug {0}, {1}, {2}", px + x, y, pz + z);
                            go.transform.parent = transform;
                            var collider = go.GetComponent<BoxCollider>();
                            var voxeldata = go.AddComponent<VoxelDataBehaviour>();

                            var mr = go.GetComponent<MeshRenderer>();
                            mr.material = new Material(Shader.Find("Legacy Shaders/Transparent/Diffuse"));
                            mr.material.color = new Color(0f,1f,0f,0.25f);
                            mr.enabled = false;

                            voxeldata.VoxelData = voxel;
                            collider.isTrigger = true;

                            go.transform.localPosition = Vector3.right*((px + x)*2) + Vector3.back*((pz + z)*2) +
                                                         Vector3.up*y*2;

                            go.transform.localScale = Vector3.one*2.05f;

//                            collider.size = Vector3.one*2f;
                        }
                    }
                }
            }
        }
    }
}