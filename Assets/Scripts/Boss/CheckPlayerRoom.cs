using UnityEngine;
using UnityEngine.Events;

public class CheckPlayerRoom : MonoBehaviour
{
    [SerializeField] private UnityEvent playerEntersRoom;
    [SerializeField] private UnityEvent playerLeavesRoom;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log($"Player entered the [{name}] room");
            playerEntersRoom.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log($"Player exited the [{name}] room");
            playerLeavesRoom.Invoke();
        }
    }
}
