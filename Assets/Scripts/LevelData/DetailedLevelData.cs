using System;
using System.Collections.Generic;

namespace AnimarsCatcher
{
    [Serializable]
    public class AreaData
    {
        public int FoodNum;
        public int CrystalNum;
        public List<int> Area;
    }

    [Serializable]
    public class DetailedLevelData
    {
        public int Day;
        public List<AreaData> Resources;
        public int PickerAniCount;
        public int BlasterAniCount;
        public int LevelTime;
    }

    public class DetailedLevelInfo
    {
        public List<DetailedLevelData> LevelDatas;
    }
}
