using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapdoorTeleport : MonoBehaviour
{
    [SerializeField]
    private GameObject targetDoor;
    private static bool canTravel = true;
    private static bool interactWasPressed = false;
    private bool startCounting = false;

    private float defaultTime = 0.5f;
    private float delayTimer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            startCounting = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) //This should eventually get moved to the player input, under the generic Interact Unity Event
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (canTravel && interactWasPressed)
            {
                collision.gameObject.transform.position = targetDoor.transform.position;
                delayTimer = 0;
                canTravel = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        startCounting = false;
        delayTimer = 0;
    }

    public void Update()
    {
        Debug.Log(delayTimer);
        if (startCounting)
        {
            delayTimer += Time.deltaTime;
        }
        
        if (delayTimer >= defaultTime)
        {
            canTravel = true;
        }

        if (Input.GetKeyDown("space"))
        {
            interactWasPressed = true;
        }
        if (Input.GetKeyUp("space"))
        {
            interactWasPressed = false;
        }
    }
}
