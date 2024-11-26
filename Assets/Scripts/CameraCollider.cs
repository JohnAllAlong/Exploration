using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    private CameraController camController;
    // Start is called before the first frame update
    void Start()
    {
        camController = FindFirstObjectByType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            camController.EnableFarFollowCamera();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            camController.EnableCloseFollowCamera();
        }
    }
}
