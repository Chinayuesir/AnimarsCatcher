using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace AnimarsCatcher
{
    public class CameraController : MonoBehaviour
    {
        public float XSpeed = 300f;
        private CinemachineFreeLook mFreeLook;

        private void Awake()
        {
            mFreeLook = GetComponent<CinemachineFreeLook>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(2))
            {
                mFreeLook.m_XAxis.m_MaxSpeed = XSpeed;
            }
            else
            {
                mFreeLook.m_XAxis.m_MaxSpeed = 0;
            }
        }
    }

}

