using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapdoorTeleport : MonoBehaviour
{
    [SerializeField]
    private GameObject targetDoor;
    private static bool canTravel = true;
    private static bool interactWasPressed = false;

    private static float defaultTime = 0.5f;
    private static float delayTimer;

    private void OnTriggerStay2D(Collider2D collision) //This should eventually get moved to the player input, under the generic Interact Unity Event
    {
        if (collision.gameObject.CompareTag("Player") && canTravel && interactWasPressed)
        {
            collision.gameObject.transform.position = targetDoor.transform.position;
            delayTimer = 0;
            canTravel = false;
        }
    }

    public void Update()
    {
        delayTimer += Time.deltaTime;
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
