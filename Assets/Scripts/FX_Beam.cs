using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace AnimarsCatcher
{
    public class FX_Beam : MonoBehaviour
    {
        public IObjectPool<GameObject> BeamPool;
        private GameObject mHit;

        private void OnEnable()
        {
            mHit = gameObject;
        }

        private IEnumerator OnParticleCollision(GameObject other)
        {
            if (!mHit.Equals(other))
            {
                mHit = other;
                if (other.CompareTag("FragileItem"))
                {
                    other.GetComponent<FragileItem>().HP.Value -= 10;
                }
            }

            yield return new WaitForSeconds(2f);
            BeamPool.Release(gameObject);
        }
    }
}