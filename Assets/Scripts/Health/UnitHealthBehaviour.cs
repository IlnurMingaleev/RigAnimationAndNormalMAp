using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitHealthBehaviour : MonoBehaviour
{
    [Header("Health Info")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;


    [Header("Events")]
    public UnityEvent healthIsZeroEvent;

    public delegate void HealthChangedAction(int healthAmount);
    public event HealthChangedAction HealthChangedEvent;

    public void TakeDamage(int damage) 
    {
        currentHealth -= damage;

        if (currentHealth <= 0) 
        {
            currentHealth = 0;
            HealthIsZeroEvent();
        }
        DelegateEventHealthChanged();
        
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth >= maxHealth) 
        {
            currentHealth = maxHealth;
        }

        DelegateEventHealthChanged();
    }

    public void HealthIsZeroEvent() 
    {
        healthIsZeroEvent.Invoke();    

    }

    public int GetCurrentHealth() 
    {
        return currentHealth;
    }

    public void DelegateEventHealthChanged() 
    {
        if (HealthChangedEvent != null) 
        {
            HealthChangedEvent(currentHealth);
        }
    }

    public int GetMaxHealth() 
    {
        return maxHealth;
    }
}
