using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleCollider : MonoBehaviour
{
    public UnityEvent enterTriggerEvent = new UnityEvent();
    public UnityEvent exitTriggerEvent = new UnityEvent();
    [SerializeField]
    private string tagToCompare = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagToCompare))
        {
            enterTriggerEvent.Invoke();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagToCompare))
        {
            exitTriggerEvent.Invoke();
        }
    }
}
