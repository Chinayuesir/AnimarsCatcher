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
        public Transform LeftHandIKTrans;
        public Transform RightHandIKTrans;

        private Animator mAnimator;

        //State Machine
        private StateMachine mStateMachine;
        
        //get from Player.cs
        public bool IsFollow=false;
        public bool IsPick = false;
        public bool ReadyToCarry = false;
        public PickableItem PickableItem;

        //destination
        public Vector3 Destination;

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

            Destination = transform.position;

            LeftHandIKTrans = new GameObject(name + "_LeftHandEffector").transform;
            RightHandIKTrans = new GameObject(name + "_RightHandEffector").transform;
            LeftHandIKTrans.parent = transform;
            RightHandIKTrans.parent = transform;
        }

        private void Update()
        {
            mStateMachine.Update();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (mStateMachine.CurrentState.ID == (int)PickerAniState.Carry)
            {
                mAnimator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIKTrans.position);
                mAnimator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandIKTrans.rotation);
                mAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                mAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                mAnimator.SetIKPosition(AvatarIKGoal.RightHand, RightHandIKTrans.position);
                mAnimator.SetIKRotation(AvatarIKGoal.RightHand, RightHandIKTrans.rotation);
                mAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                mAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            }
        }
    }
}
