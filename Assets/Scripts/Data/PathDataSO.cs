using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PathDataSO : SerializedScriptableObject
{
    public POIDataSO EndPoint1;
    public POIDataSO EndPoint2;
    public List<Vector3> PathPoints;
}
