using AnimarsCatcher.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace AnimarsCatcher
{
    public class BlasterAniStateBase:StateTemplate<BLASTER_Ani>
    {
        protected Animator mAnimator;
        protected NavMeshAgent mNavmeshAgent;
        protected Transform mPlayerTrans;
        protected static readonly int AniSpeed = Animator.StringToHash("AniSpeed");
        
        public BlasterAniStateBase(int id, BLASTER_Ani o) : base(id, o)
        {
            mAnimator = o.GetComponent<Animator>();
            mNavmeshAgent = o.GetComponent<NavMeshAgent>();
            mPlayerTrans = GameObject.FindWithTag("Player").transform;
        }
    }
}