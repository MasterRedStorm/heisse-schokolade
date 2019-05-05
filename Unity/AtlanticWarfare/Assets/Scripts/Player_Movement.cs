using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private KeyCode[] WASD_Controls = new KeyCode[4];
    [SerializeField] private KeyCode BuildCityKey = KeyCode.Space;
    [SerializeField] private KeyCode InvestKey = KeyCode.RightArrow;
    [SerializeField] private KeyCode StealKey = KeyCode.LeftArrow;

    public Vector3 MovingDir = Vector3.zero;
    [SerializeField] private Transform PlayerObject;
    [SerializeField, Range(0, 1)] private float Smoothness = 0.9f;
    private NavMeshAgent PlayerAgent = null;
    [SerializeField] string TerrainTag = "Player";
    [SerializeField] string MoveableTag = "Walkable";
    [SerializeField, Range(0,1)] private float Speed = 0.5f;
    public bool MoveIsPossible = false;
    

    public void Start()
    {
        PlayerAgent = PlayerObject.GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        Vector3 WantedDir = Vector3.zero;
        for (int i = 0; i < WASD_Controls.Length; i++) if (Input.GetKey(WASD_Controls[i])) WantedDir += new Vector3( (i-2)%2 , 0 , -(i-1)%2 );
        if (WantedDir == Vector3.zero) MovingDir *= 1-Speed;
        else MovingDir = Vector3.Lerp(MovingDir, WantedDir.normalized, (1 - Smoothness)).normalized;
        if (Input.GetKey(BuildCityKey)) Game_manager.BuildCity(transform.position + transform.forward * 3);
        if (Input.GetKey(InvestKey)) Game_manager.InvestInCity(transform.position);
        if (Input.GetKey(StealKey)) Game_manager.StealFromCity(transform.position);

    }

    public void FixedUpdate()
    {
        if (PlayerAgent == null) return;
        Ray ToNextWayPoint = new Ray(PlayerObject.transform.position + PlayerObject.transform.up, MovingDir*Speed - PlayerObject.transform.up);
        RaycastHit[] Hits = Physics.RaycastAll(ToNextWayPoint);
        Debug.DrawRay(ToNextWayPoint.origin, ToNextWayPoint.direction, Color.green, 30);

        foreach (RaycastHit Hit in Hits)
        {
            NavMeshHit NavHit = new NavMeshHit();
            if (Hit.collider.tag == TerrainTag && NavMesh.SamplePosition(Hit.point, out NavHit, 1, NavMesh.AllAreas))
            {
                Debug.Log("Move?:"+MoveIsPossible, this);
                if (MoveIsPossible) PlayerAgent.nextPosition = NavHit.position;
                Debug.DrawRay(NavHit.position, Vector3.up , Color.red, 30);
                break;
            }
        }

        if (MovingDir.magnitude == 1)PlayerObject.rotation = Quaternion.LookRotation(MovingDir, Vector3.up);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == MoveableTag)
            MoveIsPossible = true;
    }
    public void OnTriggerExit(Collider other){ if (other.tag == MoveableTag) MoveIsPossible = false; }

}
