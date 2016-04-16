using UnityEngine;
using UnityEditor;

using System.Collections;

namespace Cubiquity
{
    [CustomEditor(typeof(ColoredCubesVolumeCollider))]
    public class ColoredCubesVolumeColliderInspector : VolumeColliderInspector
    {
        public override void OnInspectorGUI()
        {
            OnInspectorGUIImpl();
        }
    }
}