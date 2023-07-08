using AnimarsCatcher.FSM;
using UnityEngine;

namespace AnimarsCatcher
{
    public class PickerAni_Follow:PickerAniStateBase
    {
        public PickerAni_Follow(int id, PICKER_Ani o) : base(id, o)
        {
        }

        public override void OnEnter(params object[] args)
        {
            
        }

        public override void OnStay(params object[] args)
        {
            FollowPlayer();
            if (Owner.IsPick)
            {
                if(Owner.PickableItem.CheckCanPick())
                   StateMachine.TranslateState((int)PickerAniState.Pick);
                else
                {
                    StateMachine.TranslateState((int)PickerAniState.Follow);
                    Owner.IsPick = false;
                    Owner.PickableItem = null;
                }
            }
        }

        private void FollowPlayer()
        {
            if (Vector3.Distance(Owner.transform.position, mPlayerTrans.position)
                <= mNavmeshAgent.stoppingDistance)
            {
                mNavmeshAgent.isStopped = true;
                mAnimator.SetFloat(AniSpeed,0f);
            }
            else
            {
                mNavmeshAgent.isStopped = false;
                mNavmeshAgent.destination = mPlayerTrans.position;
                mAnimator.SetFloat(AniSpeed,10f);
            }
        }

    }
}