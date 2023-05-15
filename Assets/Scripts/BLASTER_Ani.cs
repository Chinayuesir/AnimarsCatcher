using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher
{
    public class BLASTER_Ani : MonoBehaviour
    {
        public Transform LeftHandIKTrans;
        public Transform RightHandIKTrans;
        
        private Animator mAnimator;
        private bool mCanMove;
        private Vector3 mTargetPos;
        private float mAniSpeed = 5f;
        private static readonly int AniSpeed = Animator.StringToHash("AniSpeed");

        private void Awake()
        {
            mAnimator = GetComponent<Animator>();
        }

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

