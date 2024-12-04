using UnityEngine;

public class Spotlights : MonoBehaviour
{   
    [SerializeField] protected float alertRadius;
    [SerializeField] protected ContactFilter2D contactFilter2D;
    protected RaycastHit2D alertRange;
    [SerializeField] protected RaycastHit2D[] results = new RaycastHit2D[20];

    protected int hits;

    protected void OnTriggerEnter2D(Collider2D collider2D){
        if (results.Length == 0) return;

        foreach(var r in results){
            if(r.collider!=null){
                Debug.Log(r.collider.name);
                r.collider.GetComponent<EnemyChase>().OverrideChaseState(true);
            }
        }
    }
    

    public void Update(){
        results = new RaycastHit2D[20];
        hits = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y),
                                            alertRadius, Vector2.zero, contactFilter2D, results);
    }

     void OnDrawGizmos(){
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
    }
}
