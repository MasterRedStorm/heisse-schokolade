using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    public static Game_manager instance = null;

    private bool GameIsRunning = false;

    private Player Player;
    private City[] Citys = new City[] { };
    private Enemie[] Enemies = new Enemie[] { };
    private static float Kapital = 0;
    [SerializeField] private TextMesh KapitalText = null;

    void Start()
    {
        StartCoroutine(IncreaseMoney());
    }

    public void Update()
    {
        KapitalText.text = "Investkapital: " + Kapital + "/1000";
    }

    private void IncreaseKapital()
    {
        Kapital += Citys.Length * (Kapital / 10);
        if (Kapital <= 0)
        {
            Kapital = 0;
        }
    }

    private void DecreaseKapital(float amount)
    {
        Kapital -= amount;
    }

    IEnumerator IncreaseMoney()
    {
        while (true)
        {
            Kapital += Citys.Length * (Kapital / 10);
            SetKapitalText();
            yield return new WaitForSeconds(1);
        }
    }

    private void SetKapitalText()
    {
        KapitalText.text = "Investkapital: " + Kapital;
    }
}