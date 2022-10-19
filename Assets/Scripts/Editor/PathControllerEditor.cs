using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(PathController))]
public class PathControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.CreateInspectorGUI();
        DrawDefaultInspector();

        if (GUILayout.Button("Add Path Points"))
        {
            (target as PathController).AddPathPoints();
        }
        if (GUILayout.Button("Delete Path")) {
            (target as PathController).DeletePath();
        }
    }
}
