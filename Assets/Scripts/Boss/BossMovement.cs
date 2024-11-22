using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    [SerializeField] private float targetRangeY;
    private Transform playerPos;

    
    private void OnEnable() {
        playerPos = FindObjectOfType<PlayerMove>().GetComponent<Transform>();
    }

    private void Update() {
        if (playerPos) {
            int directionY = transform.position.y < playerPos.position.y ? 1 : -1;
            int directionX = transform.position.x > playerPos.position.x ? -1 : 1;

            if (transform.position.y >= playerPos.position.y - targetRangeY && transform.position.y <= playerPos.position.y + targetRangeY) {
                transform.Translate(new Vector2(directionX, 0) * movementSpeed * Time.deltaTime);
            } else {
                transform.Translate(new Vector2(0, directionY) * movementSpeed * Time.deltaTime);
            }
        }
    }

}
