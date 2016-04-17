using Entitas;
using ShapeTetris.Models;

namespace ShapeTetris.Components
{
    [Game]
    public class PieceBounds : IComponent
    {
        public Vector3i Size;
        public Vector3i Offset;
    }
}