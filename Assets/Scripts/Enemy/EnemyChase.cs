using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;

/*
    Chase mechanics for all enemy Variants 
*/
public class EnemyChase : MonoBehaviour
{
    [Header("Player Collision Settings:")]
    [SerializeField] protected float alertRadius;
    [SerializeField] protected float delay;
    [SerializeField] protected LayerMask _enemyMask;

    [Header("Control Current state enemy state")]
    [SerializeField] protected bool chase;
    [SerializeField] protected bool overrideChase;

    [Header("Movement Speed")]
    [SerializeField] protected float chaseSpeed;

    [Header("Animation Controller")]
    [SerializeField] private EnemyanimatorController EAC;
    protected float distFromPlayer, target;

    //please link this to death when its added santiago - aiden
    [Header("Information for enemy tracking")]
    public int enemyId;
    public bool isAlive = true; //true if alive false if dead

    [Header("Chase Delay")]
    public float moveDelay;
    
    protected Transform playerPos;
    protected float timer, time;
    protected Color col;
    protected RaycastHit2D alertRange;
    protected Vector2 currV;
    protected bool disbaleMovement = false;

    /*
        Runs once on awake
    */
    private void Awake(){

        // Set to false by default to avoid enemies chasing player as soon as their spawned
        overrideChase = false;

        // Used for tracking player location in the event that chase override is called
        // Sets the player transform automatically if it was not set in editor
        if(playerPos == null){
            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        
        // Used to control the current state of animations
        // There are two differenet animation controllers in every enemy so this one has to be specified
        if(EAC == null){
            EAC = GetComponent<Transform>().GetChild(1).GetComponent<EnemyanimatorController>();
        }
    }

    // Returns wether or not the enemy is chasing player at the current moment
    public bool GetChaseState(){
        return chase;
    }

    // Overrides the wander state and causes enemy to begin tracking and chasing player
    // Will only disable once the player has been reached
    public void OverrideChaseState(bool state){
        overrideChase = state;
    }
    
    // This function allows enemies to create a circle cast that will detect players
    // Within it vicinity
    public void PlayerDetected(){

        // Creates Circle cast
        alertRange = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y),
                                            alertRadius, Vector2.zero, 0f, _enemyMask);
        
        // Controls wether or not the enemy begins chasing player
        if(!overrideChase){
            
            // If player has entered circle cast a countdown will begin 
            if(alertRange.collider!=null){

                // chase is set to the value returned by countdown
                // false is returned if countdown has not ended
                // true is returned once countdown is over and enemy will begin chasing plasyer
                chase = CountDownToChase();
            }else{

                // Resets timer and chase state
                timer = 0;
                chase = false;
            }
        }else{

            // Enables chase state if override is true
            chase = true;
        }
    }

    // Simple timer that counts up to given delay
    public bool CountDownToChase(){
        // Increments on Time.deltaTime 
        timer+=Time.deltaTime;
        if(timer > delay){

            // returns true once delay has been passed
            return true;
        }

        // Returns false by default
        return false;
    }

    // Allows the enemy to track and chase the player
    public int Chase(){
        
        // Calls player detected to see if player is in range and can be chased
        PlayerDetected();

        // Flips enemy on Y axis to face the player
        transform.rotation = playerPos.position.x > transform.position.x ? new Quaternion(0, 180f, 0, 0) : 
                                                                        new Quaternion(0, 0, 0, 0);

        /*
            When chasing the enemy has two possible targets:
            The player position of the player within the circle cast
            The position of the player in the world

            This controller iterates through both of these points depending on 
            whether or not ovveride chase was triggered 
        */
        if(!overrideChase){
            /*
                If the current chase state was enabled by timer the following will run
            */

            // Adjusts the area where the enemy will stop in front of the player on the X axis
            // This ensures that it will always be stop 0.2 units away from the player
            distFromPlayer = alertRadius <= 1 ? 0.9f : 0.4f+(0.5f*alertRadius);

            /*
                When the player is being chased, Enemy will at first head directly towards them
                When getting within 1.5 units of the player the target position on the X axis will be adjusted to
                Ensure the enemy is in front of the player and not overlapping them
            */
            if(Vector2.Distance(transform.position, playerPos.position) <  1.5f){
                // Adusts targeted X axis
                target = alertRange.point.x < transform.position.x ? alertRange.point.x + distFromPlayer: 
                                                                 alertRange.point.x - distFromPlayer;
            }else{
                // Plays Alert animation to show the player is being hunted
                EAC.PlayAlert();
                // Targets the players Exact position on the X axis
                target = alertRange.point.x;
            }
        }else{
            
            // Disables the Override once the distance between the player and enemy is less than 2 units
            if(Vector2.Distance(transform.position, playerPos.position) < 2){
                overrideChase = false;
            }else{

                // Plays Alert animation to show the player is being hunted
                EAC.PlayAlert();
            }

            // Sets target to players position outside of the valid detection radius
            target = playerPos.position.x;
        }     
        
        if(!disbaleMovement){
            // Moves the enemy towards the player
            transform.position = Vector2.SmoothDamp(transform.position, new Vector2(target, playerPos.position.y),
                                                        ref currV, Time.deltaTime, chaseSpeed);
        }else{
            return Timer();
        }
        
        // If the player has is within +-0.3 units of the player along the X axis
        // Returns 3 which is the attacking state in the animation controller
        if(Vector2.Distance(transform.position, new Vector2(target+0.2f, playerPos.position.y)) <  1f){
            return 3;
        }
        
        // Returns 1, which is the Walking state in the animaiton controller
        return 1;
    }

    protected void OnCollisionExit2D(Collision2D other){
        disbaleMovement = true;
    }

    protected int Timer(){
        time+=Time.deltaTime;
        if(time>moveDelay){
            time=0;
            disbaleMovement = false;
        }

        return 0;
    }

    //Visualizes circle cast
    protected void OnDrawGizmos(){
        col = alertRange.collider!=null ? Color.red: Color.blue;
        Gizmos.color = col;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
}
