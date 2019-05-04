using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] Transform Target = null;
    public void Update()
    {
        if (Target == null) return;
        transform.rotation = Quaternion.LookRotation(Target.position - transform.position, Vector3.up);
    }
}
