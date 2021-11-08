using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// code monkey health system

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthChanged;

    private int health;
    private int healthMax;

    // constructor
    public HealthSystem(int hm)
    {
        this.healthMax = hm;
        health = hm;
    }

    public int GetHealth()
    {
        return health;
    }

    public float GetHealthPercent()
    {
        return (float)health / healthMax;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0)
            health = 0;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > healthMax)
            health = healthMax;
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }
}
