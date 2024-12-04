using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;


    private const float MAX_LIFETIME = 5f;

    private Vector2 targetDirection;

    public void MoveTowards(Vector2 targetDirection) {
        this.targetDirection = targetDirection;
    }

    private void Update() {
        transform.Translate(targetDirection * projectileSpeed * Time.deltaTime);

        Destroy(gameObject, MAX_LIFETIME);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            //Apply damage to the player
            Destroy(gameObject);
        }
    }
}
