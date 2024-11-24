using System.Linq.Expressions;
using UnityEngine;

/*
    Base class for all enemies
*/
public class EnemyWander : MonoBehaviour
{
    [Header("Set to 'walls'")]
    [SerializeField] protected LayerMask _enemyMask;
    
    [Header("Delay Before moving again")]
    [SerializeField] protected float delay;

    [Header("Enemy Wander move speed")]
    [SerializeField] protected float moveSpeed;
    protected float timer;
    protected RaycastHit2D line1, line2, line3, line4;
    protected Vector2 yCords;
    protected Vector2 target;
    protected Vector2 currV;
    protected Transform enemySprite, enemyWeapon;

    // Gets transform for rotating it to avoid using more sprites
    protected void GetTransforms(){
        enemySprite = GetComponent<Transform>();
    }

    // Simple Timer that returns true when delay has been Reached
    protected bool Timer(){
        timer+=Time.deltaTime;
        if(timer > delay){
            timer = 0;
            return true;
        } else return false;
    }

    // Moves the player to targetted position
    protected int Move(){
        transform.position = Vector2.SmoothDamp(transform.position, target, ref currV, Time.deltaTime, moveSpeed);

        transform.rotation = target.x > transform.position.x ? new Quaternion(0, 180f, 0, 0): 
                                                               new Quaternion(0, 0, 0, 0);
        // Once the enemy has reached the Targetted position 
        // They will remain there until the timer has been run and cleared
        if(new Vector2(transform.position.x, transform.position.y) == target){
            if(Timer()) FindValidLocation();
            
            // Returns 0 Which is idle state for animation controller
            return 0;
        }

        // Returns 1 which is walking state for animation controller
        return 1;
    }

    // Casts 4 raycast in 4 different directions to find dimansions of room to select a valid target position
    protected void FindValidLocation(){
  
        line1 = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity ,_enemyMask);
        line2 = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity ,_enemyMask);
        line3 = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity ,_enemyMask);
        line4 = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity ,_enemyMask);
        
        // Sets target location
        target = new(Random.Range(line3.point.x, line4.point.x), 
                     Random.Range(line1.point.y, line2.point.y));
    }
    
    // Finds a new valid loocation to travel to  if enemy collides with a wall
    protected void OnCollisionStay2D(Collision2D collision){
        if(collision.transform.name == "Walls"){
           FindValidLocation();
        }
    }

    protected void OnTriggerExit2D(Collider2D collider){
        if(collider.transform.name == "Doors"){
           FindValidLocation();
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (line1.point != null) ? line1.point : new(0, 0));
        Gizmos.DrawLine(transform.position, (line2.point != null) ? line2.point : new(0, 0));
        Gizmos.DrawLine(transform.position, (line3.point != null) ? line3.point : new(0, 0));
        Gizmos.DrawLine(transform.position, (line4.point != null) ? line4.point : new(0, 0));
    }

    
}
