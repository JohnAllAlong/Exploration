using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDeath : MonoBehaviour
{
    [Header("Timer for explosion")]
    [SerializeField] protected float deathTimer = 1;
    void Start()
    {
       // Destroys explosion after X seconds
        Destroy(gameObject, deathTimer); 
    }
}
