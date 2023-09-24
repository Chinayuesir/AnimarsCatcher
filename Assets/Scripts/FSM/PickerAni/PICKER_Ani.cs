using System;
using System.Collections;
using System.Collections.Generic;
using AnimarsCatcher.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace AnimarsCatcher
{
    public enum PickerAniState
    {
        None=0,
        Idle=1,
        Follow=2,
        Pick=3,
        Carry=4
    }

    public class PICKER_Ani : MonoBehaviour
    {
        //State Machine
        private StateMachine mStateMachine;
        
        private float mAniSpeed = 5f;
        private static readonly int AniSpeed = Animator.StringToHash("AniSpeed");
        
        //get from Player.cs
        public bool IsFollow=false;
        public bool IsPick = false;
        public bool ReadyToCarry = false;
        public PickableItem PickableItem;

        // Group Behaviour
        public Vector3 Destination;

        // IK
        private Animator mAnimator;
        public Transform LeftHandEffector;
        public Transform RightHandEffector;

        private void Awake()
        {
            mAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            mStateMachine = new StateMachine(new PickerAni_Idle((int) PickerAniState.Idle, this));
            PickerAni_Follow followState = new PickerAni_Follow((int) PickerAniState.Follow, this);
            mStateMachine.AddState(followState);
            PickerAni_Pick pickState = new PickerAni_Pick((int) PickerAniState.Pick, this);
            mStateMachine.AddState(pickState);
            PickerAni_Carry carryState = new PickerAni_Carry((int) PickerAniState.Carry, this);
            mStateMachine.AddState(carryState);

            // Generate Hand Effectors
            LeftHandEffector = new GameObject("LeftHandEffector").transform;
            LeftHandEffector.parent = transform;
            RightHandEffector = new GameObject("RightHandEffector").transform;
            RightHandEffector.parent = transform;
        }

        private void Update()
        {
            mStateMachine.Update();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (mStateMachine.CurrentState.ID == (int)PickerAniState.Carry)
            {
                mAnimator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandEffector.position);
                mAnimator.SetIKPosition(AvatarIKGoal.RightHand, RightHandEffector.position);
                mAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                mAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                mAnimator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandEffector.rotation);
                mAnimator.SetIKRotation(AvatarIKGoal.RightHand, RightHandEffector.rotation);
                mAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                mAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            }
        }
    }
}
