using UnityEngine;

public class BossStateHandler : MonoBehaviour
{
    [SerializeField] private Vector2 movementSpeed;
    [SerializeField] private GameObject bookAimPrefab;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float destroyAttackTime;

    private float attackingRange = 3f;

    [SerializeField] private float centerGizmoSize;
    [SerializeField] private Vector3 mantisOffsetCenter;
    [SerializeField] private Vector3 playerOffsetCenter;

    protected enum bossState {
        Patrolling,
        Chasing,
        StaffSlam,
        ThrowBook,
        Jump,
        Death
    }

    protected bossState currentState;
    private Animator mantisAnimator;

    //Cache the player's Transform component
    protected Transform playerPos;
    
    private float currentTimer;

    protected virtual void OnEnable() {
        //Find the player on the scene, and get its Transform component
        playerPos = FindObjectOfType<PlayerMove>().GetComponent<Transform>();
        mantisAnimator = GetComponentInChildren<Animator>();
        currentState = bossState.Chasing;
        currentTimer = 0f;
    }

    private void FixedUpdate() {
    /*
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
        */
    }

    private void Update() {
        currentTimer += Time.deltaTime * attackSpeed;

        if (currentTimer >= 1f) {
            ChooseRandomAttack();
            Debug.Log(currentState);
            currentTimer = 0f;
        }


/*
        if ((playerPos.position - transform.position).magnitude <= attackingRange) {
            currentState = bossState.StaffSlam;
        } else {
            currentState = bossState.Chasing;
        }*/


/**
        //STAFF SLAMMMMMMMMM
        if (playerPos && currentState == bossState.StaffSlam) {
            if (currentTimer >= 1f) {
                mantisAnimator.SetTrigger("Slam");
                
                Quaternion toPlayer = new Quaternion();
                toPlayer = Quaternion.FromToRotation(Vector3.up, (playerPos.position + playerOffsetCenter - (transform.position + mantisOffsetCenter)).normalized);
                Debug.Log(toPlayer.eulerAngles);


                GameObject areaAttack = Instantiate(areaAttackPrefab, transform.position + mantisOffsetCenter, toPlayer*areaAttackPrefab.transform.rotation);
                

                Destroy(areaAttack, destroyAttackTime);
                
                currentTimer = 0f;
            }  
            
        }*/
    }

    protected bossState RetrieveState() {
        bossState theState = new bossState();
        theState = currentState;
        return theState;
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + mantisOffsetCenter, centerGizmoSize);

        if (playerPos)
            Gizmos.DrawWireSphere(playerPos.position + playerOffsetCenter, centerGizmoSize);
    }

    private void ChooseRandomAttack() {
        int numAttacks = Random.Range(0, 3);

        switch (numAttacks) {
            case 0:
                currentState = bossState.Jump;
                mantisAnimator.SetTrigger("Jump");
                break;
            case 1:
                currentState = bossState.StaffSlam;
                mantisAnimator.SetTrigger("Slam");
                break;
            case 2:
                currentState = bossState.ThrowBook;
                mantisAnimator.SetTrigger("Throw");
                break;
        }
    }
}
