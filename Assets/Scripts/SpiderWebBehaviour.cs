using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWebBehaviour : MonoBehaviour
{
    private GameObject victim;
    private PlayerMove movement;
    private bool isTrapped = false;
    private float pullForce;
    private float maxDistance = 2.0f;
    private float distance;
    public float pullScaler = 0.01f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        

        if (isTrapped && movement != null)
        {
            distance = Vector3.Distance(transform.position, victim.transform.position);
            pullForce = Mathf.Max(0, maxDistance - distance);
            movement._moveSpeed = pullForce;
            if (movement._moveSpeed == 0)
            {
                victim.transform.position -= transform.position * pullScaler;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        victim = collision.gameObject;
        movement = collision.GetComponent<PlayerMove>();
        isTrapped = true;
    }
}
