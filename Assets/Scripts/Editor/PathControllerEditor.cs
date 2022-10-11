using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(PathController))]
public class PathControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.CreateInspectorGUI();
        if (GUILayout.Button("Delete Path")) {
            (target as PathController).DeletePath();
        }
    }
}
