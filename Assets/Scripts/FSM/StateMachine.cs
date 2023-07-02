using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher.FSM
{
    public class StateMachine
    {
        public Dictionary<int, StateBase> StateDic;
        
        //previous state
        public StateBase PreviousState;
        //Current State
        public StateBase CurrentState;

        public StateMachine(StateBase beginState)
        {
            PreviousState = null;
            CurrentState = beginState;
            StateDic = new Dictionary<int, StateBase>();
            //Add state to dic
            AddState(beginState);
            CurrentState.OnEnter();
        }

        /// <summary>
        /// Add a state to StateMachine
        /// </summary>
        /// <param name="state"></param>
        public void AddState(StateBase state)
        {
            if (!StateDic.ContainsKey(state.ID))
            {
                StateDic.Add(state.ID,state);
                state.StateMachine = this;
            }
        }

        /// <summary>
        /// Change State
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        public void TranslateState(int id, params object[] args)
        {
            if (!StateDic.ContainsKey(id))
            {
                Debug.LogError("There is no state with id:"+id+" in the current state machine");
                return;
            }

            PreviousState = CurrentState;
            CurrentState = StateDic[id];
            PreviousState.OnExit(args);
            CurrentState.OnEnter(args);
        }

        public void Update(params object[] args)
        {
            if(CurrentState!=null)
                CurrentState.OnStay(args);
        }
    }
}