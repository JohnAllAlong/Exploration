using UnityEngine;

public class BossStateHandler : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private GameObject areaAttackPrefab;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float destroyAttackTime;

    private float attackingRange = 3f;

    protected enum bossState {
        Patrolling,
        Chasing,
        Attacking1,
        Attacking2,
        Attacking3,
        Death
    }

    protected bossState currentState;

    //Cache the player's Transform component
    protected Transform playerPos;
    
    private float currentTimer;

    protected virtual void OnEnable() {
        //Find the player on the scene, and get its Transform component
        playerPos = FindObjectOfType<PlayerMove>().GetComponent<Transform>();
        currentState = bossState.Chasing;
        currentTimer = 0f;
    }

    private void FixedUpdate() {
        //CHASING
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

    private void Update() {
        if ((playerPos.position - transform.position).magnitude <= attackingRange) {
            currentState = bossState.Attacking1;
        } else {
            currentState = bossState.Chasing;
        }

        //ATTACKING
        if (playerPos && currentState == bossState.Attacking1) {
            currentTimer += Time.deltaTime * attackSpeed;

            if (currentState == bossState.Attacking1 && currentTimer >= 1f) {
                Quaternion toPlayer = new Quaternion();
                toPlayer = Quaternion.FromToRotation(Vector3.up, (playerPos.position - transform.position).normalized);

                GameObject areaAttack = Instantiate(areaAttackPrefab, playerPos.position, toPlayer);

                Destroy(areaAttack, destroyAttackTime);
                currentTimer = 0f;
            }        
        }
    }

    protected bossState RetrieveState() {
        bossState theState = new bossState();
        theState = currentState;
        return theState;
    }
}
