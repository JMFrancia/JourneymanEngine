using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class POIManager : MonoBehaviour
{
    [OnValueChanged("SetLabel")]
    [SerializeField] string _name;
    [Header("References")]
    //[SerializeField] TextMeshProUGUI _label;
    [SerializeField] GameObject _pathObject;

    Dictionary<POIManager, PathController> _pathDict;

    public bool PathExists(POIManager destination) {
        return _pathDict != null && _pathDict.ContainsKey(destination);
    }

    //public void SetLabel() {
    //    if (_label == null)
    //        return;
    //    _label.text = _name;
    //}

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
        if (CanCreatePathWithSelected()) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(Selection.transforms[0].position, Selection.transforms[1].position);
        }
        Gizmos.color = Color.magenta;
        Handles.Label(transform.position + new Vector3(0, 2f, 0), _name);
    }

    public bool TryCreatePathWithSelected() {
        if (CanCreatePathWithSelected())
        {
            POIManager poi1 = Selection.gameObjects[0].GetComponent<POIManager>();
            POIManager poi2 = Selection.gameObjects[1].GetComponent<POIManager>();
            POIManager destination = poi1 == this ? poi2 : poi1;
            return CreatePath(destination);
        }
        else {
            return false;
        }
    }

    public bool CreatePath(POIManager destination) {
        if (_pathDict == null) {
            _pathDict = new Dictionary<POIManager, PathController>();
        }
        var path = Instantiate(_pathObject).GetComponent<PathController>();
        path.SetEndPoints(this, destination);
        if (AddPath(path) && destination.AddPath(path))
        {
            Debug.Log("Successfully added path between " + gameObject + " and " + destination.gameObject);
            return true;
        }
        else {
            return false;
        }
    }

    public bool CanCreatePathWithSelected() {
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

    public bool AddPath(PathController path) {
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
    }

    public void RemovePath(POIManager destination) {
        if(_pathDict != null)
            _pathDict.Remove(destination);
    }

    bool TryGetPathDestination(PathController path, out POIManager destination) {
        destination = null;
        if (path.EndPoint1 != this && path.EndPoint2 != this)
        {
            return false;
        }
        destination = path.EndPoint1 == this ? path.EndPoint2 : path.EndPoint1;
        return true;
    }
}
