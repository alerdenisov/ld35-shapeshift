using UnityEngine;
using System.Collections;
using TriangleMinecraft.Behaviours;
using UnityEditor;

[CustomEditor(typeof(VoxelTester))]
public class VoxelTesterEditor : Editor
{

    private VoxelDataBehaviour _selected;

    protected VoxelDataBehaviour Selected
    {
        get { return _selected;}
        set
        {
            if (_selected)
            {
                _selected.GetComponent<MeshRenderer>().enabled = false;
            }
            _selected = value;
            _selected.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseMove)
        {
//            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Selected = hit.collider.gameObject.GetComponent<VoxelDataBehaviour>();
            }
        }
        else if (Event.current.type == EventType.Layout)
        {
        }


        if (Selected)
        {
            Handles.BeginGUI();
            GUI.Box(new Rect(0,0, 250, 30), Selected.name);
            GUI.Box(new Rect(0, 30, 250, 30), string.Format("AA: {0}, AB: {1}, BA: {2}, BB: {3}", Selected.VoxelData.PointAA, Selected.VoxelData.PointAB, Selected.VoxelData.PointBA, Selected.VoxelData.PointBB));
            Handles.EndGUI();
        }
    }
}
