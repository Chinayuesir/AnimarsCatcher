using AnimarsCatcher.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace AnimarsCatcher
{
    public class PickerAni_Idle:PickerAniStateBase
    {
        public PickerAni_Idle(int id, PICKER_Ani o) : base(id, o)
        {
          
        }

        public override void OnEnter(params object[] args)
        {
            mNavmeshAgent.isStopped = false;
            mAnimator.SetFloat(AniSpeed,0);
        }

        public override void OnStay(params object[] args)
        {
            if (Owner.IsFollow)
            {
                StateMachine.TranslateState((int)PickerAniState.Follow);
            }
        }
        
    }
}