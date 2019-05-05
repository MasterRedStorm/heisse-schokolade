﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider = null;
    [SerializeField] private int MaxHealth = 100;
    [SerializeField] private int CurrentHealth = 0;

    private GameObject ExplosionPrefab; 
    
    public void Start()
    {
        CurrentHealth = MaxHealth;
        
        if (HealthSlider == null) { Debug.Log("No Health Slider!", this); return; }
        HealthSlider.minValue = 0;
        HealthSlider.maxValue = MaxHealth;
        CurrentHealth = MaxHealth;
    }

    public void Update()
    {
        if (HealthSlider == null) return;
        HealthSlider.value = CurrentHealth;
    }

    public bool DoDamage(int Damage)
    {
        CurrentHealth -= Damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Kill();
        }
        else if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        return (CurrentHealth == 0);
    }

    public void Kill()
    {
        ExplosionPrefab = (GameObject)Resources.Load("VFX/Explosions/ExplosionBoom", typeof(GameObject));

        Instantiate(
            ExplosionPrefab,
            transform.position,
            Quaternion.identity
        );
        
        gameObject.SetActive(false);
    }
}
