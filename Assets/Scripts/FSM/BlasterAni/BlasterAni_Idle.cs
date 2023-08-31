namespace AnimarsCatcher
{
    public class BlasterAni_Idle:BlasterAniStateBase
    {
        public BlasterAni_Idle(int id, BLASTER_Ani o) : base(id, o)
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
                StateMachine.TranslateState((int)BlasterAniState.Follow);
            }
        }
    }
}