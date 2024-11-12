using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera roomCam;
    public CinemachineVirtualCamera followCam;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RelocateCamera(Vector3 newPosition)
    {
        roomCam.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z - 10);
    }

    public void SetFollowCamera(bool follow)
    {
        if (follow) 
        { 
            followCam.gameObject.SetActive(true);
            roomCam.gameObject.SetActive(false);
        }
        else
        {
            followCam.gameObject.SetActive(false);
            roomCam.gameObject.SetActive(true);
        }
    }
}
