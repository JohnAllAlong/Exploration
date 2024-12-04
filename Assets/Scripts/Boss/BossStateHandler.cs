using UnityEngine;

public class BossStateHandler : MonoBehaviour
{
    [SerializeField] private Vector2 movementSpeed;
    [SerializeField] private float attackSpeed;

    protected enum bossState {
        Patrolling,
        Chasing,
        StaffSlam,
        ThrowBook,
        Jump,
        Death
    }

    protected bossState currentState;
    [SerializeField] private Animator mantisAnimator;
    [SerializeField] private Animator slamAnimator;

    //Cache the player's Transform component
    protected Transform playerPos;
    private Transform mantisPos;

    [SerializeField] private float slamDistance;
    
    private float currentTimer;

    protected virtual void Awake() {
        //Find the player on the scene, and get its Transform component
        playerPos = FindObjectOfType<PlayerMove>().GetComponent<Transform>();
        mantisPos = transform.parent;
        currentState = bossState.Chasing;
        currentTimer = 0f;
    }

    private void FixedUpdate() {
    
        //CHASING
        if (playerPos && currentState == bossState.Chasing) {
            //If the player is on the above the Mantis, set the directionY to 1. Else if they are below, set to -1;
            int directionY = mantisPos.position.y < playerPos.position.y ? 1 : -1;

            //If the player is on the left side of the Mantis, set the directionX to -1. Else, set to 1;
            int directionX = mantisPos.position.x > playerPos.position.x ? -1 : 1;

            Vector2 distanceFromPlayer = playerPos.position - mantisPos.position;

            //If the player is further away from the Mantis on the X-axis
            if (Mathf.Abs(distanceFromPlayer.x) > Mathf.Abs(distanceFromPlayer.y)) {

                //Horizontally move the Mantis toward the player
                mantisPos.Translate(new Vector2(directionX, 0) * movementSpeed * Time.deltaTime);
            }
            else 
            {
                //Vertically move the Mantis toward the player
                mantisPos.Translate(new Vector2(0, directionY) * movementSpeed * Time.deltaTime);
            }
        }

    }

    private void Update() {
        currentTimer += Time.deltaTime * attackSpeed;

        if (currentTimer >= 1f) {
            ChooseRandomAttack();
            currentTimer = 0f;
        }


    }

    protected bossState RetrieveState() {
        bossState theState = new bossState();
        theState = currentState;
        return theState;
    }


    private void ChooseRandomAttack() {
        int numAttacks = Random.Range(0, 2);

        switch (numAttacks) {

            //Option 1: If the player is close enough, will slam. Otherwise, close the gap
            case 0:
                float distanceFromPlayer = (playerPos.position - mantisPos.position).magnitude;
                
                if (distanceFromPlayer <= slamDistance) {
                    currentState = bossState.StaffSlam;
                    mantisAnimator.SetTrigger("Slam");
                    slamAnimator.SetTrigger("Slam");
                } else { 
                    currentState = bossState.Jump;
                    mantisAnimator.SetTrigger("Jump");
                }

                break;

            //Option 2: Throw a book at the player
            case 1:
                currentState = bossState.ThrowBook;
                mantisAnimator.SetTrigger("Throw");

                break;
        }
    }

    public void ChangeToChasing() {
        currentState = bossState.Chasing;
    }
}
