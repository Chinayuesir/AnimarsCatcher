using System;
using UnityEngine;

namespace AnimarsCatcher
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance { get; private set; }

        private Timer mTimer;

        private void Awake()
        {
            Instance = this;
            mTimer = new Timer();
        }

        private void Update()
        {
            mTimer.Update();
        }

        public void AddTask(Action callback, double delay, int count = 1)
        {
            mTimer.AddTask(id =>
            {
                callback?.Invoke();
                count--;
                if (count <= 0)
                    mTimer.DeleteTask(id);
            }, delay, count);
        }
    }
}
