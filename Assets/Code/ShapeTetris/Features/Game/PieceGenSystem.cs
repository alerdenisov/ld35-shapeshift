using System;
using System.Collections;
using System.Collections.Generic;
using Entitas;
using ShapeTetris.Behaviours;
using ShapeTetris.Behaviours.Rendering;
using ShapeTetris.Components;
using ShapeTetris.Models;
using UniRx;
using UnityEngine;

namespace ShapeTetris.Features.Game
{
    public class PieceGenSystem : IReactiveSystem
    {
        private Queue<PieceWrapperBehaviour> _availableRenderers;
        private Dictionary<int, PieceWrapperBehaviour> _activeVolumes;

        public PieceGenSystem()
        {
            _activeVolumes = new Dictionary<int, PieceWrapperBehaviour>();
            _availableRenderers = new Queue<PieceWrapperBehaviour>();

            foreach (var vol in GameObject.FindGameObjectsWithTag("PieceVolume"))
            {
                var volume = vol.GetComponent<VoxelVolume>();
                var wrapper = new GameObject("Piece Wrapper", typeof(PieceWrapperBehaviour));
                vol.transform.parent = wrapper.transform;
                vol.transform.localPosition = -Vector3.one*8f;

                // setup wrapper
                var pieceWrapper = wrapper.GetComponent<PieceWrapperBehaviour>();
                pieceWrapper.Volume = volume;

                _availableRenderers.Enqueue(pieceWrapper);
            }
        }

        public void Execute(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if(entity.hasPiece)
//                    Observable.Start(()=> UpdatePiece(entity)).Subscribe();
                    UpdatePiece(entity);
                else
                {
                    var wrapper = _activeVolumes[entity.creationIndex];
                    _activeVolumes.Add(entity.creationIndex, wrapper);
                }
            }
            
        }

        private void UpdatePiece(Entity piece)
        {
            var wrapper = GetPieceWrapper(piece.creationIndex);
            piece.ReplaceInScene(wrapper.gameObject);

            var renderer = wrapper.Volume;
            var b = piece.pieceBounds;
            var data = new VoxelData(16,16,16);
            var red = new Color32(255, 55, 55, 255);

            data.Clear();

            for (int i = 0; i < piece.piece.Dots.Length; i++)
            {
                var d = piece.piece.Dots[i];
                var npos = d.Position + Vector3i.one * 8;
//                var npos = d.Position + b.Offset + Vector3i.one * 8;
                data.SetVoxel(npos.x, npos.y, npos.z, red);
            }

            renderer.Data = data;
            renderer.Data.CommitChanges();

            var offset = new Vector3(-b.Offset.x, -b.Offset.y, -b.Offset.z);
            var colSize = new Vector3(b.Size.x + 1, b.Size.y + 1, b.Size.z + 1);

            var n1 = colSize - Vector3.one;
            var n2 = n1*0.5f;
            var n3 = n2 + offset;

            piece.ReplaceRotation(Vector3.zero);

//            wrapper.UpdateCollision(colSize, n3);
        }

        private PieceWrapperBehaviour GetPieceWrapper(int index)
        {
            if (_activeVolumes.ContainsKey(index))
                return _activeVolumes[index];

            if (_availableRenderers.Count > 0)
            {
                var render = _availableRenderers.Dequeue();
                _activeVolumes.Add(index, render);
                return render;
            }
            else
            {
                throw new NullReferenceException("!");
            }
        }


        public TriggerOnEvent trigger
        {
            get
            {
                return new TriggerOnEvent(Matcher.AllOf(GameMatcher.Piece, GameMatcher.PieceBounds), GroupEventType.OnEntityAddedOrRemoved);
            }
        }
    }
}