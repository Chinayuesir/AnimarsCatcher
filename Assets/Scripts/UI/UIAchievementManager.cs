using UnityEngine;

namespace AnimarsCatcher
{
    public enum AchievementType
    {
        Distance
    }

    public class UIAchievementManager : MonoBehaviour
    {
        public static UIAchievementManager Instance { get; private set; }

        public GameObject UIDistance;

        private void Awake()
        {
            Instance = this;
        }

        public void DisplayUI(AchievementType type)
        {
            var target = GetTarget(type);
            target.SetActive(true);
        }

        public void HideUI(AchievementType type)
        {
            var target = GetTarget(type);
            target.SetActive(false);
        }

        private GameObject GetTarget(AchievementType type)
        {
            return type switch
            {
                AchievementType.Distance => UIDistance,
                _ => null
            };
        }  
    }
}
