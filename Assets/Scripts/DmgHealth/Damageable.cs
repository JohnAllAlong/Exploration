﻿using Saving;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Represents a damageable entity with health.
public class Damageable : MonoBehaviour
{
    [Header("Enemy Health")]
    [SerializeField] 
    private float Maxhealth = 1.0f; // Maximum health.
    [SerializeField]
    private float currentHealth; // Current health.

    [SerializeField]
    private float maxCooldownTime = 0.5f;
    private float timeElapsed = 0.0f;

    // Events triggered on health change and death.
    public UnityEvent onHealthChanged = new UnityEvent();
    public UnityEvent onDeath = new UnityEvent();

    //private GameObject healthBarObject; // Health bar game object.
    //private Image healthBar; // Health bar image component.

    public void Start()
    {
        //healthBar = healthBarObject.GetComponent<Image>();
        currentHealth = Maxhealth;
    }

    // Reduces health and updates the health bar.
    public void TakeDamage(int damage)
    {
        if (timeElapsed >= maxCooldownTime)
        {
            currentHealth -= damage;
            //healthBar.fillAmount = health / Maxhealth;
            onHealthChanged?.Invoke();
            print("damage taken " + gameObject.name);
            timeElapsed = 0.0f;  


            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public float GetHP(){
        return currentHealth;
    }

    public void Update()
    {
        timeElapsed += Time.deltaTime;
    }

    // Handles the entity's death.
    protected virtual void Die()
    {
        onDeath?.Invoke();               

        // Check if the thing dying is the player or not
        // if so, play the player death animation
        if(this.gameObject.tag == "Player")
        {
            this.gameObject.GetComponentInChildren<PlayerAnimation>().SetState(5);
            new Timer(2).OnEnd(() => {
                SaveFramework.DestroySaveData();
                SceneManager.LoadScene("Title"); 
            }).StartTimer();
        }
        // Is this a bad way to do death animations for the player?
        // Probably, but i don't have time to do a cleaner implementation
    }
}