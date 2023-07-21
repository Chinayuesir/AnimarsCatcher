using System.IO;
using UnityEngine;

namespace AnimarsCatcher
{
    public class GameRoot : MonoBehaviour
    {
        private void Start()
        {
            string json = File.ReadAllText(ResPath.LevelInfoJson);
            LevelInfo info = JsonUtility.FromJson<LevelInfo>(json);
            foreach (var levelData in info.LevelDatas)
            {
                Debug.Log($"Day:{levelData.Day} "+
                          $"FoodNum:{levelData.FoodNum }"+
                          $"CrystalNum:{levelData.CrystalNum}");
            }
        }
    }
}