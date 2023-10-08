using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace AnimarsCatcher
{
    public class PlayerScore : NetworkBehaviour
    {
        private int mScore = 0;

        public void ChangeScore(int scoreChange)
        {
            if (!IsServer)
            {
                return;
            }
            mScore += scoreChange;
            UpdatePlayerScoreClientRpc(mScore);
        }

        [ClientRpc]
        private void UpdatePlayerScoreClientRpc(int num)
        {
            if (IsClient && IsOwner)
            {
                NetworkUI.PlayerScore = num;
            }
        }
    }

}
