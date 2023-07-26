using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace AnimarsCatcher
{
    public class FX_Beam : MonoBehaviour
    {
        public IObjectPool<GameObject> BeamPool;

        private IEnumerator OnParticleCollision(GameObject other)
        {
            yield return new WaitForSeconds(2f);
            BeamPool.Release(gameObject);
        }
    }
}