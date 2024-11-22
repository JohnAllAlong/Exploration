using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Player Collision Settings:")]
    [SerializeField] protected float alertRadius, delay;
    [SerializeField] protected LayerMask _enemyMask;
    [SerializeField] protected bool chase, overrideChase;
    [SerializeField] protected Transform playerPos;
    [SerializeField] protected float chaseSpeed;
    protected float distFromPlayer, target;
    private EnemyanimatorController EAC;
    public int enemyId;

    //please link this to death when its added santiago - aiden
    public bool isAlive = true; //true if alive false if dead
    
    protected float timer;
    protected Color col;
    protected RaycastHit2D alertRange;
    protected Vector2 currV;
    protected bool backUp = false;

    private void Awake(){
        overrideChase = false;
        if(playerPos == null){
            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        //easy anti cheat?!?!?!!? no way?!?!?!?
        EAC = GetComponentInChildren<EnemyanimatorController>();
    }

    public bool GetChaseState(){
        return chase;
    }

    public void OverrideChaseState(bool state){
        overrideChase = state;
    }
    
    public void PlayerDetected(){
        alertRange = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y),
                                            alertRadius, Vector2.zero, 0f, _enemyMask);
        
        if(!overrideChase){    
            if(alertRange.collider!=null){
                chase = CountDownToChase();
            }else{
                timer = 0;
                chase = false;
            }
        }else{
            chase = true;
        }
    }

    public bool CountDownToChase(){
        timer+=Time.deltaTime;
        if(timer > delay){
            
            return true;
        }
        return false;
    }

    public void Chase(){
        PlayerDetected();

        transform.rotation = playerPos.position.x > transform.position.x ? new Quaternion(0, 180f, 0, 0) : 
                                                                        new Quaternion(0, 0, 0, 0);

        if(!overrideChase){
            distFromPlayer = alertRadius <= 1 ? 0.8f : 0.3f+(0.5f*alertRadius);

            if(Vector2.Distance(transform.position, playerPos.position) <  1.5f){
                target = alertRange.point.x < transform.position.x ? alertRange.point.x + distFromPlayer: 
                                                                 alertRange.point.x - distFromPlayer;
            }else{
                EAC.playAlert();
                target = alertRange.point.x;
            }
            
        }else{
            if(Vector2.Distance(transform.position, playerPos.position) < 2){
                overrideChase = false;
            }else{
                EAC.playAlert();
            }
            target = playerPos.position.x;
        }     

        transform.position = Vector2.SmoothDamp(
                                                transform.position, 
                                                new Vector2(target, playerPos.position.y),
                                                ref currV, Time.deltaTime, chaseSpeed
                                                );
    }


    //Visualizes circle cast
    protected void OnDrawGizmos(){
        col = alertRange.collider!=null ? Color.red: Color.blue;
        Gizmos.color = col;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
}
