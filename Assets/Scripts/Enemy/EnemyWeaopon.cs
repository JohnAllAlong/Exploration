using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaopon : MonoBehaviour
{
    protected void OnTriggerStay2D(Collider2D collider){
        Debug.Log(collider.transform.name);
    }
}
