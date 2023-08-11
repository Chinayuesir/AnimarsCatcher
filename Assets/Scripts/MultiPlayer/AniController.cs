using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnimarsCatcher
{
    public class AniController:NetworkBehaviour
    {
        private float mTurnSpeed = 3f;
        private float mRunSpeed = 5f;
        private Animator mAnimator;
        private CharacterController mCharacterController;
        private Vector2 mRandomX = new Vector2(140, 160);
        private Vector2 mRandomZ = new Vector2(40, 60);
        
        //NetworkVariables
        private NetworkVariable<Vector3> mNetworkPosition = new NetworkVariable<Vector3>();
        private NetworkVariable<Vector3> mNetworkRotation = new NetworkVariable<Vector3>();
        private NetworkVariable<bool> mIsRun = new NetworkVariable<bool>();
        
        //NetworkVariables Cache
        private Vector3 mOldPosition;
        private Vector3 mOldRotation;

        public override void OnNetworkSpawn()
        {
            mAnimator = GetComponent<Animator>();
            mCharacterController = GetComponent<CharacterController>();

            if (IsServer || IsClient)
            {
                Vector3 randomPos = new Vector3(
                    Random.Range(mRandomX.x, mRandomX.y), 0,
                    Random.Range(mRandomZ.x, mRandomZ.y));
                transform.position = randomPos;
            }

            if (IsClient && IsOwner)
            {
                FollowPlayerCamera.Instance.SetPlayerTrans(transform.Find("Root"));
            }

            base.OnNetworkSpawn();
        }

        private void Update()
        {
            if (IsClient && IsOwner)
            {
                ClientInput();
            }
            
            ClientMove();
            ClientAnimation();
        }

        private void ClientMove()
        {
            mCharacterController.SimpleMove(mNetworkPosition.Value);
            transform.Rotate(mNetworkRotation.Value,Space.World);
        }

        private void ClientAnimation()
        {
            mAnimator.SetBool("IsRun",mIsRun.Value);
        }

        private void ClientInput()
        {
            //Player Input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 dir = transform.TransformDirection(Vector3.forward);
            Vector3 inputPosition = dir * vertical;
            Vector3 inputRotation = new Vector3(0, horizontal, 0);

            if (mOldPosition != inputPosition || mOldRotation != inputRotation)
            {
                mOldPosition = inputPosition;
                mOldRotation = inputRotation;
                UpdatePlayerMovementServerRpc(mRunSpeed*inputPosition,mTurnSpeed*inputRotation);
            }

            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
            bool state = hasHorizontalInput || hasVerticalInput;
            UpdatePlayerAnimationServerRpc(state);
        }

        [ServerRpc]
        private void UpdatePlayerMovementServerRpc(Vector3 newPosition, Vector3 newRotation)
        {
            mNetworkPosition.Value = newPosition;
            mNetworkRotation.Value=newRotation;
        }

        [ServerRpc]
        private void UpdatePlayerAnimationServerRpc(bool state)
        {
            mIsRun.Value = state;
        }
    }
}