using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;

    private Vector2 targetDirection;

    public void MoveTowards(Vector2 targetDirection) {
        this.targetDirection = targetDirection;
    }

    private void Update() {
        transform.Translate(targetDirection * projectileSpeed * Time.deltaTime);
    }
}
