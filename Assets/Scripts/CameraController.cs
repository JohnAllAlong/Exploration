using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera roomCam;
    public CinemachineVirtualCamera leapfrogCam;
    public CinemachineVirtualCamera followCam;

    private CinemachineVirtualCamera currentCam;

    // Start is called before the first frame update
    void Start()
    {
        currentCam = roomCam;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RelocateCamera(Vector3 newPosition)
    {
        if (currentCam == roomCam)
        {
            roomCam.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z - 10);
            roomCam.gameObject.SetActive(true);
            leapfrogCam.gameObject.SetActive(false);
            currentCam = leapfrogCam;
        }
        else
        {
            leapfrogCam.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z - 10);
            leapfrogCam.gameObject.SetActive(true);
            roomCam.gameObject.SetActive(false);
            currentCam = roomCam;
        }
        
    }

    public void SetFollowCamera(bool follow)
    {
        if (follow) 
        { 
            followCam.gameObject.SetActive(true);
            roomCam.gameObject.SetActive(false);
            leapfrogCam.gameObject.SetActive(false);
        }
        else
        {
            followCam.gameObject.SetActive(false);
            roomCam.gameObject.SetActive(true);
            leapfrogCam.gameObject.SetActive(false);
        }
    }
}
