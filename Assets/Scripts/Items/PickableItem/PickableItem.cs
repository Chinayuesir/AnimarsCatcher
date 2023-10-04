using System;
using System.Collections.Generic;
using TMPro;
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

    public enum FoodType
    {
        Simple,
        AddSpeed
    }

    public class PickableItem : MonoBehaviour,ICanPick, IResource
    {
        [SerializeField]
        private int mResourceCount;
        public int ResourceCount => mResourceCount;
        public PickableItemType ItemType;
        public FoodType FoodType;

        public List<Vector3> Positions = new List<Vector3>();

        public int MaxAniCount=2;
        public ReactiveProperty<int> CurrentAniCount = new(0);
        private List<PICKER_Ani> mAnis;
        private NavMeshAgent mTeamAgent;
        private Transform mHomeTransform;

        private LayerMask mLayerMask;
        private TextMeshProUGUI mText_CurrentAniCount;

        private void Awake()
        {
            mAnis = new List<PICKER_Ani>();
            mTeamAgent = GetComponent<NavMeshAgent>();
            mTeamAgent.enabled = false;
            mHomeTransform = GameObject.FindWithTag("Home").transform;

            mLayerMask = gameObject.layer;

            transform.Find("PickerCanvas/MaxAniCount").GetComponent<TextMeshProUGUI>().text = MaxAniCount.ToString();
            mText_CurrentAniCount = transform.Find("PickerCanvas/CurrentAniCount").GetComponent<TextMeshProUGUI>();
            mText_CurrentAniCount.text = CurrentAniCount.Value.ToString();
            CurrentAniCount.Subscribe(count =>
            {
                mText_CurrentAniCount.text = count.ToString();
            });
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
                mTeamAgent.speed = (float)CurrentAniCount.Value / MaxAniCount * 2 * Const.CarrySpeed;
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
                switch (ItemType)
                {
                    case PickableItemType.Food:
                        FindObjectOfType<GameRoot>().GameModel.FoodSum.Value += mResourceCount;

                        switch (FoodType)
                        {
                            case FoodType.AddSpeed:
                                AddSpeedFood();
                                break;
                        }

                        break;
                    case PickableItemType.Crystal:
                        FindObjectOfType<GameRoot>().GameModel.CrystalSum.Value += mResourceCount;
                        break;
                    default:
                        break;
                }
                Destroy(gameObject);
            }
        }

        private void AddSpeedFood()
        {
            var player = FindObjectOfType<Player>();
            if (player == null) return;
            Debug.Log("Add Speed");
            player.SetAnisMoveSpeed(Const.FastMoveSpeed);
            player.SetAnimsCarrySpeed(Const.FastCarrySpeed);
            TimerManager.Instance.AddTask(() =>
            {
                Debug.Log("Add Speed End");
                player.SetAnisMoveSpeed(Const.BaseMoveSpeed);
                player.SetAnimsCarrySpeed(Const.BaseCarrySpeed);
            }, 10, 1);
        }

        public Vector3 GetPosition(PICKER_Ani ani)
        {
            return transform.TransformPoint(Positions[mAnis.IndexOf(ani)]);
        }

        public bool CheckCanPick()
        {
            return CurrentAniCount.Value < MaxAniCount;
        }

        public bool CheckCanCarry()
        {
            if (CurrentAniCount.Value > 0 && CurrentAniCount.Value >= MaxAniCount/2)
            {
                foreach (var ani in mAnis)
                {
                    if (!ani.ReadyToCarry) return false;
                }
                return true;
            }
            return false;
        }

        public void AddPickerAni(PICKER_Ani pickerAni)
        {
            mAnis.Add(pickerAni);
            CurrentAniCount.Value++;
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