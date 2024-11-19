using Unity.VisualScripting;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Player Collision Settings:")]
    [SerializeField] protected float alertRadius, delay;
    [SerializeField] protected LayerMask _enemyMask;
    [SerializeField] protected bool chase, overrideChase;
    [SerializeField] protected float distFromPlayer;
    protected float timer;
    protected Color col;
    protected RaycastHit2D alertRange;
    protected Vector2 currV;

    [SerializeField]
    protected Transform playerPos;

    private void Awake(){
        overrideChase = false;
        if (playerPos == null){
            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }

    public bool GetChaseState(){
        return chase;
    }

    public void OverrideChaseState(bool state = false){
        overrideChase = state;
    }
    
    public void PlayerDetected(){
        alertRange = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y),
                                            alertRadius, Vector2.down, 0f, _enemyMask);
        
        if(!overrideChase){    
            if(alertRange.collider!=null){
                CountDownToChase();
            }else{
                timer = 0;
                chase = false;
            }
        }else{
            chase = true;
        }
    }

    public void CountDownToChase(){
        timer+=Time.deltaTime;
        if(timer > delay){
            chase = true;
        }
    }

    public void Chase(){
        PlayerDetected();
        float target;

        transform.rotation = alertRange.point.x > transform.position.x ? new Quaternion(0, 0, 180f, 0) : 
                                                                        new Quaternion(0, 0, 0, 0);

        if(!overrideChase){
            target = alertRange.point.x < transform.position.x ? alertRange.point.x + distFromPlayer: 
                                                                 alertRange.point.x - distFromPlayer;
        }else{
            if(Vector2.Distance(transform.position, playerPos.position) < 2){
                overrideChase = false;
            }
            Debug.Log("over");
            target = alertRange.point.x < transform.position.x ? playerPos.position.x + distFromPlayer: 
                                                                 playerPos.position.x - distFromPlayer;
        }

        transform.position = Vector2.SmoothDamp(
                                                transform.position, 
                                                new Vector2(target, playerPos.position.y),
                                                ref currV, Time.deltaTime, 5f
                                                );
    }

    //Visualizes circle cast
    void OnDrawGizmos(){
        col = alertRange.collider!=null ? Color.red: Color.blue;
        Gizmos.color = col;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
}
