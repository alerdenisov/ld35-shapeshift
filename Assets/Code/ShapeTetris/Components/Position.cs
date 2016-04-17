using Entitas;
using UnityEngine;

namespace ShapeTetris.Components
{
    [Game]
    public class Position : IComponent
    {
        public Vector3 Point;
    }
}