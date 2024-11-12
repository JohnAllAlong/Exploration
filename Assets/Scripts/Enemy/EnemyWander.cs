using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    [SerializeField] protected LayerMask _enemyMask;
    [SerializeField] protected float circleRadius;
    protected RaycastHit2D line1, line2, line3, line4;
    protected Vector2 yCords;
    private Vector2 target;
    private Vector2 currV;

    void Start(){
        FindRoomSize();
        Target();
    }

    void Update(){
         if (new Vector2(transform.position.x, transform.position.y) == target){
            Debug.Log("Target Reached");
            Target();
        }
        FindRoomSize();
    }

    protected void Target(){
        target = new(Random.Range(line3.point.x+circleRadius, line4.point.x-circleRadius), 
                     Random.Range(line1.point.y-circleRadius, line2.point.y+circleRadius));
        
        Debug.Log($"Target location {target}");
    }

    protected void FindRoomSize(){
        line1 = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity ,_enemyMask);
        line2 = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity ,_enemyMask);
        line3 = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity ,_enemyMask);
        line4 = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity ,_enemyMask);
        transform.position = Vector2.SmoothDamp(transform.position, target, ref currV, Time.deltaTime, 5f);
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (line1.point != null) ? line1.point : new(0, 0));
        Gizmos.DrawLine(transform.position, (line2.point != null) ? line2.point : new(0, 0));
        Gizmos.DrawLine(transform.position, (line3.point != null) ? line3.point : new(0, 0));
        Gizmos.DrawLine(transform.position, (line4.point != null) ? line4.point : new(0, 0));
    }
}
