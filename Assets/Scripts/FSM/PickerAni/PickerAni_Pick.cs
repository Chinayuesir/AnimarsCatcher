using UnityEngine;

namespace AnimarsCatcher
{
    public class PickerAni_Pick:PickerAniStateBase
    {
        private Transform mFruitTrans;
        public PickerAni_Pick(int id, PICKER_Ani o) : base(id, o)
        {
            
        }

        public override void OnEnter(params object[] args)
        {
            mFruitTrans = Owner.PickableItem.transform;
        }

        public override void OnStay(params object[] args)
        {
            if (FindPickableItem()&&!Owner.ReadyToCarry)
            {
                Owner.PickableItem.AddPickerAni(Owner);
                Owner.ReadyToCarry = true;
            }

            if (Owner.PickableItem.CheckCanCarry())
            {
                StateMachine.TranslateState((int)PickerAniState.Carry);
            }
        }

        private bool FindPickableItem()
        {
            if (Vector3.Distance(Owner.transform.position, mFruitTrans.position)
                <= mNavmeshAgent.stoppingDistance)
            {
                mNavmeshAgent.isStopped = true;
                mAnimator.SetFloat(AniSpeed,0f);
                return true;
            }

            mNavmeshAgent.isStopped = false;
            mNavmeshAgent.destination = mFruitTrans.position;
            mAnimator.SetFloat(AniSpeed,10f);
            return false;
        }
    }
}