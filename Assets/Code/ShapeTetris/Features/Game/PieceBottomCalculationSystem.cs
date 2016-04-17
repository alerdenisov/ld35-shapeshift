using System.Collections.Generic;
using Entitas;
using ShapeTetris.Models;

namespace ShapeTetris.Features.Game
{
    public class PieceBottomCalculationSystem : IReactiveSystem
    {
        public void Execute(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                CalcFor(entity);
            }
        }

        private void CalcFor(Entity entity)
        {
            var bottomDots = new List<PieceDot>();
            foreach (var pieceDot in entity.piece.Dots)
            {
                var add = true;
                foreach (var bottomDot in bottomDots)
                {
                    if (bottomDot.Position.x != pieceDot.Position.x || bottomDot.Position.y >= pieceDot.Position.y)
                        continue;

                    add = false;
                    break;
                }

                if (add)
                    bottomDots.Add(pieceDot);
            }

            entity.ReplacePieceBottomDots(bottomDots.ToArray());
        }

        public TriggerOnEvent trigger
        {
            get
            {
                return new TriggerOnEvent(GameMatcher.Piece, GroupEventType.OnEntityAdded);
            }
        }
    }
}