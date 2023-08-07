using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace AnimarsCatcher
{
    public interface ICanPick
    {
        bool CheckCanPick();
        bool CheckCanCarry();
    }

    public enum PickableItemType
    {
        Food,
        Crystal
    }

    public class PickableItem : MonoBehaviour,ICanPick,IResource
    {
        [SerializeField]
        private int mResourceCount;
        public int ResourceCount => mResourceCount;
        public PickableItemType ItemType;
        
        public List<Vector3> Positions = new List<Vector3>();

        public int MaxAniCount=2;
        public int CurrentAniCount=0;
        private List<PICKER_Ani> mAnis;
        private NavMeshAgent mTeamAgent;
        private Transform mHomeTransform;
        private Transform mPickedTrans;

        private LayerMask mLayerMask;

        private bool mIsPicked = false;
        private AudioSource mPickedAudioSource;

        private void Awake()
        {
            mAnis = new List<PICKER_Ani>();
            mTeamAgent = GetComponent<NavMeshAgent>();
            mTeamAgent.enabled = false;
            mHomeTransform = GameObject.FindWithTag("Home").transform;
            mPickedTrans = mHomeTransform.Find("PickedPos").transform;

            mLayerMask = gameObject.layer;
            mPickedAudioSource = GetComponent<AudioSource>();
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
                mTeamAgent.speed = (float) CurrentAniCount / MaxAniCount * 2 * Const.BaseCarrySpeed;
            }

            if (Vector3.Distance(transform.GetPositionOnTerrain(),mHomeTransform.position)
                < mTeamAgent.stoppingDistance && !mIsPicked)
            {
                mIsPicked = true;
                foreach (var ani in mAnis)
                {
                    ani.ReadyToCarry = false;
                    ani.IsPick = false;
                }
                mAnis.Clear();
                Positions.Clear();
                switch (ItemType)
                {
                    case PickableItemType.Food:
                        FindObjectOfType<GameRoot>().GameModel.FoodSum.Value += mResourceCount;
                        break;
                    case PickableItemType.Crystal:
                        FindObjectOfType<GameRoot>().GameModel.CrystalSum.Value += mResourceCount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if(!mPickedAudioSource.isPlaying) mPickedAudioSource.Play();
                transform.DOMove(mPickedTrans.position, 2f);
                transform.DOScale(Vector3.zero, 2f).OnComplete(() =>
                {
                    Destroy(gameObject);
                });
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
            if (CurrentAniCount>0 && CurrentAniCount >= MaxAniCount/2)
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