using System.Collections.Generic;
using Entitas;
using ShapeTetris.Models;
using UnityEngine;

namespace ShapeTetris.Features.Game
{
    public class PieceBoundsCalculationSystem : IReactiveSystem
    {
        public void Execute(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                MakeFor(entity);
            }
        }

        private void MakeFor(Entity e)
        {
            var piece = e.piece;

            Vector3i aa = new Vector3i();
            Vector3i bb = new Vector3i();

            foreach (var dot in piece.Dots)
            {
                if (dot.Position.x < aa.x)
                    aa.x = dot.Position.x;
                if (dot.Position.y < aa.y)
                    aa.y = dot.Position.y;
                if (dot.Position.z < aa.z)
                    aa.z = dot.Position.z;

                if (dot.Position.x > bb.x)
                    bb.x = dot.Position.x;
                if (dot.Position.y > bb.y)
                    bb.y = dot.Position.y;
                if (dot.Position.z > bb.z)
                    bb.z = dot.Position.z;
            }

            var size = new Vector3i(
                (int)Mathf.Abs(bb.x - aa.x), 
                (int)Mathf.Abs(bb.y - aa.y), 
                (int)Mathf.Abs(bb.z - aa.z));


            e.ReplacePieceBounds(size, aa * -1);
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