using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Game_manager : MonoBehaviour
{
    [SerializeField] Transform Spawnpoint = null;
    static Spawn Spawnscript = null;
    static GameObject Cityprefab = null;
    [SerializeField] GameObject Cityprefab1 = null;
    [SerializeField] GameObject Playerprefab = null;
    private static List<Stadt> Cities = new List<Stadt>();
    public static int Kapital = 1;
    private static int MaxKapital = 20000;
    [SerializeField] private TextMeshProUGUI KapitalText = null;
    private static string TerrainTag = "Player";
    [SerializeField] static private int KostenProStadt = 10;
    [SerializeField] private TextMeshProUGUI GlobalMessage1;
    private static TextMeshProUGUI GlobalMessage;

    void Start()
    {
        GlobalMessage = GlobalMessage1;
        if (Spawnpoint == null || Cityprefab1==null || Playerprefab == null) { Debug.Log("Error starting Game manager", this); return; }
        RaycastHit Hit;
        Physics.Raycast(Spawnpoint.position, Vector3.down, out Hit);
        Spawnpoint.position = Hit.point;
        Instantiate(Playerprefab).transform.position = Spawnpoint.transform.position;
        Spawnscript = GetComponent<Spawn>();

        Cityprefab = Cityprefab1;
        Cities.Add(Instantiate(Cityprefab).GetComponent<Stadt>());
        Cities[0].transform.position = Spawnpoint.transform.position;
        Spawnscript.AddCity(Cities[0].gameObject);
        StartCoroutine(IncreaseMoney());
    }

    public static void Reset()
    {
        Cities.Clear();
        Spawnscript = null;
        Cityprefab = null;
        Kapital = 1;
        MaxKapital = 20000;
        KostenProStadt = 10;
        GlobalMessage.text = "";
        GlobalMessage = null;
    }

    public static void Endgame(bool Win)
    {
        Reset();
        if (Win) PlayerPrefs.SetInt("WinningState", 1);
        else PlayerPrefs.SetInt("WinningState", 0);
        SceneManager.LoadScene("MainMenu");
        Debug.Log("GameOver");
    }

    public static void KillCity(Stadt city)
    {
        Cities.Remove(city);
        if (Cities.Count == 0) Endgame(false);
    }

    public static bool BuildCity(Vector3 Pos)
    {
        RaycastHit[] Hits = Physics.RaycastAll(Pos + Vector3.up * 100, Vector3.down);
        bool CanBuild = false;
        foreach (RaycastHit hit in Hits)
        {
            if (hit.collider.tag == TerrainTag) CanBuild = true;
            if (hit.collider.tag == Cityprefab.tag) CanBuild = false;
        }
        if (CanBuild && Kapital >= KostenProStadt * Cities.Count)
        {
            Stadt city = Instantiate(Cityprefab).GetComponent<Stadt>();
            Cities.Add(city);
            Spawnscript.AddCity(city.gameObject);
            IncreaseKapital(-KostenProStadt * Cities.Count);
            ChatMessage("Tower built!");
        }
        else
        {
            if (CanBuild) ChatMessage("Dude, you're boke!");
            else ChatMessage("Can't build here.");
        }

        return CanBuild;
    }

    public static void ChatMessage(string message)
    {
        GlobalMessage.text = message;

    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(3);
    }

    private static Stadt GetStadt(Vector3 Pos)
    {
        float Distance = float.MaxValue;
        Stadt Closest = Cities[0];
        foreach (Stadt City in Cities) if (Vector3.Distance(Pos, City.transform.position) < Distance)
            {
                Closest = City;
                Distance = Vector3.Distance(Pos, City.transform.position);
            }
        return Closest;
    }

    public static void InvestInCity(Vector3 Pos)
    {
        if (Kapital <= 0) return;
        if (GetStadt(Pos).Invest(1)) IncreaseKapital(-1);
    }

    public static void StealFromCity(Vector3 Pos)
    {
        if (GetStadt(Pos).Steal()) IncreaseKapital(1);
        else if (Cities[0].Steal()) IncreaseKapital(1);
    }

    public void Update()
    {
        KapitalText.text = "Investkapital: " + Kapital + "/" + MaxKapital;
        if (1 - Kapital / (Kapital + 1) != 0) Spawnscript.spawnCooldown *= 1 - Kapital / (Kapital + 1);
    }

    private static void IncreaseKapital(int Amount)
    {
        Kapital += Amount;
        if (Kapital <= 0)
        {
            Kapital = 0;
        }
        if (Kapital >= MaxKapital) Endgame(true);
    }

    public static bool DecreaseKapital(int Amount)
    {
        if (Kapital >= Amount)
        {
            Kapital -= Amount;
            return true;
        } else
        {
            return false;
        }
    }

    IEnumerator IncreaseMoney()
    {
        while (true)
        {
            if (Kapital > 10) IncreaseKapital(Cities.Count * (Kapital / 10));
            else IncreaseKapital(1);
            SetKapitalText();
            yield return new WaitForSeconds(1);
        }
    }

    private void SetKapitalText()
    {
        KapitalText.text = "Investkapital: " + Kapital;
    }
       
}