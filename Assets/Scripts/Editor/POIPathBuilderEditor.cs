using UnityEngine;
using UnityEditor;

[ExecuteAlways]
[CanEditMultipleObjects]
[CustomEditor(typeof(POIPathBuilder))]
public class POIPathBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();

        if (Selection.gameObjects.Length == 2) {
            if ((target as POIPathBuilder).CanCreatePathWithSelected())
            {
                if (GUILayout.Button("Create Path"))
                {
                    (target as POIPathBuilder).TryCreatePathWithSelected();
                }
            }
        }
    }
}
