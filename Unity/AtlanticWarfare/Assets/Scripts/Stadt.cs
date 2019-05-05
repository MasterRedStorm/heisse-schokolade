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
    [SerializeField] private string EnemyTag = "Enemy";
    [SerializeField, Range(0,10)] private float FireRate = 0.5f;

    IEnumerator Shootloop()
    {
        while (true)
        {
            if (Shoot())
            {
                yield return new WaitForSeconds(1/FireRate);
                ResetLasers();
            }
            else
            {
                yield return null;
            }
        }
    }

    public bool Invest(int Amount)
    {
        Health H = transform.GetComponent<Health>();
        if (H.CurrentHealth < H.MaxHealth)
        {
            H.DoDamage(-10 * Amount); 
            Game_manager.ChatMessage("Healing Tower...");
            return true;
        }
        if (Level < 2 && Game_manager.DecreaseKapital(Game_manager.Kapital / 20 + 20))
        {
            Game_manager.ChatMessage("Upgrading Tower...");
            SetLevel(Level + 1);
            return true;
        } else Game_manager.ChatMessage("Tower maxed.");
        return false;
    }

    public bool Steal()
    {
        Health H = transform.GetComponent<Health>();
        if (Level > 0)
        {
            SetLevel(Level - 1); return true;
            Game_manager.ChatMessage("Downgrading Tower...");
        }
        if (H.CurrentHealth > 10)
        {
            Game_manager.ChatMessage("Robbing Tower...");
            H.DoDamage(10); return true;
        }
        Game_manager.ChatMessage("Don't kill Towers! plz?");
        return false;
    }

    public void Start()
    {
        Lasers = GetComponentsInChildren<LineRenderer>();
        ActiveTurrets.Add(Turrets[0]);
        foreach (Transform Arm in Arms) Arm.gameObject.SetActive(false);
        StartCoroutine(Shootloop());
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

    public bool Shoot()
    {
        if (EnemiesInRange.Count == 0) return false;
        System.Random R = new System.Random();
        Transform Target = null;
        foreach (Transform turret in ActiveTurrets)
        {
            Target = EnemiesInRange[R.Next(0, EnemiesInRange.Count - 1)];
            Lasers[ActiveTurrets.IndexOf(turret)].SetPositions(new Vector3[] { turret.GetChild(turret.childCount - 1).transform.position, Target.position });
            turret.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(Target.position-turret.position, Vector3.up),Vector3.up);

            if (Target.GetComponent<Health>().DoDamage(10))
            {
                EnemiesInRange.Remove(Target);
                Game_manager.ChatMessage("Enemy down!");
            }
        }
        return true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == EnemyTag) EnemiesInRange.Add(other.transform);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == EnemyTag) EnemiesInRange.Remove(other.transform);
    }
}
