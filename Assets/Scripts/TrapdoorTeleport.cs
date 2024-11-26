using CustomInput.Events;
using UnityEngine;
using UnityEngine.Windows;

public class TrapdoorTeleport : MonoBehaviour
{
    [SerializeField]
    private GameObject targetDoor;
    [SerializeField]
    private bool canTravel = false;

    private Transform PlayerTransform => PlayerData.singleton.GetPlayerTransform();
    private Timer PlayerTeleporterCooldown => PlayerData.singleton.GetPlayerTeleporterCooldownTimer();


    private void OnEnable()
    {
        InputHandler.singleton.ContBtnOnInteraction += UseTrapdoor;
    }

    private void OnDisable()
    {
        InputHandler.singleton.ContBtnOnInteraction -= UseTrapdoor;
    }

    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canTravel = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canTravel = false;
    }


    private void UseTrapdoor(Values input)
    {
        print(input.pressed + " " + canTravel);
        if (input.pressed && canTravel)
        {
            print(PlayerTeleporterCooldown.GetElapsedTime());
            if (PlayerTeleporterCooldown.IsRunning()) return;
            PlayerTeleporterCooldown.StartTimer();

            canTravel = false;
            PlayerTransform.position = targetDoor.transform.position;
        }

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
