using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


namespace AnimarsCatcher
{
    public class NetFruit : NetworkBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!IsServer)
            {
                return;
            }
            
            other.SendMessage("ChangeScore",1,SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
            FruitSpawner.FruitNum--;
        }
    }
}

