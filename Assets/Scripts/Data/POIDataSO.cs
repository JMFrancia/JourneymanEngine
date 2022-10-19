using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Data", menuName = "POI", order = 1)]
public class POIDataSO : SerializedScriptableObject
{
    public string Name;
    public Vector3 Position;
    public List<PathController> Paths = new List<PathController>();
}
