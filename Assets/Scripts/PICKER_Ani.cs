using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AnimarsCatcher
{
    public class PICKER_Ani : MonoBehaviour
    {
        //components
        private Animator mAnimator;
        private NavMeshAgent mAgent;
        
        private bool mCanMove;
        private Vector3 mTargetPos;
        private float mAniSpeed = 5f;
        private static readonly int AniSpeed = Animator.StringToHash("AniSpeed");

        private void Awake()
        {
            mAnimator = GetComponent<Animator>();
            mAgent = GetComponent<NavMeshAgent>();
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
            mCanMove = true;
            mTargetPos = targetPos;
        }
    }
}
