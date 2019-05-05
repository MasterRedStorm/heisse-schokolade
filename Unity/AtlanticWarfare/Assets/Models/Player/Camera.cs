using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Cam : MonoBehaviour
{
    [SerializeField] private Transform Target = null;
    [SerializeField] private Vector3 Offset = new Vector3(0, 20, 20);

    public void Start()
    {
        Offset = transform.localPosition;
    }
    public void Update()
    {
        if (Target == null) return;
        transform.position = Target.position + Offset;
        transform.rotation = Quaternion.LookRotation(-Offset, Vector3.up);
    }
}
