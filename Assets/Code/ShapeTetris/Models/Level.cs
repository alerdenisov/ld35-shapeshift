using ShapeTetris.Enums;

namespace ShapeTetris.Models
{
    public class Level
    {
        public Dot[] Dots { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Depth { get; private set; }

        public Level(int width, int height, int depth)
        {
            Width = width;
            Height = height;
            Depth = depth;

            Dots = new Dot[width * height * depth];
        }


        public void SetDot(int x, int y, int z, Dots type)
        {
            Dots[y*Width*Depth + z*Width + x] = Dot.Create(type);
        }
    }
}