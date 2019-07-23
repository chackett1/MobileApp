using ProPooling;
using UnityEngine;

namespace ProPooling.Tutorial
{
    public class OnSpawnAddForce : MonoBehaviour, IPoolComponent
    {
        // Exposed in the editor so we can set teh force.
        public Vector3 Force;

        Rigidbody _rigidbody;

        void Awake()
        {
            // cache Rigidbody for speed.
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void OnDespawned(PoolItem poolItem)
        {
            // We could have reset the rigidbody here, but we already have a 
            // component to do that so here we do nothing in OnDespawned
        }

        public void OnSpawned(PoolItem poolItem)
        {
            // When spawned add the given force.
            _rigidbody.AddForce(Force, ForceMode.Impulse);
        }
    }
}