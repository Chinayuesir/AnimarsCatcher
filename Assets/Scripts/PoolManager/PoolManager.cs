using System;
using UnityEngine;
using UnityEngine.Pool;

namespace AnimarsCatcher
{
    public class PoolManager:MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        public bool CollectionChecks = true;
        public int MaxPoolSize = 20;

        private IObjectPool<GameObject> m_BeamPool;
        public IObjectPool<GameObject> BeamPool
        {
            get
            {
                if (m_BeamPool == null)
                {
                    m_BeamPool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, CollectionChecks, 10, MaxPoolSize);
                }
                return m_BeamPool;
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        GameObject CreatePooledItem()
        {
            var go = Instantiate(Resources.Load<GameObject>(ResPath.FX_BeamPrefabPath));
            var fxBeam = go.AddComponent<FX_Beam>();
            fxBeam.BeamPool = BeamPool;
            return go;
        }

        private void OnReturnedToPool(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        private void OnTakeFromPool(GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        private void OnDestroyPoolObject(GameObject gameObject)
        {
            Destroy(gameObject);
        }
    }
}