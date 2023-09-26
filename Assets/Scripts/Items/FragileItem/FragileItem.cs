using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AnimarsCatcher
{
    public interface ICanShoot
    {
        bool CheckCanShoot(Vector3 position);
        bool HasDestroyed();
    }

    public class FragileItem : MonoBehaviour,ICanShoot, IResource
    {
        [SerializeField]
        private int mResourceCount;
        public int ResourceCount => mResourceCount;
        public ReactiveProperty<int> HP = new ReactiveProperty<int>(100);
        public List<GameObject> PickableCrystal;

        private LayerMask mMask;

        private LayerMask mSelfLayerMask;

        private void Awake()
        {
            mMask = (1 << LayerMask.NameToLayer("Ani")) | (1 << LayerMask.NameToLayer("Player"));
            mMask = ~mMask;
            mSelfLayerMask = gameObject.layer;
        }

        private void Start()
        {
            HP.Subscribe(hp =>
            {
                if (hp <= 0)
                {
                    var go = Instantiate(PickableCrystal[Random.Range(0, PickableCrystal.Count)], transform.position, Quaternion.identity);
                    go.transform.localScale = 3 * Vector3.one;
                    Destroy(gameObject);
                }
            });
        }

        public bool CheckCanShoot(Vector3 position)
        {
            Vector3 dir = transform.position - position;
            Physics.Raycast(position, dir, out var hitInfo, 30, mMask);
            if (hitInfo.transform != null)
                return hitInfo.transform.CompareTag("FragileItem");
            return false;
        }

        public bool HasDestroyed()
        {
            return HP.Value <= 0;
        }

        private void OnMouseEnter()
        {
            gameObject.layer = LayerMask.NameToLayer("SelectedObject");
        }

        private void OnMouseExit()
        {
            gameObject.layer = mSelfLayerMask;
        }
    }
}
