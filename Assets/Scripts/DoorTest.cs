using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CustomInput.Events;

public class DoorTest : MonoBehaviour
{
    public TextMeshProUGUI doorText;
    public Transform player;
    public GameObject door;

    public PlayerCollectibleController cc;

    public bool hasKey = false;
    public bool inRange = false;

    // manually set in the editor
    public Vector2 direction = Vector2.zero;

    public LineRenderer lineRenderer;

    private void OnEnable()
    {
        InputHandler.OnceBtnOnInteractionUse += OpenDoor;
    }

    private void OnDisable()
    {
        InputHandler.OnceBtnOnInteractionUse -= OpenDoor;
    }

    public void OpenDoor(Values input)
    {
        if (!hasKey) return;
        cc.UseCollectable(1);
        door.SetActive(false);
        enabled = false;
    }

    private void Update()
    {
        //check each frame if the player has the key
        if (cc.HasCollectable(1) && !hasKey)
        {
            hasKey = true;
        }

        if (inRange)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, player.position);
            LayerMask mask = LayerMask.NameToLayer("Door");
            RaycastHit2D hit = Physics2D.Raycast(player.position, direction, Mathf.Infinity, mask);
            Debug.DrawRay(player.position, direction, Color.blue);
            if (hit)
            {
                lineRenderer.SetPosition(1, hit.point);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && hasKey)
        {
            inRange = true;
            doorText.text = "I think i have the key to this door!";
        } else if (collider.CompareTag("Player"))
        {
            doorText.text = "I need a key for this door!";
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            inRange = false;
            lineRenderer.enabled = false;
            doorText.text = "";
        }
    }
}
