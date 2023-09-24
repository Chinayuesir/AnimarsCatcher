namespace AnimarsCatcher
{
    public class BlasterAni_Find : BlasterAniStateBase
    {
        public BlasterAni_Find(int id, BLASTER_Ani o) : base(id, o)
        {
        }

        public override void OnEnter(params object[] args)
        {
            mNavmeshAgent.destination = Owner.FragileItem.transform.position;
        }

        public override void OnStay(params object[] args)
        {
            if (Owner.FragileItem == null || Owner.FragileItem.HasDestroyed())
            {
                StateMachine.TranslateState((int)BlasterAniState.Follow);
            }
            else if(Owner.FragileItem.CheckCanShoot(Owner.GunTrans.position))
            {
                mNavmeshAgent.isStopped = true;
                StateMachine.TranslateState((int)BlasterAniState.Shoot);
            }
            else
            {
                mNavmeshAgent.isStopped = false;
                mAnimator.SetFloat(AniSpeed, 10f);
            }
        }
    }
}
