using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher
{
    public class UIBlueprintManager : MonoBehaviour
    {
        public static UIBlueprintManager Instance { get; private set; }

        public GameObject PositionPointer;
        public Transform Player;
        public Transform MinimapPanel;
        public Transform Minimap;

        private Dictionary<Transform, UIBlueprintPointer> mPointers = new();

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            foreach (var item in mPointers.Values)
            {
                item.Update();
            }
        }

        public void AddPointer(Transform blueprint)
        {
            var pointer = Instantiate(PositionPointer, MinimapPanel).transform;
            pointer.position = Minimap.position;
            mPointers.Add(blueprint, new UIBlueprintPointer(pointer, Player, blueprint));
        }

        public void RemovePointer(Transform blueprint)
        {
            Destroy(mPointers[blueprint].PositionPointer.gameObject);
            mPointers.Remove(blueprint);
        }
    }
}
