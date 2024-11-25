using UnityEngine;

public class BossMovement : BossStateHandler
{
    [SerializeField] private float movementSpeed;

    private void FixedUpdate() {
        //If the player is in the scene
        if (playerPos && currentState == bossState.Chasing) {
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
