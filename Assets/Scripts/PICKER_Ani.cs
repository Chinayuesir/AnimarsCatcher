<<<<<<< HEAD
<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher
{
    public class PICKER_Ani1 : MonoBehaviour
    {
        private Animator mAnimator;
        private bool mCanMove;
        private Vector3 mTargetPos;
        private float mAniSpeed = 5f;
        private static readonly int AniSpeed = Animator.StringToHash("AniSpeed");

        
=======
=======
>>>>>>> parent of 9723e04 (Chapter6 demo version)
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
<<<<<<< HEAD
        public Transform LeftHandIKTrans;
        public Transform RightHandIKTrans;

        private Animator mAnimator;

        //State Machine
        private StateMachine mStateMachine;
        
=======
        //State Machine
        private StateMachine mStateMachine;
        
        private float mAniSpeed = 5f;
        private static readonly int AniSpeed = Animator.StringToHash("AniSpeed");
        
>>>>>>> parent of 9723e04 (Chapter6 demo version)
        //get from Player.cs
        public bool IsFollow=false;
        public bool IsPick = false;
        public bool ReadyToCarry = false;
        public PickableItem PickableItem;
<<<<<<< HEAD

        //destination
        public Vector3 Destination;
>>>>>>> parent of 9723e04 (Chapter6 demo version)

        private void Awake()
        {
            mAnimator = GetComponent<Animator>();
        }

<<<<<<< HEAD
        private void Update()
        {
            if (mCanMove)
            {
                mAnimator.SetFloat(AniSpeed,mAniSpeed);
                float step = mAniSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, mTargetPos, step);

                var dir = (mTargetPos - transform.position).normalized;
                transform.forward = dir;

                if (Vector3.Distance(transform.position, mTargetPos) < 1f)
                {
                    mCanMove = false;
                    mAnimator.SetFloat(AniSpeed,0);
                }
            }
        }

        public void SetMoveTargetPos(Vector3 targetPos)
        {
            mCanMove = true;
            mTargetPos = targetPos;
=======
=======
        
>>>>>>> parent of 9723e04 (Chapter6 demo version)
        private void Start()
        {
            mStateMachine = new StateMachine(new PickerAni_Idle((int) PickerAniState.Idle, this));
            PickerAni_Follow followState = new PickerAni_Follow((int) PickerAniState.Follow, this);
            mStateMachine.AddState(followState);
            PickerAni_Pick pickState = new PickerAni_Pick((int) PickerAniState.Pick, this);
            mStateMachine.AddState(pickState);
            PickerAni_Carry carryState = new PickerAni_Carry((int) PickerAniState.Carry, this);
            mStateMachine.AddState(carryState);
<<<<<<< HEAD

            Destination = transform.position;

            LeftHandIKTrans = new GameObject(name + "_LeftHandEffector").transform;
            RightHandIKTrans = new GameObject(name + "_RightHandEffector").transform;
            LeftHandIKTrans.parent = transform;
            RightHandIKTrans.parent = transform;
=======
>>>>>>> parent of 9723e04 (Chapter6 demo version)
        }

        private void Update()
        {
            mStateMachine.Update();
        }
<<<<<<< HEAD

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
>>>>>>> parent of 9723e04 (Chapter6 demo version)
        }
=======
>>>>>>> parent of 9723e04 (Chapter6 demo version)
    }
}
