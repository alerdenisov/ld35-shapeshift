using Entitas;
using ShapeTetris.Enums;
using ShapeTetris.Models;
using UnityEngine;

namespace ShapeTetris.Components
{
    [Game]
    public class Piece : IComponent
    {
        public PieceDot[] Dots { get; set; }

    }
}