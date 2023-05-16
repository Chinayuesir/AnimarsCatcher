using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher
{
    public class PICKER_Ani : MonoBehaviour
    {
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
    }
}
