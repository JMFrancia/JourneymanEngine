using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[ExecuteAlways]
[CanEditMultipleObjects]
[CustomEditor(typeof(POIManager))]
public class POIManagerEditor : Editor
{
    //const string NAME_PROPERTY = "_name";
    //const string LABEL_REF_PROPERTY = "_label";

    //SerializedProperty _nameProperty;
    //SerializedProperty _labelReferenceProperty;

    //private void OnEnable()
    //{
    //    _nameProperty = serializedObject.FindProperty(NAME_PROPERTY);
    //    _labelReferenceProperty = serializedObject.FindProperty(LABEL_REF_PROPERTY);
    //}

    //[ExecuteAlways]
    //private void OnValidate()
    //{
    //    if()
    //    (target as POIManager).OnDataLoaded();
    //}

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
        //if (GUILayout.Button("LoadData")) {
        //    (target as POIManager).LoadData();
        //}
        if ((target as POIManager).CanCreatePathWithSelected())
        {
            if (GUILayout.Button("Create Path"))
            {
                (target as POIManager).TryCreatePathWithSelected();
            }
        }
    }
}
