using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnimarsCatcher
{
    public class AniController:NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        private void Start()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        private void Update()
        {
            transform.position = Position.Value;
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPos = GetRandomPosition();
                transform.position = randomPos;
                Position.Value = transform.position;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        static Vector3 GetRandomPosition()
        {
            return new Vector3(Random.Range(52, 55f), 0f, Random.Range(33, 37f));
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPosition();
        }
    }
}