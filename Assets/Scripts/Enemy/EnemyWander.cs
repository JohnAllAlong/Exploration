using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    [SerializeField] protected LayerMask _enemyMask;
    protected RaycastHit2D line1, line2, line3, line4;
    protected Vector2 yCords;
    private Vector2 target;

    void Start(){
        FindRoomSize();
        Target();
    }

    void Update()
    {
        FindRoomSize();
    }

    protected void Target(){
        target = new(Random.Range(line3.point.x, line4.point.x), Random.Range(line1.point.y, line2.point.y));
        
        Debug.Log($"Target location x: {target.x}, y: {target.y}");
    }

    protected void FindRoomSize(){
        line1 = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity ,_enemyMask);
        line2 = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity ,_enemyMask);
        line3 = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity ,_enemyMask);
        line4 = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity ,_enemyMask);
        transform.position = Vector2.Lerp(transform.position, target, Time.deltaTime*2);
        
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (line1.point != null) ? line1.point : new(0, 0));
        Gizmos.DrawLine(transform.position, (line2.point != null) ? line2.point : new(0, 0));
        Gizmos.DrawLine(transform.position, (line3.point != null) ? line3.point : new(0, 0));
        Gizmos.DrawLine(transform.position, (line4.point != null) ? line4.point : new(0, 0));
    }
}
