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
            GetObservableMesh().Subscribe(OnRenderer);
        }

        private void OnRenderer(ThreadableMesh raw)
        {
            UpdateNow = false;
            var mesh = new Mesh();
            mesh.vertices = raw.vertices;
            mesh.triangles = raw.triangles;
            mesh.RecalculateNormals();
            Filter.mesh = mesh;

        }
    }
}