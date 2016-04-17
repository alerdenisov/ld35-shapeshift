using UnityEngine;

namespace ShapeTetris.Models
{
    public struct Voxel
    {
        public Color32 Color { get; private set; }

        public bool IsSolid
        {
            get { return Color.a > 0; }
        }

        public Voxel(Color32 color) : this()
        {
            Color = color;
        }
    }
}