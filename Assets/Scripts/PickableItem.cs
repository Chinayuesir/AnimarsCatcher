using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AnimarsCatcher
{
    public interface ICanPick
    {
        bool CheckCanPick();
        bool CheckCanCarry();
    }

    public class PickableItem : MonoBehaviour,ICanPick
    {
        public List<Vector3> Positions=new List<Vector3>();
        
        public int MaxAniCount=2;
        public int CurrentAniCount=0;
        private List<PICKER_Ani> mAnis;
        private NavMeshAgent mTeamAgent;
        private Transform mHomeTransform;

        private void Awake()
        {
            mAnis = new List<PICKER_Ani>();
            mTeamAgent = GetComponent<NavMeshAgent>();
            mTeamAgent.enabled = false;
            mHomeTransform = GameObject.FindWithTag("Home").transform;
        }
        
        private void Update()
        {
            TeamAgentMove();
        }
        
        private void TeamAgentMove()
        {
            if (CheckCanCarry() && !mTeamAgent.enabled)
            {
                mTeamAgent.enabled = true;
            }

            if (mTeamAgent.enabled)
            {
                mTeamAgent.SetDestination(mHomeTransform.position);
            }

            if (Vector3.Distance(transform.GetPositionOnTerrain(), mHomeTransform.position)
                < mTeamAgent.stoppingDistance)
            {
                foreach (var ani in mAnis)
                {
                    ani.ReadyToCarry = false;
                    ani.IsPick = false;
                }
                mAnis.Clear();
                Positions.Clear();
                Destroy(gameObject);
            }
        }
        
        public Vector3 GetPosition(PICKER_Ani ani)
        {
            return transform.position + Positions[mAnis.IndexOf(ani)];
        }

        public bool CheckCanPick()
        {
            return CurrentAniCount < MaxAniCount;
        }

        public bool CheckCanCarry()
        {
            return CurrentAniCount == MaxAniCount;
        }

        public void AddPickerAni(PICKER_Ani pickerAni)
        {
            mAnis.Add(pickerAni);
            CurrentAniCount++;
        }
    }
}