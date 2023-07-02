using System;
using System.Collections;
using System.Collections.Generic;
using AnimarsCatcher.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace AnimarsCatcher
{
    public enum PickerAniState
    {
        None=0,
        Idle=1,
        Follow=2,
        Pick=3,
        Carry=4
    }

    public class PICKER_Ani : MonoBehaviour
    {
        //State Machine
        private PickerAniState mStateID;
        private StateMachine mStateMachine;
        
        //components
        private Animator mAnimator;
        private NavMeshAgent mAgent;
        
        private float mAniSpeed = 5f;
        private static readonly int AniSpeed = Animator.StringToHash("AniSpeed");
        
        //get from Player.cs
        public bool IsFollow=false;

        private void Awake()
        {
            mAnimator = GetComponent<Animator>();
            mAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            mStateMachine = new StateMachine(new PickerAni_Idle((int) PickerAniState.Idle, this));
            PickerAni_Follow followState = new PickerAni_Follow((int) PickerAniState.Follow, this);
            mStateMachine.AddState(followState);
            mStateID = PickerAniState.Idle;
        }

        private void Update()
        {
            mStateMachine.Update();
        }
    }
}
