using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapSettings : MonoBehaviour
{
    public static MapSettingsSO Data => _instance?._settingsData;

    [SerializeField] MapSettingsSO _settingsData;

    static MapSettings _instance;

    private void OnEnable()
    {
        if (_instance == null)
            _instance = this;
        else
            DestroyImmediate(this);
    }
}
