using System;
using System.IO;
using UnityEngine;

namespace AnimarsCatcher
{
    public class GameRoot : MonoBehaviour
    {
        private LevelInfo mInfo;
        private Timer mTimer;
        
        public int Day = 1;
        private void Start()
        {
            string json = File.ReadAllText(ResPath.LevelInfoJson);
             mInfo = JsonUtility.FromJson<LevelInfo>(json);
            LoadMap(Day);

            mTimer = new Timer();
            int count = 1;
            int id =mTimer.AddTask(id =>
            {
                Debug.Log(10-count);
                count++;
            },1,10);
            mTimer.AddTask(_ =>
            {
               mTimer.DeleteTask(id);
            }, 5, 1);
        }

        private void Update()
        {
            mTimer.Update();
        }

        private void LoadMap(int day)
        {
            LevelData levelData = mInfo.LevelDatas[day - 1];
            Vector2 mapSize = new Vector2(levelData.X, levelData.Y);
            
            MapManager.Instance.LoadItems(mapSize,levelData.FoodNum,2,"Fruits");
            MapManager.Instance.LoadItems(mapSize,levelData.CrystalNum,2,"Crystals");
        }
    }
}