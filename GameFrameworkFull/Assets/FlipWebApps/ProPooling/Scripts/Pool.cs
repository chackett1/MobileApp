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
    /// Prefab pooling class that can preallocate a certain number of instances.
    /// </summary>
    [System.Serializable]
    public class Pool
    {
        #region Enums

        /// <summary>
        /// The action to take when a pool item is requested and the pool is empty (all items are in use)
        /// </summary>
        public enum SizeExceededModeType {
            /// <summary>
            /// Increase the pool size
            /// </summary>
            IncreasePoolSize,
            /// <summary>
            /// Create ad-hoc extra items on the fly that aren't returned to the pool
            /// </summary>
            CreateNonPoolItems,
            /// <summary>
            /// Return null
            /// </summary>
            ReturnNull,
            /// <summary>
            /// Recycle (Reuse) the oldest item)
            /// </summary>
            RecycleOldest
        }

        /// <summary>
        /// The action to take when a pool item is requested and the maximum number of items allowed are all already in use
        /// </summary>
        public enum MaximumSizeExceededModeType
        {
            /// <summary>
            /// Create ad-hoc extra items on the fly that aren't returned to the pool
            /// </summary>
            CreateNonPoolItems,
            /// <summary>
            /// Return null
            /// </summary>
            ReturnNull,
            /// <summary>
            /// Recycle (Reuse) the oldest item)
            /// </summary>
            RecycleOldest
        }
        #endregion Enums

        #region Inspector Variables

        /// <summary>
        /// The prefab / gameobject that we want to pool.
        /// </summary>
        public GameObject Prefab
        {
            get { return _prefab; }
            set { _prefab = value; }
        }
        [SerializeField]
        [Tooltip("The prefab / gameobject that we want to pool.")]
        [FormerlySerializedAs("Prefab")]
        GameObject _prefab;

        /// <summary>
        /// The initial number of pool items that should be created when this pool is loaded.
        /// </summary>
        public int InitialSize
        {
            get { return _initialSize; }
            set { _initialSize = value; }
        }
        [SerializeField]
        [Tooltip("The initial number of pool items that should be created when this pool is loaded.")]
        [FormerlySerializedAs("PreInitialiseCount")]
        int _initialSize = 1;


        /// <summary>
        /// Whether to progressively fill the pool to the initial size.
        /// </summary>
        public bool ProgressiveFill
        {
            get { return _progressiveFill; }
            set { _progressiveFill = value; }
        }
        [SerializeField]
        [Tooltip("Whether to progressively fill the pool to the initial size.")]
        bool _progressiveFill = false;


        /// <summary>
        /// When progressively filling the number to initailly allocate.
        /// </summary>
        public int ProgressiveFillInitialAmount
        {
            get { return _progressiveFillInitialAmount; }
            set { _progressiveFillInitialAmount = value; }
        }
        [SerializeField]
        [Tooltip("When progressively filling the number to initailly allocate.")]
        int _progressiveFillInitialAmount = 1;


        /// <summary>
        /// When progressively filling the frame interval between allocating new items.
        /// </summary>
        public int ProgressiveFillFrameInterval
        {
            get { return _progressiveFillFrameInterval; }
            set { _progressiveFillFrameInterval = value; }
        }
        [SerializeField]
        [Tooltip("When progressively filling the frame interval between allocating new items.")]
        int _progressiveFillFrameInterval = 1;


        /// <summary>
        /// When progressively filling the number to allocate each interval.
        /// </summary>
        public int ProgressiveFillIntervalAmount
        {
            get { return _progressiveFillIntervalAmount; }
            set { _progressiveFillIntervalAmount = value; }
        }
        [SerializeField]
        [Tooltip("When progressively filling the number to allocate each interval.")]
        int _progressiveFillIntervalAmount = 1;


        /// <summary>
        /// The action to take when a pool item is requested and the pool is empty (all items are in use)
        /// </summary>
        [SerializeField]
        public SizeExceededModeType SizeExceededMode
        {
            get { return _sizeExceededMode; }
            set { _sizeExceededMode = value; }
        }
        [SerializeField]
        [Tooltip("The action to take when a pool item is requested and the pool is empty (all items are in use)")]
        SizeExceededModeType _sizeExceededMode = SizeExceededModeType.IncreasePoolSize;

        /// <summary>
        /// If the size can grow, then whether the pool has a maximum size to which it can grow (default false).
        /// </summary>
        public bool HasMaximumSize
        {
            get { return _hasMaximumSize; }
            set { _hasMaximumSize = value; }
        }
        [SerializeField]
        [Tooltip("If the size can grow, then whether the pool has a maximum size to which it can grow.")]
        bool _hasMaximumSize = false;

        /// <summary>
        /// The maximum size of this pool. More items can be created on the fly if needed by enabling createIfMaximumExceeded but they won't be pooled.")]
        /// </summary>
        public int MaximumSize
        {
            get { return _maximumSize; }
            set { _maximumSize = value; }
        }
        [SerializeField]
        [Tooltip("The maximum size of this pool. More items can be created on the fly if needed by enabling createIfMaximumExceeded but they won't be pooled.")]
        [FormerlySerializedAs("MaxCapacity")]
        int _maximumSize;

        /// <summary>
        /// The action to take when a pool item is requested and the maximum number of items allowed are all already in use
        /// </summary>
        [SerializeField]
        public MaximumSizeExceededModeType MaximumSizeExceededMode
        {
            get { return _maximumSizeExceededMode; }
            set { _maximumSizeExceededMode = value; }
        }
        [SerializeField]
        [Tooltip("The action to take when a pool item is requested and the maximum number of items allowed are all already in use")]
        MaximumSizeExceededModeType _maximumSizeExceededMode = MaximumSizeExceededModeType.ReturnNull;

        #endregion Inspector Variables

        //[Tooltip("Whether to skip sending lifecycle notifications to other game objects. Use this if you don't need lifecycle notifications to get some speed benefits.")]
        //public bool SkipLifeCycleNotifications = true;

        //[Tooltip("Only pool the gameobject giving some speed benefits. Use this if you don't need lifecycle notifications and does not need to get or override the pool item.")]
        //public bool PoolGameobjectOnly = true;

        /// <summary>
        /// The parent transform under which pooled items will be held.
        /// </summary>
        public Transform ItemParent { get; set; }

        /// <summary>
        /// Returns the total number of instances in this pool (both inactive and in use).
        /// </summary>
        public int Count { get { return InactiveCount + SpawnedCount; } }

        /// <summary>
        /// Returns the number of inactive items in this pool
        /// </summary>
        public int InactiveCount { get { return _inactiveInstances.Count; } }

        /// <summary>
        /// Returns the number of spawned (in use) instances from this pool
        /// </summary>
        public int SpawnedCount { get { return _activeInstances.Count; } }

        /// <summary>
        /// Returns the number of spawned (in use) instances from this pool plus any non pool items that have been allocated
        /// </summary>
        public int SpawnedAndNonPoolCount { get; set; }

        /// <summary>
        /// Returns the maximum number of spawned (in use instances from this pool plus any non pool items that have been allocated
        /// </summary>
        public int SpawnedAndNonPoolCountMaximum { get; set; }

        /// <summary>
        /// Returns the instance ID of the prefab associated with this pool
        /// </summary>
        public int ID { get { return Prefab == null ? -1 : Prefab.GetInstanceID(); } }

        /// <summary>
        /// Returns the name of the prefab associated with this pool
        /// </summary>
        public string Name { get { return Prefab == null ? null : Prefab.name; } }

        /// <summary>
        /// Returns whether this pool has been added to the GlobalPools
        /// </summary>
        public bool AddedToGlobalPools { get; set; }

        /// <summary>
        /// Returns whether this pool has been initialised and pool items created
        /// </summary>
        public bool IsInitialised { get; set; }

        Queue<PoolItem> _inactiveInstances;
        [System.NonSerialized]
        List<PoolItem> _activeInstances;
        Dictionary<GameObject, PoolItem> _spawnedGameObjectToPoolItemLookup;

        /// <summary>
        /// Parameterless constructor to let us override and instantiate from within the pool editor
        /// </summary>
        public Pool() {
            AddedToGlobalPools = false;

            _inactiveInstances = new Queue<PoolItem>(MaximumSize);
            _activeInstances = new List<PoolItem>(MaximumSize);
            _spawnedGameObjectToPoolItemLookup = new Dictionary<GameObject, PoolItem>(MaximumSize);
        }


        /// <summary>
        /// Constructor for creating a pool instance
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="initialSize"></param>
        /// <param name="sizeExceededMode"></param>
        /// <param name="hasMaximumSize"></param>
        /// <param name="maximumSize"></param>
        /// <param name="itemParent"></param>
        public Pool(GameObject prefab, 
            int initialSize = 0, 
            SizeExceededModeType sizeExceededMode = SizeExceededModeType.IncreasePoolSize, 
            bool hasMaximumSize = false, 
            int maximumSize = 0,
            MaximumSizeExceededModeType maximumSizeExceededMode = MaximumSizeExceededModeType.ReturnNull,
            Transform itemParent = null) : this()
        {
            Assert.IsNotNull(prefab, "Pro Pooling: prefab must not be null!");

            Prefab = prefab;
            InitialSize = initialSize;
            SizeExceededMode = sizeExceededMode;
            HasMaximumSize = hasMaximumSize;
            MaximumSize = maximumSize;
            MaximumSizeExceededMode = maximumSizeExceededMode;

            ItemParent = itemParent;
        }

        #region Initialisation and Cleanup

        /// <summary>
        /// Create the specified number of instances in an inactive state and add to the inactive list.
        /// </summary>
        public void Initialise()
        {
            MaximumSize = Mathf.Max(InitialSize, MaximumSize);
            if (ProgressiveFill)
                Debug.LogWarning("If using Progressive Fill then please pass a parent to the Initialise method.");

            AddPoolItems(InitialSize);

            IsInitialised = true;
        }


        /// <summary>
        /// Create the specified number of instances in an inactive state and add to the inactive list.
        /// </summary>
        public void Initialise(MonoBehaviour Owner)
        {
            if (ProgressiveFill)
            {
                MaximumSize = Mathf.Max(InitialSize, MaximumSize);
                Owner.StartCoroutine(ProgressiveFillCoroutine());
                IsInitialised = true;
            }
            else
            {
                Initialise();
            }
        }


        /// <summary>
        /// Add the specified number of items to the pool
        /// </summary>
        public void AddPoolItems(int number)
        {
            for (var i = 0; i < number; i++)
            {
                var poolItem = CreatePoolInstance(ItemParent, Vector3.zero, Quaternion.identity);
                poolItem.IsFromPool = true;
                poolItem.GameObject.SetActive(false);

                _spawnedGameObjectToPoolItemLookup.Add(poolItem.GameObject, poolItem);
                _inactiveInstances.Enqueue(poolItem);
            }
        }


        /// <summary>
        /// Clear all pooled items.
        /// </summary>
        /// Any active instances will be first despawned before being destroyed.
        public void ClearPool()
        {
            while (_inactiveInstances.Count > 0)
            {
                var poolItem = _inactiveInstances.Dequeue();
                poolItem.OnDestroy();
#if UNITY_EDITOR
                // different for editor mode so we can run tests.
                if (!Application.isPlaying)
                    GameObject.DestroyImmediate(poolItem.GameObject);
                else
#endif
                    GameObject.Destroy(poolItem.GameObject);
            }

            while (_activeInstances.Count > 0)
            {
                var poolItem = _activeInstances[_activeInstances.Count-1];
                _activeInstances.RemoveAt(_activeInstances.Count - 1);
                poolItem.OnDespawn();
                poolItem.OnDestroy();
#if UNITY_EDITOR
                // different for editor mode so we can run tests.
                if (!Application.isPlaying)
                    GameObject.DestroyImmediate(poolItem.GameObject);
                else
#endif
                    GameObject.Destroy(poolItem.GameObject);
            }

            _spawnedGameObjectToPoolItemLookup.Clear();
        }


        /// <summary>
        /// Returns the PoolItem corresponding to the specified GameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>PoolItem or null if the GameObject is not in the Pool</returns>
        public PoolItem PoolItemForGameObject(GameObject gameObject)
        {
            if (gameObject == null) return null;

            PoolItem poolItem;
            _spawnedGameObjectToPoolItemLookup.TryGetValue(gameObject, out poolItem);
            return poolItem;
        }


        /// <summary>
        /// Return whether the specified item is spawned and contained in the pool.
        /// </summary>
        /// <param name="poolItem"></param>
        /// <returns></returns>
        public bool IsSpawnedInstance(PoolItem poolItem)
        {
            return _activeInstances.Contains(poolItem);
        }


        /// <summary>
        /// Return whether the specified item is spawned and contained in the pool.
        /// </summary>
        /// <param name="poolItem"></param>
        /// <returns></returns>
        public bool IsSpawnedInstance(GameObject gameObject)
        {
            var poolItem = PoolItemForGameObject(gameObject);
            return poolItem == null ? false : _activeInstances.Contains(poolItem);
        }


        /// <summary>
        /// Progressive filling of the pool to avoid startup delays
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator ProgressiveFillCoroutine()
        {
            AddPoolItems(ProgressiveFillInitialAmount);
            var framesInterval = ProgressiveFillFrameInterval;
            while (Count < InitialSize)
            {
                while (framesInterval > 0)
                {
                    framesInterval--;
                    yield return null;
                }
                AddPoolItems(Mathf.Min(ProgressiveFillIntervalAmount, InitialSize - Count));
                framesInterval = ProgressiveFillFrameInterval;
            }
        }

        #endregion Initialisation and Cleanup

        #region Spawn

        /// <summary>
        /// Spawn a gameobject from the pool.
        /// </summary>
        /// Position and rotation will be set to the default values that the initial prefab had.
        /// <returns></returns>
        public GameObject Spawn(Transform parent = null)
        {
            var poolItem = SpawnPoolItem(Prefab.transform.position, Prefab.transform.rotation, parent);
            return poolItem == null ? null : poolItem.GameObject;
        }


        /// <summary>
        /// Spawn a gameobject from the pool.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var poolItem = SpawnPoolItem(position, rotation, parent);
            return poolItem == null ? null : poolItem.GameObject;
        }



        /// <summary>
        /// Spawn a PoolItem from the pool.
        /// </summary>
        /// Position and rotation will be set to the default values that the initial prefab had.
        /// <returns></returns>
        public virtual PoolItem SpawnPoolItem(Transform parent = null)
        {
            return SpawnPoolItem(Prefab.transform.position, Prefab.transform.rotation, parent);
        }


        ///// <summary>
        ///// Get an item from the pool, optionally creating a new one if there aren't already enough avilable.
        ///// </summary>
        ///// <returns></returns>
        //public T GetPoolItemFromPool<T>(Vector3 position, Quaternion rotation, Transform parent = null) where T : PoolItem
        //{
        //    return GetPoolItemFromPool(position, rotation, parent = null) as T;
        //}


        /// <summary>
        /// Spawn a PoolItem from the pool.
        /// </summary>
        /// <returns></returns>
        public PoolItem SpawnPoolItem(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            PoolItem poolItem;
            // if inactive item available then use
            if (_inactiveInstances.Count > 0)
            {
                poolItem = _inactiveInstances.Dequeue();
                if (poolItem.GameObject == null) { 
                    Debug.LogErrorFormat("Pro Pooling: Found an Destroyed item in the {0} pool. Please ensure you use Despawn rather than Destroy for pooled items.");
                    return null;
                }
                poolItem.GameObject.transform.position = position;
                poolItem.GameObject.transform.rotation = rotation;
            }
            // check conditions for returning null
            else if (SizeExceededMode == SizeExceededModeType.ReturnNull ||
                    (SizeExceededMode == SizeExceededModeType.IncreasePoolSize &&
                     HasMaximumSize && _activeInstances.Count >= MaximumSize && MaximumSizeExceededMode == MaximumSizeExceededModeType.ReturnNull))
            {
                return null;
            }
            // check conditions for recycling
            else if (SizeExceededMode == SizeExceededModeType.RecycleOldest ||
                    (SizeExceededMode == SizeExceededModeType.IncreasePoolSize &&
                     HasMaximumSize && _activeInstances.Count >= MaximumSize && MaximumSizeExceededMode == MaximumSizeExceededModeType.RecycleOldest))
            {
                poolItem = _activeInstances[0];

                _activeInstances.RemoveAt(0);
                poolItem.OnDespawn();
                SpawnedAndNonPoolCount--;
            }
            // create new item for either adding to pool or as ad hoc
            else
            {
                // create new item for either adding to pool or as ad hoc
                poolItem = CreatePoolInstance(ItemParent, position, rotation);

                // check whether this should be a pooled item
                if (SizeExceededMode == SizeExceededModeType.IncreasePoolSize && (!HasMaximumSize || _activeInstances.Count < MaximumSize))
                {
                    poolItem.IsFromPool = true;
                    _spawnedGameObjectToPoolItemLookup.Add(poolItem.GameObject, poolItem);
                }
            }

            if (parent != null)
                poolItem.GameObject.transform.SetParent(parent, true);
            poolItem.GameObject.SetActive(Prefab.activeSelf);
            poolItem.OnSpawn();

            if (poolItem.IsFromPool)
                _activeInstances.Add(poolItem);

            SpawnedAndNonPoolCount++;
            SpawnedAndNonPoolCountMaximum = Mathf.Max(SpawnedAndNonPoolCountMaximum, SpawnedAndNonPoolCount);

            return poolItem;
        }

        #endregion Spawn

        #region Despawn

        /// <summary>
        /// Despawn an item back to the queue. If we are above the maximum capacity then we just delete the item.
        /// </summary>
        /// <param name="poolItem"></param>
        /// <returns></returns>
        public bool Despawn(PoolItem poolItem)
        {
            if (poolItem != null)
            {
                var gameObject = poolItem.GameObject;
                if (poolItem.IsFromPool)
                {
                    _activeInstances.Remove(poolItem);
                    _inactiveInstances.Enqueue(poolItem);

                    poolItem.OnDespawn();

                    if (gameObject.transform.parent != ItemParent)
                        gameObject.transform.SetParent(ItemParent, false);
                    gameObject.SetActive(false);
                }
                else
                {
                    GameObject.Destroy(gameObject);
                }

                SpawnedAndNonPoolCount--;
            }
            else
                Debug.LogWarning("Despawn: Can not return a null object!");

            return false;
        }


        /// <summary>
        /// Despawn an item back to the queue. If we are above the maximum capacity then we just delete the item.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public bool Despawn(GameObject gameObject)
        {
            if (gameObject != null)
            {
                var poolItem = PoolItemForGameObject(gameObject);
                if (poolItem != null)
                {
                    return Despawn(poolItem);
                }
                else
                {
                    GameObject.Destroy(gameObject);
                    // decrease counter as this covers non pool items also
                    SpawnedAndNonPoolCount--;   
                }
            }
            else
                Debug.LogWarning("Despawn: Can not return a null object!");

            return false;
        }


        /// <summary>
        /// Despawn all items back to the queue.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public void DespawnAll()
        {
            // note: could have used while (!list empty), but don't due to possible infinate loop issues if new items get added.
            var poolItems = new List<PoolItem>();
            poolItems.AddRange(_activeInstances);
            foreach (var poolItem in poolItems)
            {
                Despawn(poolItem);
            }
        }
        #endregion Despawn

        #region Create

        /// <summary>
        /// Create a new pooled instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        PoolItem CreatePoolInstance(Transform parent, Vector3 position, Quaternion rotation)
        {
            var prefabClone = (GameObject)GameObject.Instantiate(Prefab, position, rotation);
            if (parent != null)
                prefabClone.transform.SetParent(parent, true);
            prefabClone.name = Prefab.name;

            var poolInstance = CreatePoolItem();
            poolInstance.GameObject = prefabClone;
            poolInstance.OriginalPrefab = Prefab;
            poolInstance.Pool = this;
            poolInstance.OnSetup();
            return poolInstance;
        }


        /// <summary>
        /// Create a new PoolItem
        /// </summary>
        /// Override this in subclasses if you want to use subclasses of PoolItem. See also PoolGeneric for a generic implementation of this
        /// <returns></returns>
        protected virtual PoolItem CreatePoolItem()
        {
            return new PoolItem();
        }

#endregion Create
    }
}