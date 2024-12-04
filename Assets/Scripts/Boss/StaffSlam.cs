using UnityEngine;

public class StaffSlam : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Player")) {
            //Apply damage to the player
            Debug.Log("Slammed Player");
        }
    }
}
