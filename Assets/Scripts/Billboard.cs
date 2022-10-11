using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Billboard : MonoBehaviour
{
    void Update()
    {
        transform.forward = Camera.main.transform.forward * -1;
    }
}
