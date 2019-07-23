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
    [AddComponentMenu("Pro Pooling/Despawn After Collision2D", 3)]
    [HelpURL("http://www.flipwebapps.com/pro-pooling/")]
    public class DespawnAfterCollision2D : MonoBehaviour, IPoolComponent
    {
        /// <summary>
        /// Respond to CollisionEnter2D events.
        /// </summary>
        public bool CollisionEnter2D { get { return _collisionEnter2D; } set { _collisionEnter2D = value; } }
        [Tooltip("Respond to CollisionEnter events.")]
        [SerializeField]
        bool _collisionEnter2D = true;

        /// <summary>
        /// Respond to CollisionStay2D events.
        /// </summary>
        public bool CollisionStay2D { get { return _collisionStay2D; } set { _collisionStay2D = value; } }
        [Tooltip("Respond to CollisionStay2D events.")]
        [SerializeField]
        bool _collisionStay2D;

        /// <summary>
        /// Respond to CollisionExit2D events.
        /// </summary>
        public bool CollisionExit2D { get { return _collisionExit2D; } set { _collisionExit2D = value; } }
        [Tooltip("Respond to CollisionExit2D events.")]
        [SerializeField]
        bool _collisionExit2D;

        /// <summary>
        /// Respond to TriggerEnter2D events.
        /// </summary>
        public bool TriggerEnter2D { get { return _triggerEnter2D; } set { _triggerEnter2D = value; } }
        [Tooltip("Respond to TriggerEnter2D events.")]
        [SerializeField]
        bool _triggerEnter2D = true;

        /// <summary>
        /// Respond to TriggerStay2D events.
        /// </summary>
        public bool TriggerStay2D { get { return _triggerStay2D; } set { _triggerStay2D = value; } }
        [Tooltip("Respond to TriggerStay2D events.")]
        [SerializeField]
        bool _triggerStay2D;

        /// <summary>
        /// Respond to TriggerExit2D events.
        /// </summary>
        public bool TriggerExit2D { get { return _triggerExit2D; } set { _triggerExit2D = value; } }
        [Tooltip("Respond to TriggerExit2D events.")]
        [SerializeField]
        bool _triggerExit2D;

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

        void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (TriggerEnter2D && Tags.Contains(otherCollider.tag))
                _poolItem.DespawnSelf();
        }

        void OnTriggerStay2D(Collider2D otherCollider)
        {
            if (TriggerStay2D && Tags.Contains(otherCollider.tag))
                _poolItem.DespawnSelf();
        }

        void OnTriggerExit2D(Collider2D otherCollider)
        {
            if (TriggerExit2D && Tags.Contains(otherCollider.tag))
                _poolItem.DespawnSelf();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (CollisionEnter2D && Tags.Contains(collision.gameObject.tag))
                _poolItem.DespawnSelf();
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            if (CollisionStay2D && Tags.Contains(collision.gameObject.tag))
                _poolItem.DespawnSelf();
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            if (CollisionExit2D && Tags.Contains(collision.gameObject.tag))
                _poolItem.DespawnSelf();
        }

        #endregion Trigger / Collision Monobehaviour Methods
    }
}