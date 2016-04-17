using Entitas;
using UnityEngine;

namespace ShapeTetris.Components
{
    [Game]
    public class Rotation : IComponent
    {
        public Vector3 LocalEuler;
    }
}