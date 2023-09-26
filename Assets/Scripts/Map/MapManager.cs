using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnimarsCatcher
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private float mMapMaxHeight = 20;
        private int mRandomMaxCount = 20;
        private string mTerrainTag = "Plane";
        private Transform mParentTransform;
        private List<Transform> mItemList = new List<Transform>();
        private Queue<LoadTask> mLoadTasks = new Queue<LoadTask>();
        
        private struct LoadTask
        {
            public string path;
            public Vector2 mapPosition;
            public Vector2 mapSize;
            public int count;
            public float minDistance;
        }

        public void LoadItems(Vector2 mapSize, int count, float minDistance, string path)
        {
            mLoadTasks.Enqueue(new LoadTask()
            {
                path = path,
                mapSize =mapSize,
                count = count,
                minDistance = minDistance
            });
            if(mLoadTasks.Count==1)
                StartNextLoadTask();
        }

        private void StartNextLoadTask()
        {
            if(mLoadTasks.Count==0) return;

            var task = mLoadTasks.Dequeue();
            mItemList.Clear();
            if (mParentTransform == null)
            {
                mParentTransform = new GameObject("Items").transform;
            }

            List<GameObject> currentTaskPrefabs = Resources.LoadAll<GameObject>(task.path).ToList();
            if (currentTaskPrefabs.Count == 0)
            {
                Debug.LogError("Prefab Resources Fail or PrefabName is Empty");
                return;
            }

            StartCoroutine(LoadItemsCoroutine(task.mapPosition, task.mapSize, task.count, task.minDistance, currentTaskPrefabs));
        }

        IEnumerator LoadItemsCoroutine(Vector2 mapSize, int count, float minDistance, List<GameObject> prefabList)
        {
            yield return null;
            int resourceCount = 0;
            while (resourceCount < count)
            {
                var itemPrefab = prefabList[Random.Range(0, prefabList.Count)].transform;
                if (itemPrefab.TryGetComponent<IResource>(out var resource))
                    resourceCount += resource.ResourceCount;
                Instantiate(itemPrefab, mParentTransform).localPosition = GetRandomPosition(mapSize, minDistance);
                mItemList.Add(itemPrefab);
                yield return null;
            }
            StartNextLoadTask();
        }

        Vector3 GetRandomPosition(Vector2 mapSize,float minDistance)
        {
            for (int i = 0; i < mRandomMaxCount; i++)
            {
                Vector3 randomPos = new Vector3(Random.Range(0, mapSize.x)
                    , mMapMaxHeight + 10, Random.Range(0, mapSize.y));
                bool isTooClose = false;
                foreach (var item in mItemList)
                {
                    if (Vector3.Distance(new Vector3(randomPos.x,0,randomPos.z),
                            new Vector3(item.position.x, 0, item.position.z)) < minDistance)
                    {
                        isTooClose = true;
                        break;
                    }
                }

                if (!isTooClose)
                {
                    Ray ray = new Ray(randomPos, Vector3.down);
                    if (Physics.Raycast(ray, out RaycastHit hit)
                        && hit.transform.CompareTag(mTerrainTag)
                        && hit.point.y < 1)
                    {
                        return new Vector3(randomPos.x, hit.point.y, randomPos.z);
                    }
                }
            }
            Debug.LogError("Failed to find a suitable random position after maximum attempts");
            return new Vector3(Random.Range(0, mapSize.x), 0, Random.Range(0, mapSize.y));
        }

        #region Load Items With Area
        public void LoadItems(Vector2 mapPosition, Vector2 mapSize, int count, float minDistance, string path)
        {
            mLoadTasks.Enqueue(new LoadTask()
            {
                path = path,
                mapPosition = mapPosition,
                mapSize = mapSize,
                count = count,
                minDistance = minDistance
            });
            if (mLoadTasks.Count == 1)
                StartNextLoadTask();
        }

        IEnumerator LoadItemsCoroutine(Vector2 mapPosition, Vector2 mapSize, int count, float minDistance, List<GameObject> prefabList)
        {
            yield return null;
            int resourceCount = 0;
            while (resourceCount < count)
            {
                var itemPrefab = prefabList[Random.Range(0, prefabList.Count)].transform;
                if (itemPrefab.TryGetComponent<IResource>(out var resource))
                    resourceCount += resource.ResourceCount;
                Instantiate(itemPrefab, mParentTransform).localPosition = GetRandomPosition(mapPosition, mapSize, minDistance);
                mItemList.Add(itemPrefab);
                yield return null;
            }
            StartNextLoadTask();
        }

        Vector3 GetRandomPosition(Vector2 mapPosition, Vector2 mapSize, float minDistance)
        {
            for (int i = 0; i < mRandomMaxCount; i++)
            {
                Vector3 randomPos = new Vector3(Random.Range(mapPosition.x, mapPosition.x + mapSize.x)
                    , mMapMaxHeight + 10, Random.Range(mapPosition.y, mapPosition.y + mapSize.y));
                bool isTooClose = false;
                foreach (var item in mItemList)
                {
                    if (Vector3.Distance(new Vector3(randomPos.x, 0, randomPos.z),
                            new Vector3(item.position.x, 0, item.position.z)) < minDistance)
                    {
                        isTooClose = true;
                        break;
                    }
                }

                if (!isTooClose)
                {
                    Ray ray = new Ray(randomPos, Vector3.down);
                    if (Physics.Raycast(ray, out RaycastHit hit)
                        && hit.transform.CompareTag(mTerrainTag)
                        && hit.point.y < 1)
                    {
                        return new Vector3(randomPos.x, hit.point.y, randomPos.z);
                    }
                }
            }
            Debug.LogError("Failed to find a suitable random position after maximum attempts");
            return new Vector3(Random.Range(0, mapSize.x), 0, Random.Range(0, mapSize.y));
        }
        #endregion
    }
}