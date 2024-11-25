using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float targetRangeY;

    //Cache the player's Transform component
    private Transform playerPos;
    
    private void OnEnable() {
        //Find the player on the scene, and get its Transform component
        playerPos = FindObjectOfType<PlayerMove>().GetComponent<Transform>();
    }

    private void FixedUpdate() {
        //If the player is in the scene
        if (playerPos) {
            //If the player is on the above the Mantis, set the directionY to 1. Else if they are below, set to -1;
            int directionY = transform.position.y < playerPos.position.y ? 1 : -1;

            //If the player is on the left side of the Mantis, set the directionX to -1. Else, set to 1;
            int directionX = transform.position.x > playerPos.position.x ? -1 : 1;

            Vector2 distanceFromPlayer = playerPos.position - transform.position;

            //If the player is further away from the Mantis on the X-axis
            if (Mathf.Abs(distanceFromPlayer.x) > Mathf.Abs(distanceFromPlayer.y)) {

                //Horizontally move the Mantis toward the player
                transform.Translate(new Vector2(directionX, 0) * movementSpeed * Time.deltaTime);
            }
            else 
            {
                //Vertically move the Mantis toward the player
                transform.Translate(new Vector2(0, directionY) * movementSpeed * Time.deltaTime);
            }
        }
    }

}