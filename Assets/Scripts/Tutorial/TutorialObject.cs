using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialObject : MonoBehaviour
{
    public UnityEvent triggerEnter = new UnityEvent();
    [SerializeField] private string _tagToCheckFor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == _tagToCheckFor)
        {           
            // in this event, pass in the number of the tutorial phase
            // you want to activate
            triggerEnter.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == _tagToCheckFor)
        {
            // in this event, pass in the number of the tutorial phase
            // you want to activate
            triggerEnter.Invoke();
            gameObject.SetActive(false);
        }
    }
}
