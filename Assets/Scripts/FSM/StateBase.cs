using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher.FSM
{
    /// <summary>
    /// State Base Class
    /// </summary>
    public class StateBase
    {
        public int ID { get; set; }

        public StateMachine StateMachine;

        protected StateBase(int id)
        {
            ID = id;
        }

        public virtual void OnEnter(params object[] args)
        {
            
        }
        
        public virtual void OnStay(params object[] args)
        {
            
        }
        
        public virtual void OnExit(params object[] args)
        {
            
        }
    }

    public class StateTemplate<T> : StateBase
    {
        public T Owner;
        protected StateTemplate(int id,T o) : base(id)
        {
            Owner = o;
        }
    }

}

