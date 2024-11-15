using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebBehaviour : MonoBehaviour
{
    private GameObject victim;
    private PlayerMove movement;
    private float defaultSpeed;
    private Vector3 defaultScale;
    [SerializeField]
    private bool isTrapped;
    private float pullForce;
    private float maxDistance = 2.0f;
    private float distance;
    public float pullScaler = 0.01f;
    public float stretchScaler = 0.25f;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        isTrapped = false;
        defaultScale = transform.localScale;
        lineRenderer = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (movement != null)
        {
            if (isTrapped)
            {
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, victim.transform.position);
                distance = Vector3.Distance(transform.position, victim.transform.position);
                pullForce = Mathf.Max(0.01f, maxDistance - distance);
                movement._moveSpeed = pullForce;
                StretchWeb();
                
            }
            else
            {
                movement._moveSpeed = defaultSpeed;
                transform.localScale = defaultScale;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            victim = collision.gameObject;
            movement = collision.GetComponent<PlayerMove>();
            defaultSpeed = movement._moveSpeed;
            isTrapped = true;
        }
    }

    public void StretchWeb()
    {
        if (isTrapped)
        {
            transform.localScale = new Vector3(defaultScale.x + Mathf.Abs(victim.transform.position.x - transform.position.x) * stretchScaler, defaultScale.y + Mathf.Abs(victim.transform.position.y - transform.position.y) * stretchScaler, 0);
        }
    }

}
