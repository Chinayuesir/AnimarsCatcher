using System;
using System.Collections.Generic;

namespace AnimarsCatcher
{
    class TimeTask
    {
        public int taskID;
        public Action<int> callback;
        public double destTime;
        public double delay;
        public int count;

        public TimeTask(int taskID,Action<int> callback,double destTime,double delay,int count)
        {
            this.taskID = taskID;
            this.callback = callback;
            this.delay = delay;
            this.destTime = destTime;
            this.count = count;
        }
    }

    public class Timer
    {
        private DateTime mStartDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private double mNowTime;
        private int mTaskID;
        private List<int> mTaskIDList = new List<int>();
        private List<int> mRecycleTaskIDList = new List<int>();

        private List<TimeTask> mTempTimeTaskList = new List<TimeTask>();
        private List<TimeTask> mTaskTimeList = new List<TimeTask>();
        private List<int> mTempDeleteTimeTaskList = new List<int>();

        public Timer()
        {
            Reset();
        }

        public void Update()
        {
            CheckTimeTask();
            DeleteTimeTask();

            if (mRecycleTaskIDList.Count > 0)
            {
                RecycleTaskID();
            }
        }

        public int AddTask(Action<int> callback, double delay, int count = 1)
        {
            int taskID = GetTaskID();
            mNowTime = GetUTCSeconds();
            mTempTimeTaskList.Add(new TimeTask(taskID,callback,mNowTime+delay,delay,count));
            return taskID;
        }

        public void DeleteTask(int taskID)
        {
            mTempDeleteTimeTaskList.Add(taskID);
        }


        private void CheckTimeTask()
        {
            mTaskTimeList.AddRange(mTempTimeTaskList);
            mTempTimeTaskList.Clear();

            mNowTime = GetUTCSeconds();
            mTaskTimeList.RemoveAll(task =>
            {
                if (mNowTime >= task.destTime)
                {
                    task.callback?.Invoke(task.taskID);
                    if (task.count > 1)
                    {
                        task.count -= 1;
                        task.destTime += task.delay;
                        return false;
                    }
                    else
                    {
                        mRecycleTaskIDList.Add(task.taskID);
                        return true;
                    }
                }

                return false;
            });
        }

        private void DeleteTimeTask()
        {
            foreach (var deleteTaskID in mTempDeleteTimeTaskList)
            {
                TimeTask task = mTaskTimeList.Find(t => t.taskID == deleteTaskID);
                if (task != null)
                {
                    mTaskTimeList.Remove(task);
                    mRecycleTaskIDList.Add(deleteTaskID);
                }
                
                task= mTempTimeTaskList.Find(t => t.taskID == deleteTaskID);
                if (task != null)
                {
                    mTempTimeTaskList.Remove(task);
                    mRecycleTaskIDList.Add(deleteTaskID);
                }
            }
            mTempDeleteTimeTaskList.Clear();
        }

        private void Reset()
        {
            mTaskID = 0;
            mTaskTimeList.Clear();
            mRecycleTaskIDList.Clear();
            mTempTimeTaskList.Clear();
            mTaskTimeList.Clear();
        }

        private int GetTaskID()
        {
            mTaskID += 1;
            while (mTaskIDList.Contains(mTaskID))
            {
                if (mTaskID == int.MaxValue) mTaskID = 0;
                mTaskID += 1;
            }
            mTaskIDList.Add(mTaskID);
            return mTaskID;
        }

        private void RecycleTaskID()
        {
            mRecycleTaskIDList.ForEach(taskID=>mTaskIDList.Remove(taskID));
            mRecycleTaskIDList.Clear();
        }

        private double GetUTCSeconds()
        {
            return (DateTime.UtcNow - mStartDateTime).TotalSeconds;
        }
    }
}