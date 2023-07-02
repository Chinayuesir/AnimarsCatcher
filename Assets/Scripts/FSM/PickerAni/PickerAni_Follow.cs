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
            mNavmeshAgent.isStopped = false;
            mAnimator.SetFloat(AniSpeed,10f);
        }

        public override void OnStay(params object[] args)
        {
            mNavmeshAgent.destination = mPlayerTrans.position;
            if (Vector3.Distance(Owner.transform.position, mPlayerTrans.position)
                <= mNavmeshAgent.stoppingDistance)
            {
                StateMachine.TranslateState((int)PickerAniState.Idle);
            }
        }
    }
}