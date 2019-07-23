//----------------------------------------------
// Flip Web Apps: Pro Pooling
// Copyright © 2016-2017 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProPooling.Components
{
    /// <summary>
    /// Component to automatically return the gameobjects back to the pool after a collision.
    /// </summary>
    [AddComponentMenu("Pro Pooling/Despawn After Collision", 2)]
    [HelpURL("http://www.flipwebapps.com/pro-pooling/")]
    public class DespawnAfterCollision : MonoBehaviour, IPoolComponent
    {
        /// <summary>
        /// Respond to CollisionEnter events.
        /// </summary>
        public bool CollisionEnter { get { return _collisionEnter; } set { _collisionEnter = value; } }
        [Tooltip("Respond to CollisionEnter events.")]
        [SerializeField]
        bool _collisionEnter = true;

        /// <summary>
        /// Respond to CollisionStay events.
        /// </summary>
        public bool CollisionStay { get { return _collisionStay; } set { _collisionStay = value; } }
        [Tooltip("Respond to CollisionStay events.")]
        [SerializeField]
        bool _collisionStay;

        /// <summary>
        /// Respond to CollisionExit events.
        /// </summary>
        public bool CollisionExit { get { return _collisionExit; } set { _collisionExit = value; } }
        [Tooltip("Respond to CollisionExit events.")]
        [SerializeField]
        bool _collisionExit;

        /// <summary>
        /// Respond to TriggerEnter events.
        /// </summary>
        public bool TriggerEnter { get { return _triggerEnter; } set { _triggerEnter = value; } }
        [Tooltip("Respond to TriggerEnter events.")]
        [SerializeField]
        bool _triggerEnter = true;

        /// <summary>
        /// Respond to TriggerStay events.
        /// </summary>
        public bool TriggerStay { get { return _triggerStay; } set { _triggerStay = value; } }
        [Tooltip("Respond to TriggerStay events.")]
        [SerializeField]
        bool _triggerStay;

        /// <summary>
        /// Respond to TriggerExit events.
        /// </summary>
        public bool TriggerExit { get { return _triggerExit; } set { _triggerExit = value; } }
        [Tooltip("Respond to TriggerExit events.")]
        [SerializeField]
        bool _triggerExit;

        /// <summary>
        /// A list of tags with which to collide with
        /// </summary>
        public List<string> Tags { get { return _tags; } set { _tags = value; } }
        [Tooltip("A list of tags with which to collide with")]
        [SerializeField]
        List<string> _tags;

        PoolItem _poolItem;

        #region IPoolComponent

        /// <summary>
        /// When spawned from the pool, start a coroutine to despawn the item after the specified delay.
        /// </summary>
        /// <param name="poolItem"></param>
        public void OnSpawned(PoolItem poolItem)
        {
            _poolItem = poolItem;
        }


        public void OnDespawned(PoolItem poolItem)
        {
        }

        #endregion IPoolComponent

        #region Trigger / Collision Monobehaviour Methods

        void OnTriggerEnter(Collider otherCollider)
        {
            if (TriggerEnter && Tags.Contains(otherCollider.tag))
                _poolItem.DespawnSelf();
        }

        void OnTriggerStay(Collider otherCollider)
        {
            if (TriggerStay && Tags.Contains(otherCollider.tag))
                _poolItem.DespawnSelf();
        }

        void OnTriggerExit(Collider otherCollider)
        {
            if (TriggerExit && Tags.Contains(otherCollider.tag))
                _poolItem.DespawnSelf();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (CollisionEnter && Tags.Contains(collision.gameObject.tag))
                _poolItem.DespawnSelf();
        }

        void OnCollisionStay(Collision collision)
        {
            if (CollisionStay && Tags.Contains(collision.gameObject.tag))
                _poolItem.DespawnSelf();
        }

        void OnCollisionExit(Collision collision)
        {
            if (CollisionExit && Tags.Contains(collision.gameObject.tag))
                _poolItem.DespawnSelf();
        }

        #endregion Trigger / Collision Monobehaviour Methods
    }
}