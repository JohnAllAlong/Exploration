using UnityEngine;

public class BossStateHandler : MonoBehaviour
{
    private float attackingRange = 3f;

    protected enum bossState {
        Chasing,
        Attacking1,
        Attacking2,
        Attacking3,
        Death
    }

    protected bossState currentState;

    //Cache the player's Transform component
    protected Transform playerPos;
    
    protected void OnEnable() {
        //Find the player on the scene, and get its Transform component
        playerPos = FindObjectOfType<PlayerMove>().GetComponent<Transform>();
        currentState = bossState.Chasing;
    }

    private void Update() {
        if ((playerPos.position - transform.position).magnitude <= attackingRange) {
            currentState = bossState.Attacking1;
        } else {
            currentState = bossState.Chasing;
        }
    }
}
