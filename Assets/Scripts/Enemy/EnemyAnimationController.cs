using UnityEngine;

public class EnemyanimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    protected void Awake(){
        if(animator == null){
            animator = GetComponentInChildren<Animator>();
        }    
    }

    public void PlayAlert(){
        animator.SetTrigger("Alert");
    }

    public void PlayState(int state){
        animator.SetInteger("State", state);
    }
}
