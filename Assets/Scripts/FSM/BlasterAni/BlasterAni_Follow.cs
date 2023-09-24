using UnityEngine;

namespace AnimarsCatcher
{
    public class BlasterAni_Follow:BlasterAniStateBase
    {
        public BlasterAni_Follow(int id, BLASTER_Ani o) : base(id, o)
        {
        }
        
        public override void OnStay(params object[] args)
        {
            FollowPlayer();
            if (Owner.IsShoot)
            {
                if(Owner.FragileItem!=null && Owner.FragileItem.CheckCanShoot(Owner.GunTrans.position))
                {
                    if(Owner.FragileItem != null)
                    {
                        if (Owner.FragileItem.CheckCanShoot(Owner.GunTrans.position))
                        {
                            StateMachine.TranslateState((int)BlasterAniState.Shoot);
                        }
                        else
                        {
                            StateMachine.TranslateState((int)BlasterAniState.Find);
                        }
                    }
                }
                else
                {
                    StateMachine.TranslateState((int)BlasterAniState.Follow);
                    Owner.IsShoot = false;
                    Owner.FragileItem = null;
                }
            }
        }

        private void FollowPlayer()
        {
            if (Vector3.Distance(Owner.transform.position, Owner.Destination)
                <= mNavmeshAgent.stoppingDistance)
            {
                mNavmeshAgent.isStopped = true;
                mAnimator.SetFloat(AniSpeed,0f);
            }
            else
            {
                mNavmeshAgent.isStopped = false;
                mAnimator.SetFloat(AniSpeed,10f);
                mNavmeshAgent.destination = Owner.Destination;
            }
        }
    }
}