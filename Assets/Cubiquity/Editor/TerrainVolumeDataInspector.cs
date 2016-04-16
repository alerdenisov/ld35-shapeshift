using UnityEngine;
using UnityEditor;

using System.Collections;

namespace Cubiquity
{	
	[CustomEditor (typeof(TerrainVolumeData))]
	public class TerrainVolumeDataInspector : VolumeDataInspector
	{
		public override void OnInspectorGUI()
		{
			OnInspectorGUIImpl();
		}
	}
}