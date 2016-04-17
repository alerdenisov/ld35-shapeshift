using ShapeTetris.Behaviours.Rendering;
using UnityEngine;

namespace ShapeTetris.Behaviours
{
    [RequireComponent(typeof(BoxCollider))]
    public class PieceWrapperBehaviour : MonoBehaviour
    {
        private VoxelVolume _volume;

        public VoxelVolume Volume
        {
            get
            {
                if (!_volume) _volume = GetComponentInChildren<VoxelVolume>();
                return _volume;
            }

            set
            {
                if(value.transform.parent == transform)
                    _volume = value;
            }
        }

        public void UpdateCollision(Vector3 size, Vector3 center)
        {
            GetComponent<BoxCollider>().center = center;
            GetComponent<BoxCollider>().size = size;
        }
    }
}