using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AnimarsCatcher
{
<<<<<<< HEAD:Assets/Scripts/FSM/BlasterAni/BLASTER_Ani.cs
    public enum BlasterAniState
    {
        None=0,
        Idle=1,
        Follow=2,
        Shoot=3,
        Find=4
    }

=======
>>>>>>> parent of 9723e04 (Chapter6 demo version):Assets/Scripts/BLASTER_Ani.cs
    public class BLASTER_Ani : MonoBehaviour
    {
        public Transform LeftHandIKTrans;
        public Transform RightHandIKTrans;
        
        private Animator mAnimator;
<<<<<<< HEAD:Assets/Scripts/FSM/BlasterAni/BLASTER_Ani.cs

        //StateMachine
        private StateMachine mStateMachine;
        
        //Get from Player.cs
        public bool IsFollow = false;
        public bool IsShoot = false;
        public FragileItem FragileItem;
        private static readonly int Shoot1 = Animator.StringToHash("Shoot");
        
        public Transform GunTrans;
=======
        private NavMeshAgent mAgent;
        
        private bool mCanMove;
        private Vector3 mTargetPos;
        private float mAniSpeed = 5f;
        private static readonly int AniSpeed = Animator.StringToHash("AniSpeed");
>>>>>>> parent of 9723e04 (Chapter6 demo version):Assets/Scripts/BLASTER_Ani.cs

        //destination
        public Vector3 Destination;

        private void Awake()
        {
            mAnimator = GetComponent<Animator>();
<<<<<<< HEAD:Assets/Scripts/FSM/BlasterAni/BLASTER_Ani.cs
        }

        private void Start()
        {
            mStateMachine = new StateMachine(new BlasterAni_Idle((int) BlasterAniState.Idle, this));
            BlasterAni_Follow followState = new BlasterAni_Follow((int) BlasterAniState.Follow, this);
            mStateMachine.AddState(followState);
            BlasterAni_Shoot shootState = new BlasterAni_Shoot((int) BlasterAniState.Shoot, this);
            mStateMachine.AddState(shootState);
            BlasterAni_Find findState = new BlasterAni_Find((int)BlasterAniState.Find, this);
            mStateMachine.AddState(findState);

            Destination = transform.position;
=======
            mAgent = GetComponent<NavMeshAgent>();
>>>>>>> parent of 9723e04 (Chapter6 demo version):Assets/Scripts/BLASTER_Ani.cs
        }

        private void Update()
        {
            if (mCanMove)
            {
                mAnimator.SetFloat(AniSpeed,mAniSpeed);
                mAgent.SetDestination(mTargetPos);
                
                if (Vector3.Distance(transform.position, mTargetPos) < mAgent.stoppingDistance)
                {
                    mCanMove = false;
                    mAnimator.SetFloat(AniSpeed,0);
                }
            }
        }

        public void SetMoveTargetPos(Vector3 targetPos)
        {
<<<<<<< HEAD:Assets/Scripts/FSM/BlasterAni/BLASTER_Ani.cs
            if (FragileItem != null)
            {
                mAnimator.SetTrigger(Shoot1);
                Vector3 offset = FragileItem.transform.position - transform.position;
                Quaternion dir=Quaternion.LookRotation(offset);
                var go = PoolManager.Instance.BeamPool.Get();
                go.transform.position = GunTrans.position;
                go.transform.rotation = dir;
            }
=======
            mCanMove = true;
            mTargetPos = targetPos;
>>>>>>> parent of 9723e04 (Chapter6 demo version):Assets/Scripts/BLASTER_Ani.cs
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

