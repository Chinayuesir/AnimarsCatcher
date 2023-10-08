using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace AnimarsCatcher
{
    public class GameRoot : MonoBehaviour
    {
        //private LevelInfo mInfo;
        private DetailedLevelInfo mInfo;
        private Timer mTimer;
        private GameModel mGameModel;
        public GameModel GameModel => mGameModel;
        private AchievementSystem mAchievementSystem;

        private GameObject mPickerAniPrefab;
        private GameObject mBlasterAniPrefab;
        private ReactiveProperty<int> mLevelTime;

        private Transform mHomeTrans;
        public Transform Anis;
        public Animator EnvironmentAnimator;
        public Transform BluePrints;

#if UNITY_EDITOR
        [MenuItem("Tools/Clear Save Data")]
        public static void ClearSaveData()
        {
            PlayerPrefs.DeleteAll();
        }
#endif

        private void Awake()
        {
            mHomeTrans = GameObject.FindWithTag("Home").transform;
            mTimer = new Timer();
            mGameModel = new GameModel();
            mAchievementSystem = new AchievementSystem();
            mAchievementSystem.Init(mGameModel);
            mLevelTime = new ReactiveProperty<int>(60);
            UIManager.Instance.Init(mGameModel, mLevelTime);

            mPickerAniPrefab = Resources.Load<GameObject>(ResPath.PickerAniPath);
            mBlasterAniPrefab = Resources.Load<GameObject>(ResPath.BlasterAniPath);
        }

        private void Start()
        {
            //string json = File.ReadAllText(ResPath.LevelInfoJson);
            //mInfo = JsonUtility.FromJson<LevelInfo>(json);
            string json = File.ReadAllText(ResPath.DetailedInfoJson);
            mInfo = JsonUtility.FromJson<DetailedLevelInfo>(json);
            if (mGameModel.HasSaveData())
            {
                mGameModel.Load();
                LoadLevelFromSaveData();
            }
            else
            {
                mGameModel.Day.Value = 1;
                mGameModel.InTeamPickerAniCount.Value = 0;
                mGameModel.InTeamBlasterAniCount.Value = 0;
                LoadLevel(1);
            }
            GameModel.BlueprintCount.Subscribe(count =>
            {
                if (count == 9)
                {
                    Debug.Log("Mission Complete!");
                }
            });
        }

        private void Update()
        {
            mTimer.Update();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadNextLeval();
            }
        }

        private void LoadNextLeval()
        {
            int day = ++mGameModel.Day.Value;
            LoadLevel(day);
        }

        private void LoadLevel(int day)
        {
            Debug.Log($"load level day: {day}");
            DetailedLevelData levelData = mInfo.LevelDatas[day - 1];
            LoadMap(levelData);
            StartCoroutine(SpawnAnis(levelData.PickerAniCount, levelData.BlasterAniCount));
            mGameModel.PickerAniCount.Value += levelData.PickerAniCount;
            mGameModel.BlasterAniCount.Value += levelData.BlasterAniCount;

            EnvironmentAnimator.Rebind();
            EnvironmentAnimator.Play("Light");
            StartTimer(levelData.LevelTime);
        }

        private void LoadLevelFromSaveData()
        {
            Debug.Log($"load level from savedata day: {mGameModel.Day}");
            DetailedLevelData levelData = mInfo.LevelDatas[mGameModel.Day.Value - 1];
            LoadMap(levelData);
            StartCoroutine(SpawnAnis(mGameModel.PickerAniCount.Value, mGameModel.BlasterAniCount.Value));
            EnvironmentAnimator.Rebind();
            EnvironmentAnimator.Play("Light");
            StartTimer(levelData.LevelTime);
        }

        private void LoadMap(LevelData levelData)
        {
            Vector2 mapSize = new Vector2(levelData.X, levelData.Y);

            MapManager.Instance.LoadItems(mapSize, levelData.FoodNum, 2, ResPath.FoodPrefabPath);
            MapManager.Instance.LoadItems(mapSize, levelData.CrystalNum, 2, ResPath.CrystalPrefabPath);

        }

        private void LoadMap(DetailedLevelData levelData)
        {
            foreach (var res in levelData.Resources)
            {
                Vector2 mapPosition = new(res.Area[0], res.Area[1]);
                Vector2 mapSize = new(res.Area[2], res.Area[3]);
                MapManager.Instance.LoadItems(mapPosition, mapSize, res.FoodNum, 2, ResPath.FoodPrefabPath);
                MapManager.Instance.LoadItems(mapPosition, mapSize, res.CrystalNum, 2, ResPath.CrystalPrefabPath);
            }
        }

        private void StartTimer(int seconds)
        {
            mLevelTime.Value = seconds;
            mTimer.AddTask(id =>
            {
                mLevelTime.Value -= 1;
                if (mLevelTime.Value <= 0)
                {
                    //LoadNextLeval();
                    UIManager.Instance.OpenResourcePanel();
                    mTimer.DeleteTask(id);
                }
            }, 1, mLevelTime.Value);

            mTimer.AddTask(id => 
            {
                int random = Random.Range(0, 2);
                if (random == 0)
                {
                    GetOneBlueprint();
                    mTimer.DeleteTask(id);
                }
            }, 30, 1);
        }

        private void GetOneBlueprint()
        {
            int childCount = BluePrints.childCount;
            if (childCount != 0)
            {
                var child = BluePrints.GetChild(Random.Range(0, BluePrints.childCount));
                if (!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                    child.gameObject.AddComponent<Blueprint>();
                    UIBlueprintManager.Instance.AddPointer(child.transform);
                }
            }
        }

        private IEnumerator SpawnAnis(int pickerAniCount, int blasterAniCount)
        {
            for (int i = 0; i < pickerAniCount; i++)
            {
                yield return new WaitForSeconds(1f);
                var position = new Vector3(mHomeTrans.position.x - Random.Range(1, 3),
                    mHomeTrans.position.y,
                    mHomeTrans.position.z + Random.Range(-3, 3));
                var ani = Instantiate(mPickerAniPrefab, position, Quaternion.identity, Anis);
                ani.GetComponent<NavMeshAgent>().SetDestination(position);
            }

            for (int i = 0; i < blasterAniCount; i++)
            {
                yield return new WaitForSeconds(1f);
                var position = new Vector3(mHomeTrans.position.x - Random.Range(1, 3),
                    mHomeTrans.position.y,
                    mHomeTrans.position.z + Random.Range(-3, 3));
                var ani = Instantiate(mBlasterAniPrefab, position, Quaternion.identity, Anis);
                ani.GetComponent<NavMeshAgent>().SetDestination(position);
            }
        }

        public bool TryLoadNextLevel(int deltaPickerAniCount, int deltaBlasterAniCount)
        {
            int day = mGameModel.Day.Value + 1;
            if (day > mInfo.LevelDatas.Count) return false;
            mInfo.LevelDatas[day - 1].PickerAniCount += deltaPickerAniCount;
            mInfo.LevelDatas[day - 1].BlasterAniCount += deltaBlasterAniCount;
            LoadNextLeval();
            return true;
        }
    }
}