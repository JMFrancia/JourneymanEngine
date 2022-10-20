using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(POIController))]
public class POIPathBuilder : MonoBehaviour
{
    public string Name => POI.Name;

    [SerializeField] GameObject _pathObjPrefab;
    [SerializeField] List<POIPathBuilder> _destinations;

    List<PathController> Paths {
        get => POI.Paths;
        set => POI.Paths = value;
    }

    const string PATH_NAME_TEMPLATE = "{0} <--> {1}";

    public POIController POI {
        get {
            if (_poiController == null)
            {
                _poiController = GetComponent<POIController>();
            }
            return _poiController;
        }
    }

    POIController _poiController;

    public Dictionary<POIController, PathController> GetPathData()
    {
        Dictionary<POIController, PathController> result = new Dictionary<POIController, PathController>();
        for (int n = 0; n < Paths.Count; n++)
        {
            if (TryGetPathDestination(Paths[n], out POIController destination))
            {
                result[destination] = Paths[n];
            }
        }
        return result;
    }

    public void OnDrawGizmos()
    {
        if (Selection.gameObjects.Length == 2)
        {
            POIController poi1 = Selection.gameObjects[0].GetComponent<POIController>();
            POIController poi2 = Selection.gameObjects[1].GetComponent<POIController>();
            if (poi1 != null && poi2 != null && !poi1.PathExists(poi2))
            {
                Gizmos.color = new Color(1, 1f, 1f, .5f);
                Gizmos.DrawLine(poi1.transform.position, poi2.transform.position);
            }
        }
        if (CanCreatePathWithSelected())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Selection.transforms[0].position, Selection.transforms[1].position);
        }
        Gizmos.color = Color.magenta;
        Handles.Label(transform.position + new Vector3(0, 2f, 0), POI.name);
    }

    public bool TryCreatePathWithSelected()
    {
        if (CanCreatePathWithSelected())
        {
            var poi1 = Selection.gameObjects[0].GetComponent<POIPathBuilder>();
            var poi2 = Selection.gameObjects[1].GetComponent<POIPathBuilder>();
            var destination = poi1 == this ? poi2 : poi1;
            return CreatePath(destination);
        }
        else
        {
            return false;
        }
    }

    public bool CreatePath(POIPathBuilder destination)
    {
        
        var path = Instantiate(_pathObjPrefab).GetComponent<PathController>();
        path.name = string.Format(PATH_NAME_TEMPLATE, POI.Name, destination.Name);
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

    public bool CanCreatePathWithSelected()
    {
        if (Selection.gameObjects.Length == 2)
        {
            POIPathBuilder poi1 = Selection.gameObjects[0].GetComponent<POIPathBuilder>();
            POIPathBuilder poi2 = Selection.gameObjects[1].GetComponent<POIPathBuilder>();
            return (poi1 == this || poi2 == this) && !poi1.PathExists(poi2);
        }
        else
        {
            return false;
        }
    }

    bool PathExists(POIController destination) {
        bool result = false;
        for (int n = 0; n < Paths.Count; n++) {
            if (TryGetPathDestination(Paths[n], out POIController dest)) {
                if (dest == destination) {
                    return true;
                }
            }
        }
        return result;
    }

    bool TryGetPathDestination(PathController path, out POIController destination)
    {
        destination = null;
        if (path.EndPoint1 != this && path.EndPoint2 != this)
        {
            return false;
        }
        destination = path.EndPoint1 == this ? path.EndPoint2 : path.EndPoint1;
        return true;
    }

    public bool AddPath(PathController path)
    {
        if (Paths.Contains(path))
        {
            return false;
        }
        if (TryGetPathDestination(path, out POIController destination))
        {
            Paths.Add(path);
            Debug.Log("Adding path in " + gameObject.name);
        }
        else
        {
            Debug.LogWarning($"Attempting to add path {path} to POI {gameObject}, but {gameObject} isn't an endpoint. Endpoints are {path.EndPoint1} and {path.EndPoint2}");
            return false;
        }
        EditorUtility.SetDirty(this);

        return true;
    }

    public void RemovePath(PathController path) => Paths.Remove(path);
}
