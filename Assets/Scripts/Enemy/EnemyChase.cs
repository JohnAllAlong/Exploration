
using System;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Player Collision Settings:")]
    [SerializeField] protected float alertRadius, delay;
    [SerializeField] protected LayerMask _enemyMask;
    [SerializeField] protected bool chase;
    protected float timer;
    protected Color col;
    protected RaycastHit2D alertRange;
    protected Vector2 currV;

    public bool GetChaseState(){
        return chase;
    }

    public void PlayerDetected(){
        alertRange = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y),
                                            alertRadius, Vector2.down, 0f, _enemyMask);
        
        if(alertRange.collider!=null){
            CountDownToChase();
        }else{
            timer = 0;
            chase = false;
        }
    }

    public void CountDownToChase(){
        timer+=Time.deltaTime;
        if(timer > delay){
            Debug.Log("Chasing");
            chase = true;
        }
    }

    public void Chase(){
        PlayerDetected();
        if(chase){
            transform.position = Vector2.SmoothDamp(transform.position, alertRange.point,
                                 ref currV, Time.deltaTime, 5f);
        }
    }

    //Visualizes circle cast
    void OnDrawGizmos(){
        col = alertRange.collider!=null ? Color.red: Color.blue;
        Gizmos.color = col;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
}
