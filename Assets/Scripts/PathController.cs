using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[System.Serializable]
public class PathController : MonoBehaviour
{
    //[SerializeField] PathDataSO _data;
    [SerializeField] GameObject _pathPointPrefab;

    public POIManager EndPoint1 => _endPoint1;
    public POIManager EndPoint2 => _endPoint2;
    public List<Transform> PathPoints => _pathPoints;

    [SerializeField] POIManager _endPoint1;
    [SerializeField] POIManager _endPoint2;
    [SerializeField] List<Transform> _pathPoints;

    public void Setup(POIManager endPoint1, POIManager endPoint2)
    {
        _endPoint1 = endPoint1;
        _endPoint2 = endPoint2;
        _pathPoints = new List<Transform>() {
            _endPoint1.transform,
            _endPoint2.transform
        };
    }

    //Edit mode only
    public void AddPathPoints() {
        int pathSize = (2 * _pathPoints.Count) - 1;
        Transform lastPoint = _pathPoints[0];
        Transform nextPoint = _pathPoints[1];
        Transform[] newPath = new Transform[pathSize];
        int oldPathIndex = 0;
        for (int newPathIndex = 0; newPathIndex < pathSize; newPathIndex++) {
            //Existing path point
            if (newPathIndex % 2 == 0)
            {
                newPath[newPathIndex] = _pathPoints[oldPathIndex];
                lastPoint = _pathPoints[oldPathIndex];
                oldPathIndex++;
                if (newPathIndex < pathSize - 1)
                {
                    nextPoint = _pathPoints[oldPathIndex];
                }
            }
            //New path point
            else {
                GameObject pathPoint = Instantiate(_pathPointPrefab, transform);
                pathPoint.transform.position = GetHalfwayPoint(lastPoint.position, nextPoint.position);
                newPath[newPathIndex] = pathPoint.transform;
                lastPoint = pathPoint.transform;
            }
        }
        _pathPoints = new List<Transform>(newPath);
    }

    //Edit mode only
    Vector3 GetHalfwayPoint(Vector3 p1, Vector3 p2) {
        return (p1 + p2) / 2;
    }

    private void Update()
    {
        if (_endPoint1 != null && _endPoint2 != null) {
            transform.position = (_endPoint1.transform.position + _endPoint2.transform.position) / 2;
        }
    }

    private void OnDrawGizmos()
    {
        //Draw path
        if (_endPoint1 != null && _endPoint2 != null)
        {
            Gizmos.color = Color.blue;
            if (_pathPoints == null || _pathPoints.Count == 0)
            {
                Gizmos.DrawLine(_endPoint1.transform.position, _endPoint2.transform.position);
            }
            else
            {
                Gizmos.DrawLine(_endPoint1.transform.position, _pathPoints[0].transform.position);
                for (int n = 0; n < _pathPoints.Count - 1; n++)
                {
                    Gizmos.DrawLine(_pathPoints[n].transform.position, _pathPoints[n + 1].transform.position);
                }
                Gizmos.DrawLine(_pathPoints[_pathPoints.Count - 1].transform.position, EndPoint2.transform.position);
            }

            Gizmos.color =  new Color(1, 1, 1, .25f);
            Gizmos.DrawLine(_endPoint1.transform.position, _endPoint2.transform.position);
        }



        //Draw distance
        Gizmos.color = Color.magenta;
        Handles.Label(transform.position + new Vector3(0, 2f, 0), GetDistance().ToString());
    }

    public float GetDistance() {
        return Vector3.Distance(EndPoint1.transform.position, EndPoint2.transform.position) * MapSettings.Data.MilesToUnits;
    }

    public void DeletePath() {
        EndPoint1.RemovePath(EndPoint2);
        EndPoint2.RemovePath(EndPoint1);
        DestroyImmediate(gameObject);
    }

    public float PathLength {
        get {
            float result = 0f;
            for (int n = 1; n < _pathPoints.Count; n++) {
                result += Vector3.Distance(_pathPoints[n].position, _pathPoints[n - 1].position);
            }
            return result;
        }
    }
}
