using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher
{
    public class FollowPlayer : MonoBehaviour
    {
        private Transform mPlayerTrans;
        private Vector3 mOffset;
        void Start()
        {
            mPlayerTrans = GameObject.FindWithTag("Player").transform;
            mOffset = mPlayerTrans.position - transform.position;
        }

        private void LateUpdate()
        {
            transform.position = mPlayerTrans.position - mOffset;
        }
    }
}


