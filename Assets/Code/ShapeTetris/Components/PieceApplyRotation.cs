using Entitas;
using ShapeTetris.Enums;

namespace ShapeTetris.Components
{
    [Game]
    public class PieceApplyRotation : IComponent
    {
        public RotateDirection Direction;
    }
}