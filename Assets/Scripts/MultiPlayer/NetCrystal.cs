using Unity.Netcode;
using UnityEngine;

namespace AnimarsCatcher
{
    public class NetCrystal : NetworkBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer)
            {
                return;
            }

            other.SendMessage("ChangeScore", -1, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
            CrystalSpwner.CrystalNum--;
        }
    }
}
