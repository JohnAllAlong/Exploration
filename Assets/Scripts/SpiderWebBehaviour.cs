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
    public bool isTrapped;
    private float pullForce;
    [SerializeField]
    private float maxDistance = 2.0f;
    private float distance;
    public float pullScaler = 0.01f;
    public float stretchScaler = 0.20f;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        isTrapped = false;
        defaultScale = transform.localScale;
        lineRenderer = GetComponent<LineRenderer>();
        victim = GameObject.FindGameObjectWithTag("Player");
        movement = victim.GetComponent<PlayerMove>();
        defaultSpeed = movement._moveSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        if (movement != null)
        {
            if (isTrapped)
            {
                PlayerTrapped();
                StretchWeb();
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTrapped = true;
        }
    }

    public void PlayerTrapped()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, victim.transform.position);
        distance = Vector3.Distance(transform.position, victim.transform.position);
        pullForce = Mathf.Max(0.01f, maxDistance - distance);
        movement._moveSpeed = pullForce;
    }

    public void WebDestroyed()
    {
        isTrapped = false;
        movement._moveSpeed = defaultSpeed;
        transform.localScale = defaultScale;
    }

    public void StretchWeb()
    {
        transform.localScale = defaultScale + defaultScale * distance * -stretchScaler;
    }

}
