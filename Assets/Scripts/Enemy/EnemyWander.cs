using System.Linq.Expressions;
using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    [SerializeField] protected LayerMask _enemyMask;
    [SerializeField] protected float delay;
    [SerializeField] protected float moveSpeed;
    protected float timer;
    protected RaycastHit2D line1, line2, line3, line4;
    protected Vector2 yCords;
    protected Vector2 target;
    protected Vector2 currV;
    protected Transform enemySprite, enemyWeapon;

    protected void GetTransforms(){
        enemySprite = GetComponent<Transform>();
    }

    protected bool Timer(){
        timer+=Time.deltaTime;
        if(timer > delay){
            timer = 0;
            return true;
        } else return false;
    }

    protected void Move(){ 
        transform.position = Vector2.SmoothDamp(transform.position, target, ref currV, Time.deltaTime, moveSpeed);
        if(new Vector2(transform.position.x, transform.position.y) == target){
            if(Timer()) FindValidLocation();
        }
    }

    protected void FindValidLocation(){
  
        line1 = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity ,_enemyMask);
        line2 = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity ,_enemyMask);
        line3 = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity ,_enemyMask);
        line4 = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity ,_enemyMask);
        
        target = new(Random.Range(line3.point.x, line4.point.x), 
                     Random.Range(line1.point.y, line2.point.y));

        transform.rotation = target.x > transform.position.x ? new Quaternion(0, 0, 180f, 0): 
                                                               new Quaternion(0, 0, 0, 0);
    }
    
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
