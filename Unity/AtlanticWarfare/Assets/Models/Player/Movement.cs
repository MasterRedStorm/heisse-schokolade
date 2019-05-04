using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private KeyCode[] WASD_Controls = new KeyCode[4];
    [SerializeField, Range(0,1)] private float Speed = 0.75f;
    private Vector3 MovingDir = Vector3.zero;
    [SerializeField] private Transform PlayerObject;
    [SerializeField, Range(0, 1)] private float Smoothness = 0.9f;
    [SerializeField] string TerrainTag = "Walkable";
    [SerializeField] string ObstacleTag = "Obstacle";

    public void Update()
    {
        Vector3 WantedDir = Vector3.zero;
        for (int i = 0; i < WASD_Controls.Length; i++) if (Input.GetKey(WASD_Controls[i])) WantedDir += new Vector3( (i-2)%2 , 0 , -(i-1)%2 );
        if (WantedDir == Vector3.zero) MovingDir *= 1-Speed;
        else MovingDir = Vector3.Lerp(MovingDir, WantedDir.normalized, (1-Smoothness)).normalized;
    }

    public void FixedUpdate()
    {
        Ray TestForWalkable = new Ray(PlayerObject.transform.position + PlayerObject.transform.up, MovingDir.normalized * Speed - PlayerObject.transform.up);
        RaycastHit[] Hits = Physics.RaycastAll(TestForWalkable);
        bool MoveIsPossible = false;
        foreach (RaycastHit Hit in Hits)
        {
            if (Hit.collider.tag == TerrainTag) MoveIsPossible = true;
            if (Hit.collider.tag == ObstacleTag) { MoveIsPossible = false; break; }
        }
        Debug.DrawRay(TestForWalkable.origin, TestForWalkable.direction, Color.green, 30);
        if (MoveIsPossible) PlayerObject.transform.position += (MovingDir*Speed);
        if (MovingDir.magnitude == 1)PlayerObject.rotation = Quaternion.LookRotation(MovingDir, Vector3.up);
    }
}
