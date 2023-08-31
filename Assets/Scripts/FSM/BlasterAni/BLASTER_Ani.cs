using System;
using System.Collections;
using System.Collections.Generic;
using AnimarsCatcher.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace AnimarsCatcher
{
    public enum BlasterAniState
    {
        None=0,
        Idle=1,
        Follow=2,
        Shoot=3,
    }

    public class BLASTER_Ani : MonoBehaviour
    {
        public Transform LeftHandIKTrans;
        public Transform RightHandIKTrans;
        
        private Animator mAnimator;

        //StateMachine
        private StateMachine mStateMachine;
        
        //Get from Player.cs
        public bool IsFollow = false;
        public bool IsShoot = false;
        public FragileItem FragileItem;
        private static readonly int Shoot1 = Animator.StringToHash("Shoot");
        
        public Transform GunTrans;

        private void Awake()
        {
            mAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            mStateMachine = new StateMachine(new BlasterAni_Idle((int) BlasterAniState.Idle, this));
            BlasterAni_Follow followState = new BlasterAni_Follow((int) BlasterAniState.Follow, this);
            mStateMachine.AddState(followState);
            BlasterAni_Shoot shootState = new BlasterAni_Shoot((int) BlasterAniState.Shoot, this);
            mStateMachine.AddState(shootState);
        }

        private void Update()
        {
           mStateMachine.Update();
        }

        public void Shoot()
        {
            if (FragileItem != null)
            {
                mAnimator.SetTrigger(Shoot1);
                Vector3 offset = FragileItem.transform.position - transform.position;
                Quaternion dir=Quaternion.LookRotation(offset);
                var go = PoolManager.Instance.BeamPool.Get();
                go.transform.position = GunTrans.position;
                go.transform.rotation = dir;
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (mAnimator.GetCurrentAnimatorStateInfo(1).IsName("Shoot"))
            {
                mAnimator.SetIKPosition(AvatarIKGoal.LeftHand,LeftHandIKTrans.position);
                mAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand,0.5f);
                mAnimator.SetIKRotation(AvatarIKGoal.LeftHand,LeftHandIKTrans.rotation);
                mAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand,0.5f);
            
                mAnimator.SetIKPosition(AvatarIKGoal.RightHand,RightHandIKTrans.position);
                mAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand,0.5f);
                mAnimator.SetIKRotation(AvatarIKGoal.RightHand,RightHandIKTrans.rotation);
                mAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand,0.5f);
            }
        }
    }
}

