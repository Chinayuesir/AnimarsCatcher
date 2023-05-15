using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher
{
    public class CameraFollow : MonoBehaviour
    {
        private Vector3 mOffset;

        private Transform mPlayer;
        // Start is called before the first frame update
        void Start()
        {
            mPlayer = GameObject.FindWithTag("Player").transform;
            mOffset = transform.position - mPlayer.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = mOffset + mPlayer.position;
        }
    }

}
