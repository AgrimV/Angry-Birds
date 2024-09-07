using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _idleCam;
    [SerializeField] CinemachineVirtualCamera _followCam;

    private void Awake()
    {
        SwitchToIdleCam();
    }

    public void SwitchToIdleCam()
    {
        _idleCam.enabled = true;
        _followCam.enabled = false;
    }

    public void SwitchToFollowCam(Transform followObject)
    {
        _followCam.Follow = followObject;

        _followCam.enabled = true;
        _idleCam.enabled = false;
    }
}
