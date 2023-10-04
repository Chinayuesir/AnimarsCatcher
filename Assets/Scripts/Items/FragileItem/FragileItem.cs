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
        public int MaxHP = 100;
        public ReactiveProperty<int> HP;
        public List<GameObject> PickableCrystal;

        private LayerMask mMask;

        private LayerMask mSelfLayerMask;
        private HPBar mHpBar;

        private void Awake()
        {
            HP = new ReactiveProperty<int>(MaxHP);

            mMask = (1 << LayerMask.NameToLayer("Ani")) | (1 << LayerMask.NameToLayer("Player"));
            mMask = ~mMask;
            mSelfLayerMask = gameObject.layer;
            mHpBar = transform.Find("HPCanvas/HPBarBg/HPBar").GetComponent<HPBar>();
            mHpBar.Init(HP);
        }

        private void Start()
        {
            HP.Subscribe(hp =>
            {
                if (hp <= 0)
                {
                    for (int i = 0; i < mResourceCount; i++)
                    {
                        Vector3 pos = new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y, transform.position.z + Random.Range(-3f, 3f));
                        var go = Instantiate(PickableCrystal[Random.Range(0, PickableCrystal.Count)], pos, Quaternion.identity);
                        go.transform.localScale = 3 * Vector3.one;
                    }
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
