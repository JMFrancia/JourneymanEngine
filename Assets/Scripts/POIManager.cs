using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
[System.Serializable]
public class POIManager : MonoBehaviour
{
    public string Name => gameObject.name;

    //[SerializeReference] POIDataSO _data;
    [Header("References")]
    [SerializeField] GameObject _pathObjPrefab;

    Dictionary<POIManager, PathController> _pathDict;

    [SerializeField] List<PathController> _paths;

    const string PATH_NAME_TEMPLATE = "{0} <--> {1}";

    private void Awake()
    {
        //LoadData();
        _pathDict = LoadPathData();
        
    }

    Dictionary<POIManager, PathController> LoadPathData()
    {
        Dictionary<POIManager, PathController> result = new Dictionary<POIManager, PathController>();
        for (int n = 0; n < _paths.Count; n++)
        {
            if (TryGetPathDestination(_paths[n], out POIManager destination))
            {
                result[destination] = _paths[n];
            }
        }
        return result;
    }

    public bool PathExists(POIManager destination)
    {
        return _pathDict != null && _pathDict.ContainsKey(destination);
    }

    public bool TryGetPath(POIManager destination, out PathController path)
    {
        path = null;
        if (!_pathDict.ContainsKey(destination))
            return false;
        path = _pathDict[destination];
        return true;
    }

    public POIManager GetRandomDestination()
    {
        List<POIManager> destinations = new List<POIManager>(_pathDict.Keys);
        return destinations[Random.Range(0, destinations.Count)];
    }

    public void OnDrawGizmos()
    {
        if (Selection.gameObjects.Length == 2)
        {
            POIManager poi1 = Selection.gameObjects[0].GetComponent<POIManager>();
            POIManager poi2 = Selection.gameObjects[1].GetComponent<POIManager>();
            if (poi1 != null && poi2 != null && !poi1.PathExists(poi2))
            {

            }
        }
        if (CanCreatePathWithSelected())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Selection.transforms[0].position, Selection.transforms[1].position);
        }
        Gizmos.color = Color.magenta;
        Handles.Label(transform.position + new Vector3(0, 2f, 0), Name);
    }

    //Edit mode only
    public bool TryCreatePathWithSelected()
    {
        if (CanCreatePathWithSelected())
        {
            POIManager poi1 = Selection.gameObjects[0].GetComponent<POIManager>();
            POIManager poi2 = Selection.gameObjects[1].GetComponent<POIManager>();
            POIManager destination = poi1 == this ? poi2 : poi1;
            return CreatePath(destination);
        }
        else
        {
            return false;
        }
    }

    //Edit mode only
    public bool CreatePath(POIManager destination)
    {
        if (_pathDict == null)
        {
            _pathDict = new Dictionary<POIManager, PathController>();
        }
        var path = Instantiate(_pathObjPrefab).GetComponent<PathController>();
        path.name = string.Format(PATH_NAME_TEMPLATE, Name, destination.Name);
        path.Setup(this, destination);
        if (AddPath(path) && destination.AddPath(path))
        {
            Debug.Log("Successfully added path between " + Name + " and " + destination.Name);
            return true;
        }
        else
        {
            return false;
        }
    }

    //void UpdateData()
    //{
    //    EditorUtility.SetDirty(_data);
    //    AssetDatabase.SaveAssets();
    //    Debug.Log($"Data saved for {_data.name}");
    //}

    //public void LoadData() {
    //    if (_data == null) {
    //        Debug.LogError($"Can't load data for {gameObject}, null reference");
    //        return;
    //    }
    //    _pathDict = LoadPathData();
    //    gameObject.name = _data.Name;
    //    Debug.Log($"Data loaded for {_data.Name}");
    //}

    //Edit mode only
    public bool CanCreatePathWithSelected()
    {
        if (Selection.gameObjects.Length == 2)
        {
            POIManager poi1 = Selection.gameObjects[0].GetComponent<POIManager>();
            POIManager poi2 = Selection.gameObjects[1].GetComponent<POIManager>();
            return (poi1 == this || poi2 == this) && !poi1.PathExists(poi2);
        }
        else
        {
            return false;
        }
    }

    //Edit mode only
    public bool AddPath(PathController path)
    {
        if (_paths.Contains(path))
        {
            return false;
        }
        if (TryGetPathDestination(path, out POIManager destination))
        {
            _pathDict.Add(destination, path);
            _paths.Add(path);
            Debug.Log("Adding path in " + gameObject.name);
        }
        else
        {
            Debug.LogWarning($"Attempting to add path {path} to POI {gameObject}, but {gameObject} isn't an endpoint. Endpoints are {path.EndPoint1} and {path.EndPoint2}");
            return false;
        }
        EditorUtility.SetDirty(this);

        return true;
        /*
        if (_pathDict == null)
        {
            _pathDict = new Dictionary<POIManager, PathController>();
        }
        if (TryGetPathDestination(path, out POIManager destination))
        {
            _pathDict[destination] = path;
        }
        else {
            Debug.LogWarning($"Attempting to add path {path} to POI {gameObject}, but {gameObject} isn't an endpoint. Endpoints are {path.EndPoint1} and {path.EndPoint2}");
            return false;
        }
        return true;
        */
    }

    //Edit mode only
    public void RemovePath(POIManager destination)
    {
        if (_pathDict != null)
        {
            _paths.Remove(_pathDict[destination]);
            _pathDict.Remove(destination);
        }
    }

    bool TryGetPathDestination(PathController path, out POIManager destination)
    {
        destination = null;
        if (path.EndPoint1 != this && path.EndPoint2 != this)
        {
            return false;
        }
        destination = path.EndPoint1 == this ? path.EndPoint2 : path.EndPoint1;
        return true;
    }
}
