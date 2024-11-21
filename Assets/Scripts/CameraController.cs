using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera closeFollowCam;
    public CinemachineVirtualCamera farFollowCam;
    public CinemachineVirtualCamera bigRoomCam;

    private CinemachineVirtualCamera currentCam;

    // Start is called before the first frame update
    void Start()
    {
        currentCam = closeFollowCam;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableRoomCamera(Vector3 newPosition)
    {
        bigRoomCam.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z - 10);
        SetActiveCamera(bigRoomCam);
    }

    public void EnableCloseFollowCamera()
    {
        SetActiveCamera(closeFollowCam);
    }

    public void EnableFarFollowCamera()
    {
        SetActiveCamera(farFollowCam);
    }

    public void SetActiveCamera(CinemachineVirtualCamera activeCam)
    {
        closeFollowCam.gameObject.SetActive(false);
        farFollowCam.gameObject.SetActive(false);
        bigRoomCam.gameObject.SetActive(false);

        activeCam.gameObject.SetActive(true);

        currentCam = activeCam;
    }
}
