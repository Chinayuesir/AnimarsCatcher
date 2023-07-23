using AnimarsCatcher.FSM;
using UnityEngine;

namespace AnimarsCatcher
{
    public class PickerAni_Follow:PickerAniStateBase
    {
        public PickerAni_Follow(int id, PICKER_Ani o) : base(id, o)
        {
        }
        

        public override void OnStay(params object[] args)
        {
            FollowPlayer();
            if (Owner.IsPick)
            {
                if (Owner.PickableItem.CheckCanPick())
                {
                    Owner.PickableItem.AddPickerAni(Owner);
                    StateMachine.TranslateState((int)PickerAniState.Pick);
                }
                else
                {
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
                mAnimator.SetFloat(AniSpeed,10f);
                mNavmeshAgent.destination = mPlayerTrans.position;
            }
        }
    }
}