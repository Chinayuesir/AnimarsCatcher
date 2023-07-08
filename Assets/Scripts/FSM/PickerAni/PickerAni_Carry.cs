using UnityEngine;

namespace AnimarsCatcher
{
    public class PickerAni_Carry:PickerAniStateBase
    {
        private Vector3 mTargetPosition;
        public PickerAni_Carry(int id, PICKER_Ani o) : base(id, o)
        {
            
        }

        public override void OnEnter(params object[] args)
        {
            mNavmeshAgent.isStopped = false;
            mNavmeshAgent.enabled = false;
        }

        public override void OnStay(params object[] args)
        {
            if (Owner.IsPick && Owner.ReadyToCarry)
            {
                mTargetPosition = Owner.PickableItem.GetPosition(Owner);
                Owner.transform.position = mTargetPosition;
                Owner.transform.forward = Owner.PickableItem.transform.forward;
                mAnimator.SetFloat(AniSpeed,3f);
            }
            else if(!Owner.IsPick && !Owner.ReadyToCarry)
            {
                mAnimator.SetFloat(AniSpeed,0f);
                StateMachine.TranslateState((int)PickerAniState.Follow);
                mNavmeshAgent.enabled = true;
            }
        }
    }
}