using UnityEngine;

public class StaffSlam : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Player")) {
            other.GetComponent<Damageable>().TakeDamage(damage);
            Debug.Log("Slammed Player");
        }
    }
}
