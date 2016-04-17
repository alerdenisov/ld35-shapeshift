using ShapeTetris.Enums;
using ShapeTetris.Models;

namespace ShapeTetris.Levels
{
    public class LevelGenerator
    {
        private int _width;
        private int _height;
        private int _depth;

        public LevelGenerator()
        {
            _width = 15;
            _height = 20;
            _depth = 25;
        }

        public LevelGenerator(int width, int height, int depth)
        {
            _width = width;
            _height = height;
            _depth = depth;
        }

        public Level Generate()
        {
            var data = new Level(_width, _height, _depth); 

            for (int y = 0; y < _height; y++)
            {
                data.SetDot(0, 0, y, Dots.Border);
                data.SetDot(_width - 1, 0, y, Dots.Border);
            }

            for (int x = 0; x < _width; x++)
            {
                data.SetDot(x, 0, 0, Dots.Border);
            }

            return data;
        }
    }
}