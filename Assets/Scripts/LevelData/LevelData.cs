using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher
{
    [Serializable]
    public class LevelData
    {
        public int Day;
        public int FoodNum;
        public int CrystalNum;
        public int X;
        public int Y;
        public int PickerAniCount;
        public int BlasterAniCount;
        public int LevelTime;
    }

    [Serializable]
    public class LevelInfo
    {
        public List<LevelData> LevelDatas;
    }

}
