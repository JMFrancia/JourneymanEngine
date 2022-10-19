using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MapSettings", order = 1)]
public class MapSettingsSO : ScriptableObject
{
    public float MilesToUnits = 1f;
}
