using UnityEngine;

namespace AnimarsCatcher
{
    public class AchievementSystem
    {
        public void Init(GameModel gameModel)
        {
            gameModel.FoodSum.Subscribe(value =>
            {
                Debug.Log(value);
                if (value == 1)
                {
                    Debug.Log("First Bite of the Food");
                }else if (value == 10)
                {
                    Debug.Log("Dining Room Hero!");
                }else if (value == 100)
                {
                    Debug.Log("Overload of the Delicacies!");
                }
            });
        }
    }
}