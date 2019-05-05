using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider = null;
    [SerializeField] private int MaxHealth = 100;
    private int CurrentHealth;

    public void Start()
    {
        if (HealthSlider == null) { Debug.Log("No Health Slider!", this); return; }
        HealthSlider.minValue = 0;
        HealthSlider.maxValue = MaxHealth;
    }

    public void Update()
    {
        if (HealthSlider == null) return;
        HealthSlider.value = CurrentHealth;
    }

    public void DoDamage(int Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Kill();
        }
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
