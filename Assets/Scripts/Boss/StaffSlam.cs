using UnityEngine;

public class StaffSlam : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        //Apply damage to the player
        Debug.Log("Slammed Player");
    }
}
