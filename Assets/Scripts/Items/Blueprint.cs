using System;
using UnityEngine;

namespace AnimarsCatcher
{
    public class Blueprint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<GameRoot>().GameModel.BlueprintCount.Value++;
                Destroy(gameObject);
            }
        }
    }
}