using ShapeTetris.Enums;
using UnityEngine;

namespace ShapeTetris.Models
{
    public class PieceDot
    {
        public Vector3i Position { get; set; }
        public Dots Type;

        public PieceDot(Vector3i position)
        {
            Position = position;
        }
    }
}