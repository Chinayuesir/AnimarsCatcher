using System;
using TMPro;
using UnityEngine;

namespace AnimarsCatcher
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        private GameModel mGameModel;
        
        //Text
        public TextMeshProUGUI Text_Day;
        public TextMeshProUGUI Text_LevelTime;
        public TextMeshProUGUI Text_Food;
        public TextMeshProUGUI Text_Crystal;
        public TextMeshProUGUI Text_InTeamAniCount;
        public TextMeshProUGUI Text_OnGroundAniCount;

        private void Awake()
        {
            Instance = this;
        }

        public void Init(GameModel gameModel, ReactiveProperty<int> levelTime)
        {
            mGameModel = gameModel;
            Text_Day.text = gameModel.Day.Value.ToString();
            Text_Food.text = gameModel.FoodSum.Value.ToString();
            Text_Crystal.text = gameModel.CrystalSum.Value.ToString();
            Text_LevelTime.text = levelTime.Value.ToString();
            Text_InTeamAniCount.text = gameModel.InTeamPickerAniCount.Value.ToString();
            Text_OnGroundAniCount.text = (gameModel.PickerAniCount.Value -
                                          gameModel.InTeamPickerAniCount.Value).ToString();
            levelTime.Subscribe(time =>
            {
                Text_LevelTime.text = time.ToString();
            });
        }

        private void Start()
        {
            mGameModel.Day.Subscribe(day =>
            {
                Text_Day.text = day.ToString();
            });
            mGameModel.FoodSum.Subscribe(count =>
            {
                Text_Food.text = count.ToString();
            });
            mGameModel.CrystalSum.Subscribe(count =>
            {
                Text_Crystal.text = count.ToString();
            });
            mGameModel.InTeamPickerAniCount.Subscribe(count =>
            {
                Text_InTeamAniCount.text = count.ToString();
                Text_OnGroundAniCount.text = (mGameModel.PickerAniCount.Value - count).ToString();
            });
            mGameModel.PickerAniCount.Subscribe(count =>
            {
                Text_OnGroundAniCount.text = (count - mGameModel.InTeamPickerAniCount.Value).ToString();
            });
        }
    }
}