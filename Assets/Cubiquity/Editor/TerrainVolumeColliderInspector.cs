using UnityEngine;
using UnityEditor;

using System.Collections;

namespace Cubiquity
{
    [CustomEditor(typeof(TerrainVolumeCollider))]
    public class TerrainVolumeColliderInspector : VolumeColliderInspector
    {
        public override void OnInspectorGUI()
        {
            OnInspectorGUIImpl();
        }
    }
}