using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [Header("Enemy Information")]
    [SerializeField] protected float health;

    public float GetHP(){
        return health;
    }

    public void TakeDamage(float damage){
        health-=damage;
    }
}
