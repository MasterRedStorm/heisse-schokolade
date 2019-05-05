using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider = null;
    [SerializeField] public int MaxHealth = 100;
    [SerializeField] public int CurrentHealth = 0;
    [SerializeField] private GameObject ExplosionPrefab; 
    
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
        if (HealthSlider == null)
            return;
        
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
        Instantiate(
            ExplosionPrefab,
            transform.position,
            Quaternion.identity
        );

        var audioSource = GetComponent<AudioSource>();
        audioSource?.Play();

        gameObject.SetActive(false);
    }
}
