using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageApplier : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.0f;
    [SerializeField] private int damage = 1;
    [SerializeField] private string targetTag;
    [SerializeField] private bool isProjectile = false;
    private float _endTime;
    private Collider2D _collider;


    void Start()
    {

        _collider = GetComponent<Collider2D>();
        _endTime = Time.time + lifetime;
    }

    void Update()
    {
          if (isProjectile && Time.time > _endTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            Damageable damageable = other.gameObject.GetComponentInChildren<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                if (isProjectile)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            Damageable damageable = collision.gameObject.GetComponentInChildren<Damageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                if (isProjectile)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
