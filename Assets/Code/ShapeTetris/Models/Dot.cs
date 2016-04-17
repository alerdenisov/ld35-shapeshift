using ShapeTetris.Enums;

namespace ShapeTetris.Models
{
    public class Dot
    {
        private Dots _type;

        public Dots Type { get { return _type; } }

        public Dot(Dots type)
        {
            _type = type;
        }

        public static Dot Create(Dots type)
        {
            return new Dot(type);
        }
    }
}