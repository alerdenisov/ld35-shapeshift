using Entitas;
using Entitas.CodeGenerator;

namespace ShapeTetris.Components
{
    [Game]
    [SingleEntity]
    public class Level : IComponent
    {
        public int Width;
        public int Height;
    }
}