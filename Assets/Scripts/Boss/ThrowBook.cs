
using UnityEngine;

public class ThrowBook : MonoBehaviour
{
    [SerializeField] GameObject bookPrefab;
    [SerializeField] Vector3 bookOffset;
    [SerializeField] float bookGizmoSize;

    private Transform playerPos;
    
    private void Awake() {
        playerPos = FindObjectOfType<PlayerMove>().GetComponent<Transform>();
    }

    public void ThrowBookTrigger() {

        GameObject bookProjectile = Instantiate(bookPrefab, transform.position + bookOffset, Quaternion.identity);
        Vector2 direction = (playerPos.position - (transform.position + bookOffset)).normalized;

        bookProjectile.GetComponent<BookProjectile>().MoveTowards(direction);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + bookOffset, bookGizmoSize);
    }
}
