using UnityEngine;

namespace AnimarsCatcher
{
    public class GameModel
    {
        public ReactiveProperty<int> Day = new();
        public ReactiveProperty<int> PickerAniCount = new();
        public ReactiveProperty<int> BlasterAniCount = new();
        public ReactiveProperty<int> FoodSum = new();
        public ReactiveProperty<int> CrystalSum = new();
        public ReactiveProperty<int> BlueprintCount = new();

        public bool HasSaveData()
        {
            return PlayerPrefs.HasKey(nameof(Day));
        }

        public void Load()
        {
            Day.Value = PlayerPrefs.GetInt(nameof(Day));
            PickerAniCount.Value = PlayerPrefs.GetInt(nameof(PickerAniCount));
            BlasterAniCount.Value = PlayerPrefs.GetInt(nameof(BlasterAniCount));
            FoodSum.Value = PlayerPrefs.GetInt(nameof(FoodSum));
            CrystalSum.Value = PlayerPrefs.GetInt(nameof(CrystalSum));
            BlueprintCount.Value = PlayerPrefs.GetInt(nameof(BlueprintCount));
        }

        public void Save()
        {
            PlayerPrefs.SetInt(nameof(Day), Day.Value);
            PlayerPrefs.SetInt(nameof(PickerAniCount), PickerAniCount.Value);
            PlayerPrefs.SetInt(nameof(BlasterAniCount), BlasterAniCount.Value);
            PlayerPrefs.SetInt(nameof(FoodSum), FoodSum.Value);
            PlayerPrefs.SetInt(nameof(CrystalSum), CrystalSum.Value);
            PlayerPrefs.SetInt(nameof(BlueprintCount), BlueprintCount.Value);
        }
    }
}