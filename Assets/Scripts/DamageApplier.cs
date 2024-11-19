using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageApplier : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private string targetTag;
    private float _endTime;
    private BoxCollider2D _boxCollider;

    void Start()
    {

        _boxCollider = GetComponent<BoxCollider2D>();
        _endTime = Time.time + lifetime;
    }

    void Update()
    {
        /*For projectiles.
         * if (Time.time > _endTime)
        {
            Destroy(this.gameObject);
        }*/
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (targetTag == null || other.gameObject.CompareTag(targetTag))
        {
            Damageable damageable = other.gameObject.GetComponentInChildren<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                //For projectiles.
                //Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetTag == null || collision.gameObject.CompareTag(targetTag))
        {
            Damageable damageable = collision.gameObject.GetComponentInChildren<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                //For projectiles.
                //Destroy(this.gameObject);
            }
        }
    }
}
