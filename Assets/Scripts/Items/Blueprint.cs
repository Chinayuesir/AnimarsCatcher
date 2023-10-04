using System;
using UnityEngine;

namespace AnimarsCatcher
{
    public class Blueprint : MonoBehaviour
    {
        public float rotationSpeed = 100f;

        private void Update()
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<GameRoot>().GameModel.BlueprintCount.Value++;
                Destroy(gameObject);
                UIBlueprintManager.Instance.RemovePointer(transform);
            }
        }
    }
}