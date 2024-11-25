using UnityEngine;

public class EnemyWeaopon : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D collider){
        Debug.Log(collider.transform.name);
    }
}
