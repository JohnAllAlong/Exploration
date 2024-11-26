using CustomInput.Events;
using UnityEngine;

public class TrapdoorTeleport : MonoBehaviour
{
    [SerializeField]
    private GameObject targetDoor;
    private static bool canTravel = true;
    private static bool interactWasPressed = false;
    [SerializeField]
    private static float defaultTime = 2.0f;
    [SerializeField]
    private Timer interactionTimer;

    //all simple actions must be created in start
    private void OnEnable()
    {
        interactionTimer = new Timer(defaultTime).DestroyOnEnd(false);

        InputHandler.ContBtnOnInteraction += UseTrapdoor;
    }

    private void OnDisable()
    {
        InputHandler.ContBtnOnInteraction -= UseTrapdoor;
    }

    private void OnTriggerStay2D(Collider2D collision) //This should eventually get moved to the player input, under the generic Interact Unity Event
    {
        if (collision.gameObject.CompareTag("Player") && canTravel && interactWasPressed)
        {
            collision.gameObject.transform.position = targetDoor.transform.position;
            canTravel = false;
        }
    }

    private void UseTrapdoor(Values input)
    {

        if (input.pressed)
        {
            interactWasPressed = true;
        }
        if (!input.pressed)
        {
            interactWasPressed = false;
        }

        //print(interactionTimer.GetElapsedTime());
        if (interactionTimer.IsRunning()) return;
        canTravel = true;
        interactionTimer.StartTimer();

    }
    /*
    public void Update()
    {
        Debug.Log(delayTimer);
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
    */
}
