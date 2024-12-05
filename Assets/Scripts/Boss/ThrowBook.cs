
using UnityEngine;

public class ThrowBook : MonoBehaviour
{
    [SerializeField] private GameObject bookPrefab;
    [SerializeField] private Vector3 bookOffset;
    [SerializeField] private float bookGizmoSize;
    [SerializeField] private float centerGizmoSize;
    
    
    [SerializeField] private GameObject aimArrowPrefab;
    [SerializeField] private Vector3 playerOffsetCenter;
    [SerializeField] private Vector3 mantisOffsetCenter;



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


        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position + mantisOffsetCenter, centerGizmoSize);

        if (playerPos)
            Gizmos.DrawWireSphere(playerPos.position + playerOffsetCenter, centerGizmoSize);
    }

    GameObject arrowAim;
    public void ShowArrow() {
        Quaternion toPlayer = new Quaternion();
        toPlayer = Quaternion.FromToRotation(Vector3.up, (playerPos.position + playerOffsetCenter - (transform.position + mantisOffsetCenter)).normalized);

        arrowAim = Instantiate(aimArrowPrefab, transform.position + mantisOffsetCenter, toPlayer*aimArrowPrefab.transform.rotation);
    }

    private void Update() {
        if (arrowAim) {
            arrowAim.transform.rotation = Quaternion.FromToRotation(Vector3.up, (playerPos.position + playerOffsetCenter - (transform.position + mantisOffsetCenter)).normalized)*aimArrowPrefab.transform.rotation;
            arrowAim.transform.parent = transform;
        }
    }

    public void HideArrow() {
        Destroy(arrowAim);
    }
}
