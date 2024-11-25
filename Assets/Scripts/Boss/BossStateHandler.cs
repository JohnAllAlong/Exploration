using UnityEngine;

public class BossStateHandler : MonoBehaviour
{
    private enum bossState {
        Chasing,
        Attacking1,
        Attacking2,
        Attacking3,
        Death
    }

    [SerializeField] private bossState currentState;

    private void OnEnable() {
        currentState = bossState.Chasing;
    }   

    
}
