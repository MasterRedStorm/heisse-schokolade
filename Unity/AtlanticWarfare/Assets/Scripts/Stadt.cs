using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stadt : MonoBehaviour
{
    private List<Transform> EnemiesInRange = new List<Transform>();
    private LineRenderer[] Lasers = new LineRenderer[5];
    private int Level = 0;
    [SerializeField] private Transform[] Turrets = new Transform[5];
    private List<Transform> ActiveTurrets = new List<Transform>();
    [SerializeField] private Transform[] Arms = new Transform[2];


    public void Start()
    {
        Lasers = GetComponentsInChildren<LineRenderer>();
        ActiveTurrets.Add(Turrets[0]);
        foreach (Transform Arm in Arms) Arm.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) SetLevel(Level + 1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) SetLevel(Level - 1);
        Shoot();
    }

    public void SetLevel(int value)
    {
        if (value < 0 || value==Level || value > 2) return;
        ActiveTurrets.Clear();
        ActiveTurrets.Add(Turrets[0]);
        for (int i = 1; i <= value; i++)
        {
            ActiveTurrets.Add(Turrets[2 * i - 1]);
            ActiveTurrets.Add(Turrets[2 * i]);
        }
        for (int i = 0; i < Arms.Length; i++) Arms[i].gameObject.SetActive(value > i);
        Level = value;
    }

    private void ResetLasers(){ foreach (LineRenderer Laser in Lasers) Laser.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero }); }

    public void Shoot()
    {
        if (EnemiesInRange.Count == 0) return;
        System.Random R = new System.Random();
        Transform Target = null;
        foreach (Transform turret in ActiveTurrets)
        {
            Target = EnemiesInRange[R.Next(0, EnemiesInRange.Count - 1)];
            Lasers[ActiveTurrets.IndexOf(turret)].SetPositions(new Vector3[] { turret.GetChild(turret.childCount - 1).transform.position, Target.position });
            turret.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Target.position-turret.position, Vector3.up),Vector3.up);
            if (Target.GetComponent<Health>().DoDamage(10)) EnemiesInRange.Remove(Target);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player") EnemiesInRange.Add(other.transform);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player") EnemiesInRange.Remove(other.transform);
    }
}
