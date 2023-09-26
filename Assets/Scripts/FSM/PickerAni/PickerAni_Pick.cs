using UnityEngine;

namespace AnimarsCatcher
{
    public class PickerAni_Pick:PickerAniStateBase
    {
        private Transform mPickableItemTrans;
        public PickerAni_Pick(int id, PICKER_Ani o) : base(id, o)
        {
        }

        public override void OnEnter(params object[] args)
        {
            mPickableItemTrans = Owner.PickableItem.transform;
        }

        public override void OnStay(params object[] args)
        {
            if (mPickableItemTrans == null)
            {
                StateMachine.TranslateState((int)PickerAniState.Follow);
            }

            if (mPickableItemTrans != null && FindPickableItem() && !Owner.ReadyToCarry)
            {
                Owner.ReadyToCarry = true;
            }

            if (mPickableItemTrans != null && Owner.PickableItem.CheckCanCarry() && Owner.ReadyToCarry)
            {
                StateMachine.TranslateState((int)PickerAniState.Carry);
            }
        }

        private bool FindPickableItem()
        {
            if (Vector3.Distance(Owner.transform.position, mPickableItemTrans.position)
                <= mNavmeshAgent.stoppingDistance)
            {
                mNavmeshAgent.isStopped = true;
                mAnimator.SetFloat(AniSpeed,0);
                return true;
            }
            else
            {
                mNavmeshAgent.isStopped = false;
                mAnimator.SetFloat(AniSpeed,10f);
                mNavmeshAgent.SetDestination(mPickableItemTrans.position);
                return false;
            }
        }
    }
}