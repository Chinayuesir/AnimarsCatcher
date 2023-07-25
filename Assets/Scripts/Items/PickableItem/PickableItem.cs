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

    public class PickableItem : MonoBehaviour,ICanPick,IResource
    {
        [SerializeField]
        private int mResourceCount;
        public int ResourceCount => mResourceCount;
        
        public List<Vector3> Positions = new List<Vector3>();

        public int MaxAniCount=2;
        public int CurrentAniCount=0;
        private List<PICKER_Ani> mAnis;
        private NavMeshAgent mTeamAgent;
        private Transform mHomeTransform;

        private LayerMask mLayerMask;

        private void Awake()
        {
            mAnis = new List<PICKER_Ani>();
            mTeamAgent = GetComponent<NavMeshAgent>();
            mTeamAgent.enabled = false;
            mHomeTransform = GameObject.FindWithTag("Home").transform;

            mLayerMask = gameObject.layer;
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

            if (Vector3.Distance(transform.GetPositionOnTerrain(),mHomeTransform.position)
                < mTeamAgent.stoppingDistance)
            {
                foreach (var ani in mAnis)
                {
                    ani.ReadyToCarry = false;
                    ani.IsPick = false;
                }
                mAnis.Clear();
                Positions.Clear();
                //TODO: Play an animation
                Destroy(gameObject);
                FindObjectOfType<GameRoot>().GameModel.FoodSum.Value += mResourceCount;
            }
        }

        public Vector3 GetPosition(PICKER_Ani ani)
        {
            return transform.TransformPoint(Positions[mAnis.IndexOf(ani)]);
        }

        public bool CheckCanPick()
        {
            return CurrentAniCount < MaxAniCount;
        }

        public bool CheckCanCarry()
        {
            if (CurrentAniCount == MaxAniCount)
            {
                foreach (var ani in mAnis)
                {
                    if(!ani.ReadyToCarry) return false;
                }

                return true;
            }

            return false;
        }

        public void AddPickerAni(PICKER_Ani pickerAni)
        {
            mAnis.Add(pickerAni);
            CurrentAniCount++;
        }

        private void OnMouseEnter()
        {
            gameObject.layer = LayerMask.NameToLayer("SelectedObject");
        }

        private void OnMouseExit()
        {
            gameObject.layer = mLayerMask;
        }
    }
}