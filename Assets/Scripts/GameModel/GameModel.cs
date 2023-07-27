using UnityEngine;

namespace AnimarsCatcher
{
    public class GameModel
    {
        public ReactiveProperty<int> Day=new ReactiveProperty<int>();
        public ReactiveProperty<int> PickerAniCount=new ReactiveProperty<int>();
        public ReactiveProperty<int> BlasterAniCount=new ReactiveProperty<int>();
        public ReactiveProperty<int> FoodSum=new ReactiveProperty<int>();
        public ReactiveProperty<int> CrystalSum=new ReactiveProperty<int>();

        public bool HasSaveData()
        {
            return PlayerPrefs.HasKey(nameof(Day));
        }

        public void Load()
        {
            Day.Value = PlayerPrefs.GetInt(nameof(Day));
            PickerAniCount.Value = PlayerPrefs.GetInt(nameof(PickerAniCount));
            BlasterAniCount.Value = PlayerPrefs.GetInt(nameof(BlasterAniCount));
            FoodSum.Value=PlayerPrefs.GetInt(nameof(FoodSum));
            CrystalSum.Value=PlayerPrefs.GetInt(nameof(CrystalSum));
        }

        public void Save()
        {
            PlayerPrefs.SetInt(nameof(Day),Day.Value);
            PlayerPrefs.SetInt(nameof(PickerAniCount),PickerAniCount.Value);
            PlayerPrefs.SetInt(nameof(BlasterAniCount),BlasterAniCount.Value);
            PlayerPrefs.SetInt(nameof(FoodSum),FoodSum.Value);
            PlayerPrefs.SetInt(nameof(CrystalSum),CrystalSum.Value);
        }
    }
}