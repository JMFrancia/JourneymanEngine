using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Data", menuName = "POI", order = 1)]
[System.Serializable]
public class POIDataSO : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Image;
}
