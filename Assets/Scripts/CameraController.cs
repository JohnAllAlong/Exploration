using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera leapfrogCam1;
    public CinemachineVirtualCamera leapfrogCam2;
    public CinemachineVirtualCamera followCam;

    private CinemachineVirtualCamera currentCam;

    // Start is called before the first frame update
    void Start()
    {
        currentCam = leapfrogCam1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RelocateCamera(Vector3 newPosition)
    {
        if (currentCam == leapfrogCam1)
        {
            currentCam = leapfrogCam2;
            leapfrogCam1.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z - 10);
            leapfrogCam2.gameObject.SetActive(false);
            leapfrogCam1.gameObject.SetActive(true);
        }
        else if (currentCam == leapfrogCam2)
        {
            currentCam = leapfrogCam1;
            leapfrogCam2.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z - 10);
            leapfrogCam1.gameObject.SetActive(false);
            leapfrogCam2.gameObject.SetActive(true);
        }
    }

    public void SetFollowCamera(bool follow)
    {
        if (follow) 
        { 
            followCam.gameObject.SetActive(true);
            leapfrogCam1.gameObject.SetActive(false);
            leapfrogCam2.gameObject.SetActive(false);
        }
        else
        {
            followCam.gameObject.SetActive(false);
        }
    }

    public void SetCameraPriority()
    {

    }
}
