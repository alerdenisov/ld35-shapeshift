using Entitas;
using Entitas.CodeGenerator;
using ShapeTetris.Models;

namespace ShapeTetris.Components
{
    [Game]
    public class LevelData : IComponent
    {
        public Dot[] Dots;
    }
}