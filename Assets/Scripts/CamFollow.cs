using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform baseMovementObj;
    [SerializeField] Vector3 minRestrictions;
    [SerializeField] Vector3 maxRestrictions;
    [SerializeField] bool restrictXAxis = false;
    [SerializeField] bool restrictYAxis = false;
    [SerializeField] bool restrictZAxis = false;

    static CamFollow instance;

    Vector3 followCamDiff;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Debug.LogError($"Duplicate CamFollow on {gameObject.name}; deleting");
            Destroy(this);
        }
        followCamDiff = Camera.main.transform.position - baseMovementObj.position;
    }


    private void Update()
    {
        Vector3 newPos = baseMovementObj.position + followCamDiff;
        if (restrictXAxis)
        {
            newPos.x = Mathf.Clamp(newPos.x, minRestrictions.x, maxRestrictions.x);
        }
        if (restrictYAxis)
        {
            newPos.y = Mathf.Clamp(newPos.y, minRestrictions.y, maxRestrictions.y);
        }
        if (restrictZAxis)
        {
            newPos.z = Mathf.Clamp(newPos.z, minRestrictions.z, maxRestrictions.z);
        }

        Camera.main.transform.position = newPos;
    }
}
