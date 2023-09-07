using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ViewChanger : MonoBehaviour
{
    public CinemachineVirtualCameraBase FirstPersonCamera;
    public CinemachineVirtualCameraBase ThirdPersonCamera;

    private bool mIsThirdView=true;

    private void Awake()
    {
        FirstPersonCamera.Priority = 10;
        ThirdPersonCamera.Priority = 20;
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            mIsThirdView = !mIsThirdView;
            if (mIsThirdView)
            {
                FirstPersonCamera.Priority = 10;
                ThirdPersonCamera.Priority = 20;
            }
            else
            {
                FirstPersonCamera.Priority = 20;
                ThirdPersonCamera.Priority = 10;
            }
        }
    }
}
