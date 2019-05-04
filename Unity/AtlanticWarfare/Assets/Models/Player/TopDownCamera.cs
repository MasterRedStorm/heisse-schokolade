using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] bool NorthUp = true;

    public void Update()
    {
        if (NorthUp) transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.forward);
    }
}
