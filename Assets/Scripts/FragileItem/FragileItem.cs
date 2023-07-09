using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimarsCatcher
{
    public interface ICanShoot
    {
        bool CheckCanShoot(Vector3 position);
        bool HasDestroyed();
    }

    public class FragileItem : MonoBehaviour,ICanShoot
    {
        private LayerMask mMask;
        public bool IsDestroyed = false;

        private void Awake()
        {
            mMask = (1 << LayerMask.NameToLayer("Ani")) | (1 << LayerMask.NameToLayer("Player"));
            mMask = ~mMask;
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
            return IsDestroyed;
        }
    }
}
