using System.Collections.Generic;
using Entitas;
using ShapeTetris.Enums;
using UnityEngine;

namespace ShapeTetris.Features.Game
{
    public class PieceRotationSystem : IExecuteSystem, ISetPool
    {
        private Group _rotationPieces;
        public void Execute()
        {
            foreach (var entity in _rotationPieces.GetEntities())
            {
                if (entity.pieceRotation.Time < 1f)
                {
                    var time = Mathf.Clamp01(entity.pieceRotation.Time += Time.deltaTime*4f);
                    entity.ReplacePieceRotation(
                        entity.pieceRotation.Direction,
                        time);

                    var angle = Vector3.zero;

                    switch (entity.pieceRotation.Direction)
                    {
                        case RotateDirection.HorizontalNegative:
                            angle = new Vector3(0, 90, 0);
                            break;
                        case RotateDirection.HorizontalPositive:
                            angle = new Vector3(0, -90, 0);
                            break;
                        case RotateDirection.VerticalPositive:
                            angle = new Vector3(90, 0, 0);
                            break;
                        case RotateDirection.VerticalNegative:
                            angle = new Vector3(-90, 0, 0);
                            break;
                    }

                    entity.ReplaceRotation(angle*entity.pieceRotation.Time);
                }
                else
                {
                    entity.ReplacePieceApplyRotation(entity.pieceRotation.Direction);
                    entity.RemovePieceRotation();
                }
            }
        }

        public void SetPool(Pool pool)
        {
            _rotationPieces = pool.GetGroup(GameMatcher.PieceRotation);
        }
    }
}