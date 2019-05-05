using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    private static int Kapital = 0;
    [SerializeField] private TextMesh KapitalText = null;
    public static void ChangeKapital(int Amount){ Kapital+=Amount; }

    public void Update()
    {
        if (KapitalText==null) return;
        KapitalText.text = "Investkapital: " + Kapital + "/1000";
    }
}
