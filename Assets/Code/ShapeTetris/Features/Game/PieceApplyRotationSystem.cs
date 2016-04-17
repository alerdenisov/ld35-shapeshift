using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Entitas;
using ShapeTetris.Enums;
using ShapeTetris.Models;
using UnityEngine;

namespace ShapeTetris.Features.Game
{
    public class PieceApplyRotationSystem : IReactiveSystem
    {
        public void Execute(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                ApplyRotation(entity);
            }
        }

        private void ApplyRotation(Entity entity)
        {
            var piece = entity.piece;
            var direction = entity.pieceApplyRotation.Direction;

            entity.RemovePieceApplyRotation();

            var rotatedDots = new PieceDot[piece.Dots.Length];
            var rotation = GetMatrix(direction);
            for (int i = 0; i < rotatedDots.Length; i++)
            {
                var p1 = piece.Dots[i].Position;
                var p2 = rotation*(Vector3)p1;

                int x = Mathf.RoundToInt(p2.x);
                int y = Mathf.RoundToInt(p2.y);
                int z = Mathf.RoundToInt(p2.z);

                rotatedDots[i] = new PieceDot(new Vector3i(x,y,z));
                rotatedDots[i].Type = piece.Dots[i].Type;
            }

            entity.ReplacePiece(rotatedDots);
        }

        private Quaternion GetMatrix(RotateDirection direction)
        {

            var angle = Vector3.zero;

            switch (direction)
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

            return Quaternion.Euler(angle);//Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(angle), Vector3.one));
        }

        public TriggerOnEvent trigger
        {
            get
            {
                return new TriggerOnEvent(Matcher.AllOf(GameMatcher.PieceApplyRotation, GameMatcher.Piece), GroupEventType.OnEntityAdded);
            }
        }
    }
}