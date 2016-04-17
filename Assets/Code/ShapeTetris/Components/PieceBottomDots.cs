using Entitas;
using ShapeTetris.Models;

namespace ShapeTetris.Components
{
    [Game]
    public class PieceBottomDots : IComponent
    {
        public PieceDot[] Bottoms;
    }
}