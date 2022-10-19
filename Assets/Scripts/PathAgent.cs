using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PathAgent : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
    [SerializeField] float _pathYOffset = 2f;
    [SerializeField] bool _wanderMode = true;

    
    List<Transform> _route;
    POIManager _destination;
    float _pathLength;
    bool _traveling = false;

    public void AttachToNearestPOI() {
        transform.position = GetClosestPOI().transform.position + new Vector3(0f, _pathYOffset, 0f);
    }

    public void SetDestination(PathController path, POIManager destination) {
        if (path.EndPoint1 != destination && path.EndPoint2 != destination) {
            Debug.LogError($"Attempting to set non-existant destination {destination.Name} on path {path.gameObject.name}");
            return;
        }
        List<Transform> pathPoints = path.PathPoints;
        if (path.EndPoint1 == destination) {
            pathPoints.Reverse();
        }
        _route = new List<Transform>(pathPoints);
        _pathLength = path.PathLength;
    }

    public void StartTravel() {
        if (_route == null || _route.Count == 0)
            return;

        float totalTime = _pathLength * _speed;

        Sequence travelSequence = DOTween.Sequence();
        for (int n = 1; n < _route.Count; n++) {
            float travelDist = Vector3.Distance(_route[n].position, _route[n - 1].position);
            float normalizedDist = travelDist / _pathLength;
            float travelTime = normalizedDist * totalTime;

            travelSequence.Append(transform.DOMove(_route[n].position, travelTime).SetEase(Ease.Linear));
            _traveling = true;
            Debug.Log("Starting travel");
        }
        travelSequence.OnComplete(() => {
            OnTravelComplete();
        });
    }

    void OnTravelComplete()
    {
        _traveling = false;
        if (_wanderMode)
            SetRandomDestinationFromNearestPOIAndTravel();
    }

    public void SetRandomDestinationFromNearestPOIAndTravel() {
        POIManager startingPoint = GetClosestPOI();
        SetRandomDestinationFromStartingPoint(startingPoint);
        StartTravel();
    }

    public void SetRandomDestinationFromStartingPoint(POIManager startingPoint) {
        POIManager destination = startingPoint.GetRandomDestination();
        startingPoint.TryGetPath(destination, out PathController path);
        SetDestination(path, destination);
    }

    //Refactor - this is awful
    POIManager GetClosestPOI()
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        var allPOI = GameObject.FindGameObjectsWithTag("POI");
        foreach (GameObject potentialTarget in allPOI)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }
        return bestTarget.GetComponent<POIManager>();
    }
}
