using Entitas;
using ShapeTetris.Enums;

namespace ShapeTetris.Components
{
    [Game]
    public class PieceRotation : IComponent
    {
        public RotateDirection Direction;
        public float Time { get; set; }

        public void SetTime(float time)
        {
            Time = time;
        }

    }
}