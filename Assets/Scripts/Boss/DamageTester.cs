using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTester : MonoBehaviour
{
    [SerializeField] private Damageable entityDamageable;
    [SerializeField] private int damageAmount;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.O)) {
            entityDamageable.TakeDamage(damageAmount);
        }
    }
}
