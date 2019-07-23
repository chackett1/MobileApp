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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace ProPooling
{
    /// <summary>
    /// Base class with common functionality for GlobalPools and Pools components.
    /// </summary>
    public abstract class PoolsBase : MonoBehaviour
    {
        #region Constants
        public const string MessageAssertIsActive = "Pro Pooling: Global Pools is not active. Please check it is added to you scene and if needed increate it's script execution order priority.";
        #endregion Constants

        #region Inspector Variables

        /// <summary>
        /// Whether to automatically add pools for requested items that do not exist.
        /// </summary>
        public bool AutomaticallyAddRuntimePools
        {
            get { return _automaticallyAddRuntimePools; }
            set { _automaticallyAddRuntimePools = value; }
        }
        [SerializeField]
        [Tooltip("Whether to automatically add pools at runtime for requested items that do not exist.")]
        bool _automaticallyAddRuntimePools;

        /// <summary>
        /// Default settings for new Pools and Pools that are automatically created.
        /// </summary>
        public Pool DefaultPoolSettings
        {
            get { return _defaultPoolSettings; }
            set { _defaultPoolSettings = value; }
        }
        [SerializeField]
        [Tooltip("Default settings for new Pools and Pools that are automatically created.")]
        Pool _defaultPoolSettings;

        /// <summary>
        /// A list of pools that should be preallocated.
        /// </summary>
        public List<Pool> Pools
        {
            get { return _pools; }
            set { _pools = value; }
        }
        [SerializeField]
        [Tooltip("Pools that should be preallocated.")]
        [FormerlySerializedAs("Pools")]
        List<Pool> _pools = new List<Pool>();

        #endregion Inspector Variables

        /// <summary>
        /// Whether the poolmanager has been initialised.
        /// </summary>
        /// Note: There is also the IsActive property - that refers to the singleton. Whilst similar, this 
        /// property will always be set when the InitialisePools method has completed and so can be used from
        /// test cases etc. when we might not be creating a singleton in the same fashion as in a game.
        public bool IsInitialised { get; set; }

        // mapping between prefab id and pool
        readonly Dictionary<int, Pool> _poolsIdMapping = new Dictionary<int, Pool>();

        // mapping between prefab name and pool
        readonly Dictionary<string, Pool> _poolsNameMapping = new Dictionary<string, Pool>();

        #region initialisation

        /// <summary>
        /// Constructor needed for unit testing.
        /// </summary>
        public PoolsBase()
        {
            DefaultPoolSettings = new Pool();
        }

        /// <summary>
        /// Initialise all pools - called by singleton in Awake
        /// </summary>
        public void InitialisePools() {
            _poolsIdMapping.Clear();
            _poolsNameMapping.Clear();

            foreach (var pool in Pools)
            {
                if (pool.Prefab == null)
                {
                    Debug.LogWarning("Pro Pooling: Ensure all pools in GlobalPools have a valid prefab or gameobject");
                    continue;
                }

                pool.ItemParent = transform;
                pool.Initialise(this);
                AddPoolMappings(pool);
            }

            IsInitialised = true;
        }
        #endregion initialisation

        #region pool retrieval
        /// <summary>
        /// Get a pool for the given prefab, optionally creating it if needed based upon the global settings
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="autoCreate"></param>
        /// <returns></returns>
        public Pool GetPool(GameObject prefab)
        {
            return GetPool(prefab, AutomaticallyAddRuntimePools);
        }


        /// <summary>
        /// Get a pool for the given prefab, optionally creating it if needed
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="autoCreate"></param>
        /// <returns></returns>
        public Pool GetPool(GameObject prefab, bool autoCreate)
        {
            Assert.IsNotNull(prefab, "Pro Pooling: The prefab you passed must not be null!");

            Pool pool;
            if (_poolsIdMapping.TryGetValue(prefab.GetInstanceID(), out pool))
                return pool;

            return autoCreate ? CreatePool(prefab) : null;
        }


        /// <summary>
        /// Get a pool for the prefab with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Pool GetPool(string name)
        {
            Assert.IsNotNull(name, "Pro Pooling: The prefabName you passed must not be null!");

            Pool pool;
            return _poolsNameMapping.TryGetValue(name, out pool) ? pool : null;
        }

        #endregion pool retrieval

        #region pool management

        /// <summary>
        /// Whether the named pool already exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ContainsPool(string name)
        {
            return GetPool(name) != null;
        }


        /// <summary>
        /// Whether a pool for the specified GameObject already exists
        /// </summary>
        /// <param name="prefabe"></param>
        /// <returns></returns>
        public bool ContainsPool(GameObject prefab)
        {
            return GetPool(prefab, false) != null;
        }


        /// <summary>
        /// Create a pool for the given prefab with default parameters. If an existing pool is found for this prefab then that is returned instead.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="inactiveParent"></param>
        /// <returns></returns>
        public Pool CreatePool(GameObject prefab, Transform inactiveParent = null)
        {
            return CreatePool(prefab, DefaultPoolSettings.InitialSize, DefaultPoolSettings.SizeExceededMode,
                DefaultPoolSettings.HasMaximumSize, DefaultPoolSettings.MaximumSize, DefaultPoolSettings.MaximumSizeExceededMode, 
                inactiveParent);
        }

        /// <summary>
        /// Create a pool for the given prefab with the specified parameters. If an existing pool is found for this prefab then that is returned instead.
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="initialSize"></param>
        /// <param name="limitExceededMode"></param>
        /// <param name="hasMaximumSize"></param>
        /// <param name="maximumSize"></param>
        /// <param name="createIfMaximumExceeded"></param>
        /// <param name="inactiveParent"></param>
        /// <returns></returns>
        public Pool CreatePool(GameObject prefab, int initialSize, Pool.SizeExceededModeType limitExceededMode, bool hasMaximumSize, int maximumSize, Pool.MaximumSizeExceededModeType maximumSizeExceededMode, Transform inactiveParent = null)
        {
            if (prefab == null)
            {
                Debug.LogWarning("Pro Pooling: Ensure all pools in GlobalPools have a valid prefab or gameobject");
                return null;
            }

            // Return any existing pool
            Pool pool;
            if (_poolsIdMapping.TryGetValue(prefab.GetInstanceID(), out pool)) return pool;

            // No existing so create a new pool
            pool = new Pool(prefab, initialSize, limitExceededMode, hasMaximumSize, maximumSize, maximumSizeExceededMode, inactiveParent);
            pool.Initialise(this);
            AddPool(pool);
            return pool;
        }


        /// <summary>
        /// Add a pool to management. 
        /// </summary>
        /// Note: This only adds references and doesn't actually place the pool in the Pools list!
        /// <param name="pool"></param>
        public Pool AddPool(Pool pool)
        {
            Assert.IsNotNull(pool.Prefab, "Pro Pooling: Ensure all pools added to GlobalPools have a valid prefab or gameobject");

            var existingPool = GetPool(pool.Prefab, false);
            if (existingPool == null)
            {
                Pools.Add(pool);
                AddPoolMappings(pool);
                return pool;
            }
            else
            {
                Debug.LogWarningFormat("Pro Pooling: A pool for '{0}' already exists in the pool manager.", pool.Name);
                return null;
            }
        }


        /// <summary>
        /// Add pool mappings assuming the pool is already in the Pools collection
        /// </summary>
        /// <param name="pool"></param>
        void AddPoolMappings(Pool pool)
        {
            Assert.IsTrue(Pools.Contains(pool), "Pro Pooling: Only call AddPoolMappings to initialise a pool that already exits in the Pools Collection.");

            if (_poolsNameMapping.ContainsKey(pool.Name))
                Debug.LogWarningFormat("Pro Pooling: Adding a pool with a duplicate name ({0}). This might cause problems under certain conditions so should be avoided by renaming the Prefab of Gameobject you are using.", pool.Name);

            _poolsIdMapping.Add(pool.ID, pool);
            _poolsNameMapping.Add(pool.Name, pool);
            pool.AddedToGlobalPools = true;
        }


        /// <summary>
        /// Remove the specified pool from management. 
        /// </summary>
        /// Note that this will not delete or remove any pool items.
        /// <param name="pool"></param>
        public void RemovePool(Pool pool)
        {
            if (_poolsIdMapping.ContainsKey(pool.ID))
                _poolsIdMapping.Remove(pool.ID);
            if (_poolsNameMapping.ContainsKey(pool.Name))
                _poolsNameMapping.Remove(pool.Name);
            Pools.Remove(pool);
            pool.AddedToGlobalPools = false;
        }


        /// <summary>
        /// Clear the pool and remove it from management. Note that if there are still spawned items then these will not 
        /// be destroyed by this call.
        /// </summary>
        /// <param name="pool"></param>
        public void ClearAndRemovePool(Pool pool)
        {
            pool.ClearPool();

            RemovePool(pool);
        }

        #endregion pool management

        #region Spawn from managed pools by name

        /// <summary>
        /// Spawn a gameobject from the named pool.
        /// </summary>
        /// <returns></returns>
        public GameObject Spawn(string poolName, Transform parent = null)
        {
            var poolItem = SpawnPoolItem(poolName, parent);
            return poolItem == null ? null : poolItem.GameObject;
        }


        /// <summary>
        /// Spawn a gameobject from the named pool.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Spawn(string poolName, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var poolItem = SpawnPoolItem(poolName, position, rotation, parent);
            return poolItem == null ? null : poolItem.GameObject;
        }


        /// <summary>
        /// Spawn a PoolItem reference from the named pool
        /// </summary>
        /// <returns></returns>
        public PoolItem SpawnPoolItem(string poolName, Transform parent = null)
        {
            var pool = GetPool(poolName);
            if (pool != null)
            {
                return SpawnPoolItem(pool, pool.Prefab, parent);
            }
            else
            {
                Debug.LogErrorFormat("Pro Pooling: SpawnPoolItem - Pool named '{0}' not managed by GlobalPools! Can not create pool or Instantiate an item as the pool was requested by name.", poolName);
                return null;
            }
        }


        /// <summary>
        /// Spawn a PoolItem reference from the named pool
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public PoolItem SpawnPoolItem(string poolName, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var pool = GetPool(poolName);
            if (pool != null)
            {
                return SpawnPoolItem(pool, pool.Prefab, position, rotation, parent);
            }
            else
            {
                Debug.LogErrorFormat("Pro Pooling: SpawnPoolItem - Pool named '{0}' not managed by GlobalPools! Can not create pool or Instantiate an item as the pool was requested by name.", poolName);
                return null;
            }
        }

        #endregion Spawn from managed pools by name

        #region Spawn from managed pools by gameobject


        /// <summary>
        /// Spawn a gameobject from the pool of the specified type.
        /// </summary>
        /// <returns></returns>
        public GameObject Spawn(GameObject gameObject, Transform parent = null)
        {
            var poolItem = SpawnPoolItem(gameObject, parent);
            return poolItem == null ? null : poolItem.GameObject;
        }


        /// <summary>
        /// Spawn a gameobject from the pool of the spefified type.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Spawn(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var poolItem = SpawnPoolItem(gameObject, position, rotation, parent);
            return poolItem == null ? null : poolItem.GameObject;
        }


        /// <summary>
        /// Spawn a PoolItem reference from the pool of the spefified type.
        /// </summary>
        /// <returns></returns>
        public PoolItem SpawnPoolItem(GameObject gameObject, Transform parent = null)
        {
            var pool = GetPool(gameObject);
            return SpawnPoolItem(pool, gameObject, parent);
        }


        /// <summary>
        /// Spawn a PoolItem reference from the pool of the spefified type.
        /// </summary>
        /// <returns></returns>
        public PoolItem SpawnPoolItem(Pool pool, GameObject gameObject, Transform parent = null)
        {
            if (pool != null)
            {
                return SpawnPoolItem(pool, gameObject, pool.Prefab.transform.position, pool.Prefab.transform.rotation,
                    parent);
            }
            else
            {
                return SpawnPoolItem(gameObject, default(Vector3), default(Quaternion), parent);
            }
        }


        /// <summary>
        /// Spawn a PoolItem reference from the pool of the spefified type.
        /// </summary>
        /// <returns></returns>
        public PoolItem SpawnPoolItem(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            Assert.IsNotNull(gameObject, "Pro Pooling: The gameObject you passed must not be null when getting an item from a pool!");
            Assert.IsTrue(IsInitialised, string.Format(MessageAssertIsActive, gameObject.name));

            var pool = GetPool(gameObject);
            return SpawnPoolItem(pool, gameObject, position, rotation, parent);
        }


        /// <summary>
        /// Spawn a PoolItem reference from the pool of the specified type. 
        /// </summary>
        /// If pool is null then it will either be created automatically in the AutomaticallyAddRuntimePools 
        /// flag is set or if not set the item will simple be instantiated. 
        /// <returns></returns>
        public PoolItem SpawnPoolItem(Pool pool, GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            Assert.IsNotNull(gameObject, "Pro Pooling: The gameObject you passed must not be null when getting an item from a pool!");
            Assert.IsTrue(IsInitialised, string.Format(MessageAssertIsActive, gameObject.name));

            if (pool == null && AutomaticallyAddRuntimePools)
                pool = CreatePool(gameObject);

            if (pool != null)
            {
                return pool.SpawnPoolItem(position, rotation, parent);
            }
            else
            {
                Debug.LogWarningFormat("Pro Pooling: SpawnPoolItem - Object named '{0}' not in a pool managed by GlobalPools and AutomaticallyAddRuntimePools set to false! Instantiating a new copy.", gameObject.name);
                var newGameobject = Instantiate(gameObject, position, rotation) as GameObject;
                newGameobject.transform.parent = null;
                var poolInstance = new PoolItem
                {
                    GameObject = newGameobject,
                    OriginalPrefab = gameObject
                };
                poolInstance.OnSetup();
                return poolInstance;
            }
        }
        #endregion Spawn from managed pools by gameobject

        #region Despawn to managed pools

        /// <summary>
        /// Despawn an item back to a pool managed by GlobalPools.
        /// </summary>
        /// <param name="poolItem"></param>
        /// <returns></returns>
        public bool Despawn(PoolItem poolItem)
        {
            if (poolItem != null)
            {
                poolItem.DespawnSelf();
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Despawn an item back to a pool managed by GlobalPools.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool Despawn(GameObject gameObject)
        {
            if (gameObject != null)
            {
                var pool = GetPool(gameObject.name);
                if (pool != null)
                {
                    return pool.Despawn(gameObject);
                }
                else
                {
                    Debug.LogWarningFormat("Pro Pooling: Despawn - No pool named '{0}' found in GlobalPools! Destroying returned gameObject.", gameObject.name);
                    Destroy(gameObject);
                    return false;
                }
            }
            return false;
        }


        /// <summary>
        /// Despawn all items back to the queue from the named pool.
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns></returns>
        public void DespawnAll(string poolName)
        {
            var pool = GetPool(poolName);
            if (pool != null)
            {
                pool.DespawnAll();
            }
            else
            {
                Debug.LogWarningFormat("Pro Pooling: DespawnAll - No pool named '{0}' found! Doing nothing.", poolName);
            }
        }


        /// <summary>
        /// Despawn all items back to the queue from the pool given by GameObject.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public void DespawnAll(GameObject gameObject)
        {
            if (gameObject != null)
            {
                DespawnAll(gameObject.name);
            }
        }


        /// <summary>
        /// Despawn all items back to the queue.
        /// </summary>
        /// <returns></returns>
        public void DespawnAllPools()
        {
            foreach (var pool in _pools)
                pool.DespawnAll();
        }
        #endregion Despawn to managed pools
    }
}
