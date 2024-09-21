using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _idleCam;
    [SerializeField] CinemachineVirtualCamera _followCam;
    [SerializeField] CinemachineVirtualCamera _stampedeCam;

    private void Awake()
    {
        SwitchToIdleCam();
    }

    public void SwitchToIdleCam()
    {
        _idleCam.enabled = true;
        _followCam.enabled = false;
        _stampedeCam.enabled = false;
    }

    public void SwitchToFollowCam(Transform followObject)
    {
        _followCam.Follow = followObject;

        _followCam.enabled = true;
        _idleCam.enabled = false;
    }

    public void ActivateStampedeCam()
    {
        if (!_idleCam.enabled)
        {
            return;
        }
        _idleCam.enabled = false;
        _followCam.enabled = false;
        _stampedeCam.enabled = true;
    }
}
