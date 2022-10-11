using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathController : MonoBehaviour
{
    public POIManager EndPoint1 => _endPoint1;
    public POIManager EndPoint2 => _endPoint2;

    POIManager _endPoint1;
    POIManager _endPoint2;

    public void SetEndPoints(POIManager endPoint1, POIManager endPoint2)
    {
        _endPoint1 = endPoint1;
        _endPoint2 = endPoint2;
    }

    private void Update()
    {
        if (_endPoint1 != null && _endPoint2 != null) {
            transform.position = (_endPoint1.transform.position + _endPoint2.transform.position) / 2;
        }
    }

    private void OnDrawGizmos()
    {
        if (_endPoint1 != null && _endPoint2 != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_endPoint1.transform.position, _endPoint2.transform.position);
        }
    }

    public void DeletePath() {
        EndPoint1.RemovePath(EndPoint2);
        EndPoint2.RemovePath(EndPoint1);
        DestroyImmediate(gameObject);
    }
}
