using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnimarsCatcher
{
    public class FruitSpawner : NetworkBehaviour
    {
        public GameObject NetFruitPrefab;
        public int SpawnCount = 5;

        public static int FruitNum = 0;

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += () =>
            {
                RandomSpawn();
            };
        }

        private void Update()
        {
            if(FruitNum==0) RandomSpawn();
        }

        public void RandomSpawn()
        {
            if(!IsServer) return;
            for (int i = 0; i < SpawnCount; i++)
            {
                GameObject fruit = Instantiate(NetFruitPrefab,
                    new Vector3(Random.Range(100, 190),
                        0, Random.Range(10, 40)), Quaternion.identity);
                fruit.GetComponent<NetworkObject>().Spawn();
            }

            FruitNum += SpawnCount;
        }
    }

}
