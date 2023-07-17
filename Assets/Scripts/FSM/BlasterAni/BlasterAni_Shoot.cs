namespace AnimarsCatcher
{
    public class BlasterAni_Shoot:BlasterAniStateBase
    {
        public BlasterAni_Shoot(int id, BLASTER_Ani o) : base(id, o)
        {
        }

        public override void OnEnter(params object[] args)
        {
            mNavmeshAgent.isStopped = true;
            mAnimator.SetFloat(AniSpeed,0f);
            Owner.InvokeRepeating("Shoot",0.3f,1f);
            Owner.transform.LookAt(Owner.FragileItem.transform);
        }

        public override void OnStay(params object[] args)
        {
            if (Owner.FragileItem == null || Owner.FragileItem.HasDestroyed())
            {
                Owner.IsShoot = false;
                StateMachine.TranslateState((int)BlasterAniState.Follow);
            }
        }

        public override void OnExit(params object[] args)
        {
            Owner.CancelInvoke("Shoot");
        }
    }
}