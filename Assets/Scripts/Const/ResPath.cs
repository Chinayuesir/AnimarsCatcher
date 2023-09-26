using UnityEngine;

namespace AnimarsCatcher
{
    public static class ResPath
    {
        public static readonly string LevelInfoJson = Application.streamingAssetsPath
                                                      + "/LevelInfo.json";
        public static readonly string PickerAniPath = "PICKER_ANI";
        public static readonly string BlasterAniPath = "BLASTER_ANI";
        public static readonly string FoodPrefabPath = "Fruits";
        public static readonly string CrystalPrefabPath = "Crystals";
        public static readonly string FX_BeamPrefabPath = "FX_Beam";
    }
}