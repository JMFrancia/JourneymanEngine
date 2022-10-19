using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(PathAgent))]
public class PathAgentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();

        if (GUILayout.Button("Attach to POI")) {
            (target as PathAgent).AttachToNearestPOI();
        }
        if (GUILayout.Button("Start wander")) {
            (target as PathAgent).SetRandomDestinationFromNearestPOIAndTravel();
        }
    }
}
