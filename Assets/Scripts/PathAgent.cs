using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PathAgent : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
    [SerializeField] float _pathYOffset = 2f;
    [SerializeField] bool _wanderMode = true;
    [SerializeField] bool _narrateTravel = false;

    
    List<Transform> _route;
    POIManager _destination;
    float _pathLength;
    bool _traveling = false;

    public Queue<string> _travelLogMessages;

    public void AttachToNearestPOI() {
        transform.position = GetClosestPOI().transform.position + new Vector3(0f, _pathYOffset, 0f);
    }

    public void SetDestination(PathController path, POIManager destination) {
        if (path.EndPoint1 != destination && path.EndPoint2 != destination) {
            Debug.LogError($"Attempting to set non-existant destination {destination.Name} on path {path.gameObject.name}");
            return;
        }
        List<Transform> pathPoints = new List<Transform>(path.PathPoints);
        if (path.EndPoint1 == destination) {
            Debug.Log("reversing path");
            pathPoints.Reverse();
        }
        _route = new List<Transform>(pathPoints);
        _pathLength = path.PathLength;
        LogTravel($"Path agent {gameObject.name}: \"Setting destination to {destination.Name}...\"");
    }

    public void StartTravel() {
        if (_route == null || _route.Count == 0)
            return;

        float totalTime = _pathLength * _speed;

        Sequence travelSequence = DOTween.Sequence();
        _travelLogMessages.Clear();
        for (int n = 1; n < _route.Count; n++) {
            float travelDist = Vector3.Distance(_route[n].position, _route[n - 1].position);
            float normalizedDist = travelDist / _pathLength;
            float travelTime = normalizedDist * totalTime;

            travelSequence.Append(transform.DOMove(_route[n].position, travelTime).SetEase(Ease.Linear));

            if ((n + 1) == _route.Count - 1)
            {
                _travelLogMessages.Enqueue($"Path agent {gameObject.name}: \"Hit waypoint {(n+1)}/{_route.Count}, on my way to final destination!\"");
            }
            else if ((n + 1) < _route.Count - 1)
            {
                _travelLogMessages.Enqueue($"Path agent {gameObject.name}: \"Hit waypoint {(n+1)}/{_route.Count}, on my way to {(n + 2)}!\"");
            }
            travelSequence.AppendCallback(() => {
                if (_travelLogMessages.Count > 0)
                {
                    LogTravel(_travelLogMessages.Dequeue());
                }
            });
            _traveling = true;
        }
        travelSequence.OnComplete(() => {
            OnTravelComplete();
        });

        LogTravel($"Path agent {gameObject.name}: \"Starting travel!\"");
    }

    private void Awake()
    {
        _travelLogMessages = new Queue<string>();
    }

    void LogTravel(string log) {
        if (_narrateTravel)
        {
           Debug.Log(log);
        }
    }

    void OnTravelComplete()
    {
        LogTravel($"Path agent {gameObject.name}: \"Travel complete!\"");
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
