using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AnimarsCatcher
{
    public enum AniInfoType
    {
        Picker,
        Blaster
    }

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        private GameModel mGameModel;
        private AniInfoType mAniInfoType = AniInfoType.Picker;
        
        //Text
        public TextMeshProUGUI Text_Day;
        public TextMeshProUGUI Text_LevelTime;
        public TextMeshProUGUI Text_Food;
        public TextMeshProUGUI Text_Crystal;
        public TextMeshProUGUI Text_InTeamAniCount;
        public TextMeshProUGUI Text_OnGroundAniCount;
        
        //Button
        public Button RobotIcon;
        public Button Button_ReturnGame;
        public Button Button_QuitGame;
        public Button PickerAniIcon;
        public Button BlasterAniIcon;
        
        //Panel
        public GameObject MenuPanel;

        private Vector3 mBigIconPos;
        private Vector3 mSmallIconPos;
        private Vector2 mBigIconSizeDelta;
        private Vector2 mSmallIconSizeDelta;

        private void Awake()
        {
            Instance = this;
            mBigIconPos = PickerAniIcon.GetComponent<RectTransform>().position;
            mSmallIconPos = BlasterAniIcon.GetComponent<RectTransform>().position;
            mBigIconSizeDelta = PickerAniIcon.GetComponent<RectTransform>().sizeDelta;
            mSmallIconSizeDelta = BlasterAniIcon.GetComponent<RectTransform>().sizeDelta;
        }

        public void Init(GameModel gameModel, ReactiveProperty<int> levelTime)
        {
            mGameModel = gameModel;
            Text_Day.text = gameModel.Day.Value.ToString();
            Text_Food.text = gameModel.FoodSum.Value.ToString();
            Text_Crystal.text = gameModel.CrystalSum.Value.ToString();
            Text_LevelTime.text = levelTime.Value.ToString();
            Text_InTeamAniCount.text = gameModel.InTeamPickerAniCount.Value.ToString();
            Text_OnGroundAniCount.text = (gameModel.PickerAniCount.Value - gameModel.InTeamPickerAniCount.Value).ToString();
            levelTime.Subscribe(time =>
            {
                Text_LevelTime.text = time.ToString();
            });

            RobotIcon.onClick.AddListener(() =>
            {
                MenuPanel.SetActive(true);
                Time.timeScale = 0;
            });

            Button_ReturnGame.onClick.AddListener(() =>
            {
                MenuPanel.SetActive(false);
                Time.timeScale = 1;
            });

            Button_QuitGame.onClick.AddListener(() =>
            {
                Debug.Log("Quit Game");
            });

            SubscribeAniInfo(gameModel.PickerAniCount, gameModel.InTeamPickerAniCount, AniInfoType.Picker);
            SubscribeAniInfo(gameModel.BlasterAniCount, gameModel.InTeamBlasterAniCount, AniInfoType.Blaster);

            PickerAniIcon.onClick.AddListener(() =>
            {
                AniIconBtnClick(PickerAniIcon, BlasterAniIcon);
            });

            BlasterAniIcon.onClick.AddListener(() =>
            {
                AniIconBtnClick(BlasterAniIcon, PickerAniIcon);
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
                Text_Food.text = count.ToString();
            });
            mGameModel.InTeamPickerAniCount.Subscribe(count =>
            {
                Text_InTeamAniCount.text = count.ToString();
                Text_OnGroundAniCount.text = (mGameModel.PickerAniCount.Value - mGameModel.InTeamPickerAniCount.Value).ToString();
            });
            mGameModel.PickerAniCount.Subscribe(count =>
            {
                Text_OnGroundAniCount.text = (count - mGameModel.InTeamPickerAniCount.Value).ToString();
            });
        }

        private void SubscribeAniInfo(ReactiveProperty<int> sumCount, ReactiveProperty<int> inTeamCount, AniInfoType type)
        {
            inTeamCount.Subscribe(count =>
            {
                if (mAniInfoType != type) return;
                Text_InTeamAniCount.text = count.ToString();
                Text_OnGroundAniCount.text = (sumCount.Value - count).ToString();
            });
            sumCount.Subscribe(count =>
            {
                if (mAniInfoType != type) return;
                Text_OnGroundAniCount.text = (count - inTeamCount.Value).ToString();
            });
        }

        private void AniIconBtnClick(Button button1, Button button2)
        {
            switch (mAniInfoType)
            {
                case AniInfoType.Picker:
                    mAniInfoType = AniInfoType.Picker;
                    Text_InTeamAniCount.text = mGameModel.InTeamPickerAniCount.Value.ToString();
                    Text_OnGroundAniCount.text = (mGameModel.PickerAniCount.Value - mGameModel.InTeamPickerAniCount.Value).ToString();
                    break;
                case AniInfoType.Blaster:
                    mAniInfoType = AniInfoType.Blaster;
                    Text_InTeamAniCount.text = mGameModel.InTeamBlasterAniCount.Value.ToString();
                    Text_OnGroundAniCount.text = (mGameModel.BlasterAniCount.Value - mGameModel.InTeamBlasterAniCount.Value).ToString();
                    break;
            }

            button1.GetComponent<RectTransform>().DOMove(mSmallIconPos, 0.3f);
            button1.GetComponent<RectTransform>().DOSizeDelta(mSmallIconSizeDelta, 0.3f);
            button1.enabled = false;
            button2.GetComponent<RectTransform>().DOMove(mBigIconPos, 0.3f);
            button2.GetComponent<RectTransform>().DOSizeDelta(mBigIconSizeDelta, 0.3f);
            button2.enabled = true;
        }
    }
}