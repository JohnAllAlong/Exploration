using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCollider : MonoBehaviour
{
    public CameraController camController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            camController.RelocateCamera(collision.gameObject.transform.position);
            camController.SetFollowCamera(false);
        }
        else if (collision.gameObject.CompareTag("Hallway"))
        {
            camController.SetFollowCamera(true);
        }
    }
}
