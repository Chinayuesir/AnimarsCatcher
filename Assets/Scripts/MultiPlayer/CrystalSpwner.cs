using Unity.Netcode;
using UnityEngine;

namespace AnimarsCatcher
{
    public class CrystalSpwner : NetworkBehaviour
    {
        public GameObject NetCrystalPrefab;
        public int SpawnCount = 5;

        public static int CrystalNum = 0;

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += () =>
            {
                RandomSpawn();
            };
        }

        private void Update()
        {
            if (CrystalNum == 0) RandomSpawn();
        }

        public void RandomSpawn()
        {
            if (!IsServer) return;
            for (int i = 0; i < SpawnCount; i++)
            {
                GameObject fruit = Instantiate(NetCrystalPrefab,
                    new Vector3(Random.Range(100, 190),
                        0, Random.Range(10, 40)), Quaternion.identity);
                fruit.GetComponent<NetworkObject>().Spawn();
            }

            CrystalNum += SpawnCount;
        }
    }
}
