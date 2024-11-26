using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Represents a damageable entity with health.
public class Damageable : MonoBehaviour
{
    [Header("Enemy Health")]
    [SerializeField] private float Maxhealth = 1f; // Maximum health.
    [SerializeField]
    private float health; // Current health.

    [SerializeField]
    private float maxCooldownTime = 1.0f;
    private float timeElapsed = 0.0f;

    // Events triggered on health change and death.
    public UnityEvent onHealthChanged = new UnityEvent();
    public UnityEvent onDeath = new UnityEvent();

    //private GameObject healthBarObject; // Health bar game object.
    //private Image healthBar; // Health bar image component.

    public void Start()
    {
        //healthBar = healthBarObject.GetComponent<Image>();
        health = Maxhealth;
    }

    // Reduces health and updates the health bar.
    public void TakeDamage(int damage)
    {
        if (timeElapsed >= maxCooldownTime)
        {
            health -= damage;
            //healthBar.fillAmount = health / Maxhealth;
            onHealthChanged?.Invoke();
            print("damage taken " + gameObject.name);
            timeElapsed = 0.0f;

            if (health <= 0)
            {
                Die();
            }
        }
    }

    public float GetHP(){
        return health;
    }

    public void Update()
    {
        timeElapsed += Time.deltaTime;
    }

    // Handles the entity's death.
    protected virtual void Die()
    {
        onDeath?.Invoke();
        //Destroy(this.gameObject);
    }
}