using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
[System.Serializable]
[RequireComponent(typeof(POIPathBuilder))]
public class POIController : MonoBehaviour
{
    public string Name => gameObject.name;

    //[SerializeReference] POIDataSO _data;

    Dictionary<POIController, PathController> _pathDict;

    public List<PathController> Paths;

    private void Awake()
    {
        _pathDict = GetComponent<POIPathBuilder>().GetPathData();
    }

    public bool PathExists(POIController destination)
    {
        return _pathDict != null && _pathDict.ContainsKey(destination);
    }

    public bool TryGetPath(POIController destination, out PathController path)
    {
        path = null;
        if (!_pathDict.ContainsKey(destination))
            return false;
        path = _pathDict[destination];
        return true;
    }

    public POIController GetRandomDestination()
    {
        List<POIController> destinations = new List<POIController>(_pathDict.Keys);
        return destinations[Random.Range(0, destinations.Count)];
    }
}
