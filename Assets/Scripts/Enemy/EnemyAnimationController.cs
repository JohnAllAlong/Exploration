using UnityEngine;

public class EnemyanimatorController : MonoBehaviour
{
    private Animator animator;

    protected void Awake(){
        animator = GetComponentInChildren<Animator>();
    }

    public void playAlert(){
        animator.SetTrigger("Alert");
    }
}
