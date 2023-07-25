using System;
using System.Collections;
using System.IO;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace AnimarsCatcher
{
    public class GameRoot : MonoBehaviour
    {
        private LevelInfo mInfo;
        private Timer mTimer;
        private GameModel mGameModel;
        public GameModel GameModel => mGameModel;
        private AchievementSystem mAchievementSystem;

        private GameObject mPickerAniPrefab;
        private GameObject mBlasterAniPrefab;

        private Transform mHomeTrans;
        public Transform Anis;

        [MenuItem("Tools/Clear Save Data")]
        public static void ClearSaveData()
        {
            PlayerPrefs.DeleteAll();
        }

        private void Awake()
        {
            mHomeTrans = GameObject.FindWithTag("Home").transform;
            mTimer = new Timer();
            mGameModel = new GameModel();
            mAchievementSystem = new AchievementSystem();
            mAchievementSystem.Init(mGameModel);

            mPickerAniPrefab = Resources.Load<GameObject>(ResPath.PickerAniPath);
            mBlasterAniPrefab = Resources.Load<GameObject>(ResPath.BlasterAniPath);
        }

        private void Start()
        {
            string json = File.ReadAllText(ResPath.LevelInfoJson);
             mInfo = JsonUtility.FromJson<LevelInfo>(json);
             if (mGameModel.HasSaveData())
             {
                 mGameModel.Load();
                 LoadLevelFromSaveData();
             }
             else
             {
                 mGameModel.Day.Value = 1;
                 LoadLevel(1);
             }
        }

        private void Update()
        {
            mTimer.Update();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadNextLevel();
            }
        }

        private void LoadNextLevel()
        {
            int day = ++mGameModel.Day.Value;
            LoadLevel(day);
        }

        private void LoadLevel(int day)
        {
            Debug.Log($"load level day: {day}");
            LevelData levelData = mInfo.LevelDatas[day - 1];
            LoadMap(levelData);
            StartCoroutine(SpawnAnis(levelData.PickerAniCount, levelData.BlasterAniCount));
            mGameModel.PickerAniCount.Value += levelData.PickerAniCount;
            mGameModel.BlasterAniCount.Value += levelData.BlasterAniCount;
        }

        private void LoadLevelFromSaveData()
        {
            Debug.Log($"load level from savedata day: {mGameModel.Day}");
            LevelData levelData = mInfo.LevelDatas[mGameModel.Day.Value - 1];
            LoadMap(levelData);
            StartCoroutine(SpawnAnis(mGameModel.PickerAniCount.Value, mGameModel.BlasterAniCount.Value));
        }

        private void LoadMap(LevelData levelData)
        {
            Vector2 mapSize = new Vector2(levelData.X, levelData.Y);
            
            MapManager.Instance.LoadItems(mapSize,levelData.FoodNum,2,ResPath.FoodPrefabPath);
            MapManager.Instance.LoadItems(mapSize,levelData.CrystalNum,2,ResPath.CrystalPrefabPath);
        }

        private IEnumerator SpawnAnis(int pickerAniCount, int blasterAniCount)
        {
            for (int i = 0; i < pickerAniCount; i++)
            {
                yield return new WaitForSeconds(1f);
                var position = new Vector3(mHomeTrans.position.x - Random.Range(1, 3),
                    mHomeTrans.position.y,
                    mHomeTrans.position.z + Random.Range(-3, 3));
                var ani = Instantiate(mPickerAniPrefab, position, quaternion.identity, Anis);
                ani.GetComponent<NavMeshAgent>().SetDestination(position);
            }
            
            for (int i = 0; i < blasterAniCount; i++)
            {
                yield return new WaitForSeconds(1f);
                var position = new Vector3(mHomeTrans.position.x - Random.Range(1, 3),
                    mHomeTrans.position.y,
                    mHomeTrans.position.z + Random.Range(-3, 3));
                var ani = Instantiate(mBlasterAniPrefab, position, quaternion.identity, Anis);
                ani.GetComponent<NavMeshAgent>().SetDestination(position);
            }
        }

        private void OnApplicationQuit()
        {
            mGameModel.Save();
        }
    }
}