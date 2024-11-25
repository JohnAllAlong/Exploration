using UnityEngine;

public class BossStateHandler : MonoBehaviour
{
    protected enum bossState {
        Chasing,
        Attacking1,
        Attacking2,
        Attacking3,
        Death
    }

    protected bossState currentState;

    private void OnEnable() {
        currentState = bossState.Chasing;
    }
}
