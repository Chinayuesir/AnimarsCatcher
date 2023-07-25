using UnityEngine;

namespace AnimarsCatcher
{
    public class GameModel
    {
        public int Day;
        public int PickerAniCount;
        public int BlasterAniCount;

        public bool HasSaveData()
        {
            return PlayerPrefs.HasKey(nameof(Day));
        }

        public void Load()
        {
            Day = PlayerPrefs.GetInt(nameof(Day));
            PickerAniCount = PlayerPrefs.GetInt(nameof(PickerAniCount));
            BlasterAniCount = PlayerPrefs.GetInt(nameof(BlasterAniCount));
        }

        public void Save()
        {
            PlayerPrefs.SetInt(nameof(Day),Day);
            PlayerPrefs.SetInt(nameof(PickerAniCount),PickerAniCount);
            PlayerPrefs.SetInt(nameof(BlasterAniCount),BlasterAniCount);
        }
    }
}