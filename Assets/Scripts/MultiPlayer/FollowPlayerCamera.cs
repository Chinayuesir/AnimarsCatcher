using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


namespace AnimarsCatcher
{
    public class FollowPlayerCamera : MonoBehaviour
    {
        private CinemachineVirtualCamera mVirtualCamera;
        public static FollowPlayerCamera Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            mVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public void SetPlayerTrans(Transform player)
        {
            mVirtualCamera.Follow = player;
            mVirtualCamera.LookAt = player;
        }
    }
}

