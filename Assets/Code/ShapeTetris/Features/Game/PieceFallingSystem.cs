using System.Collections.Generic;
using Entitas;
using ShapeTetris.Models;
using UnityEditor;
using UnityEngine;

namespace ShapeTetris.Features.Game
{
    public class PieceFallingSystem : IExecuteSystem, ISetPool
    {
        private Group _pieces;

        public void Execute()
        {
            foreach (var entity in _pieces.GetEntities())
            {
                Fall(entity);
            }
        }

        private void Fall(Entity entity)
        {
            var position = entity.position.Point;
            position.y -= 4f*Time.deltaTime;
            entity.ReplacePosition(position);

            var bottomDots = entity.pieceBottomDots.Bottoms;

            var leveldata = Pools.game.levelEntity.levelData;
            var w = Pools.game.level.Width;
            foreach (var dot in bottomDots)
            {
                var y = dot.Position.y + (int)entity.position.Point.y;
                var x = dot.Position.x + (int)entity.position.Point.x;

                if (leveldata.Dots[y*w + x] != null)
                {
                    Debug.Log("COLLIDE!!");
                    entity.RemovePieceBottomDots();
                }
            }
        }

        private void DrawCollider()
        {
        }

        public void SetPool(Pool pool)
        {
            _pieces = pool.GetGroup(Matcher.AllOf(GameMatcher.Piece, GameMatcher.PieceBottomDots, GameMatcher.Position));
        }
    }
}