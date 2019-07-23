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

using ProPooling.Components;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProPooling
{
    /// <summary>
    /// Class for spawning a burst of items from a pool.
    /// </summary>
    [System.Serializable]
    public class Spawn
    {
        #region Enums

        // enum to determine when to spawn items from.
        public enum SpawnFromEnum
        {
            /// <summary>
            /// Spawn from the GlobalPools
            /// </summary>
            GlobalPools,
            /// <summary>
            /// Spawn from a Pools component
            /// </summary>
            PoolsComponent,
            /// <summary>
            /// Instantiate items on the fly (avoid using this).
            /// </summary>
            Instantiate,
        }


        /// <summary>
        /// How to scale spawned prefabs
        /// </summary>
        public enum ScaleModeEnum
        {
            /// <summary>
            /// Use the scale defined on the prefab
            /// </summary>
            FromPrefab,
            /// <summary>
            /// Use a constant scale
            /// </summary>
            Constant,
            /// <summary>
            /// Use a constant multiplier on the prefabs scale
            /// </summary>
            ConstantMultiplier,
            /// <summary>
            /// Use a random scale between two constants
            /// </summary>
            RandomBetweenTwoConstants,
            /// <summary>
            /// Use a random multiplier between two constants on the prefabs scale
            /// </summary>
            RandomBetweenTwoConstantsMultiplier
        }

        /// <summary>
        /// How to rotate spawned prefabs
        /// </summary>
        public enum RotationModeEnum
        {
            /// <summary>
            /// Use the rotation defined on the prefab
            /// </summary>
            FromPrefab,
            /// <summary>
            /// Use a constant rotation
            /// </summary>
            Constant,
            /// <summary>
            /// Use a random rotation between two constants
            /// </summary>
            RandomBetweenTwoConstants,
        }


        /// <summary>
        /// How to select prefabs and locations
        /// </summary>
        public enum SelectionMode
        {
            /// <summary>
            /// Select tone after the other in order
            /// </summary>
            InOrder,
            /// <summary>
            /// Select at random
            /// </summary>
            Random,
            /// <summary>
            /// Select at random using specified weights.
            /// </summary>
            RandomWeighted,
        }


        /// <summary>
        /// Where to spawn
        /// </summary>
        public enum LocationTypeEnum
        {
            /// <summary>
            /// Spawn from the spawners location
            /// </summary>
            SpawnersLocation,
            /// <summary>
            /// Spawn from specified positions
            /// </summary>
            Positions,
            /// <summary>
            /// Spawn from the locations held be specified transforms.
            /// </summary>
            Transforms,
        }

        #endregion Enums

        #region Inspector Variables

        /// <summary>
        /// When to spawn items from
        /// </summary>
        public SpawnFromEnum SpawnFrom { get { return _spawnFrom; } set { _spawnFrom = value; } }
        [Tooltip("When to spawn items from")]
        [SerializeField]
        SpawnFromEnum _spawnFrom = SpawnFromEnum.GlobalPools;


        /// <summary>
        /// If Spawn From is set to PoolsComponent then a reference to teh component to use.
        /// </summary>
        public Pools SpawnPools { get { return _spawnPools; } set { _spawnPools = value; } }
        [Tooltip("If Spawn From is set to PoolsComponent then a reference to teh component to use.")]
        [SerializeField]
        Pools _spawnPools;


        /// <summary>
        /// The scale mode to use for spawned items
        /// </summary>
        public ScaleModeEnum ScaleMode { get { return _scaleMode; } set { _scaleMode = value; } }
        [Tooltip("The scale mode to use for spawned items")]
        [SerializeField]
        ScaleModeEnum _scaleMode = ScaleModeEnum.FromPrefab;


        /// <summary>
        /// The scale to use for spawned items if using a constant scale or multiplier
        /// </summary>
        public Vector3 Scale { get { return _scale; } set { _scale = value; } }
        [Tooltip("The constant scale or multiplier to use for spawned items")]
        [SerializeField]
        Vector3 _scale = Vector3.one;


        /// <summary>
        /// Use a constant scale across all axis when performing a random scale
        /// </summary>
        public bool UseConstantAxisScale { get { return _useConstantAxisScale; } set { _useConstantAxisScale = value; } }
        [Tooltip("Use a constant scale across all axis when performing a random scales")]
        [SerializeField]
        bool _useConstantAxisScale = false;


        /// <summary>
        /// The minimum scale to use for all axis for spawned items if using a random scale or multiplier between two constants and PreserveAspectRatio is true
        /// </summary>
        public float MinimumScaleAllAxis { get { return _minimumScaleAllAxis; } set { _minimumScaleAllAxis = value; } }
        [Tooltip("The minimum scale to use for spawned items")]
        [SerializeField]
        float _minimumScaleAllAxis;


        /// <summary>
        /// The maximum scale to use for all axis for spawned items if using a random scale or multiplier between two constants and PreserveAspectRatio is true
        /// </summary>
        public float MaximumScaleAllAxis { get { return _maximumScaleAllAxis; } set { _maximumScaleAllAxis = value; } }
        [Tooltip("The maximumscale to use for spawned items")]
        [SerializeField]
        float _maximumScaleAllAxis = 1f;


        /// <summary>
        /// The minimum scale to use for spawned items if using a random scale or multiplier between two constants
        /// </summary>
        public Vector3 MinimumScale { get { return _minimumScale; } set { _minimumScale = value; } }
        [Tooltip("The minimum scale to use for spawned items")]
        [SerializeField]
        Vector3 _minimumScale = Vector3.one;


        /// <summary>
        /// The maximum scale to use for spawned items if using a random scale or multiplier between two constants
        /// </summary>
        public Vector3 MaximumScale { get { return _maximumScale; } set { _maximumScale = value; } }
        [Tooltip("The maximumscale to use for spawned items")]
        [SerializeField]
        Vector3 _maximumScale = Vector3.one;


        /// <summary>
        /// The rotation mode to use for spawned items
        /// </summary>
        public RotationModeEnum RotationMode { get { return _rotationMode; } set { _rotationMode = value; } }
        [Tooltip("The rotation mode to use for spawned items")]
        [SerializeField]
        RotationModeEnum _rotationMode = RotationModeEnum.FromPrefab;


        /// <summary>
        /// The rotation to use for spawned items if using a constant rotation or multiplier
        /// </summary>
        public Vector3 Rotation { get { return _rotation; } set { _rotation = value; } }
        [Tooltip("The constant rotation or multiplier to use for spawned items")]
        [SerializeField]
        Vector3 _rotation = Vector3.one;


        /// <summary>
        /// The minimum rotation to use for spawned items if using a random rotation or multiplier between two constants
        /// </summary>
        public Vector3 MinimumRotation { get { return _minimumRotation; } set { _minimumRotation = value; } }
        [Tooltip("The minimum rotation to use for spawned items")]
        [SerializeField]
        Vector3 _minimumRotation = Vector3.zero;


        /// <summary>
        /// The maximum rotation to use for spawned items if using a random rotation or multiplier between two constants
        /// </summary>
        public Vector3 MaximumRotation { get { return _maximumRotation; } set { _maximumRotation = value; } }
        [Tooltip("The maximumrotation to use for spawned items")]
        [SerializeField]
        Vector3 _maximumRotation = Vector3.zero;


        /// <summary>
        /// The size of the spawn area
        /// </summary>
        /// This represents a box from within which the spawn position will be randomly chosen. Use 0,0,0 to always spawn at the same location as the spawner.
        public Vector3 SpawnAreaSize { get { return _spawnAreaSize; } set { _spawnAreaSize = value; } }
        [Tooltip("The size of the spawn area. This represents a box from within which the spawn position will be randomly chosen. Use 0,0,0 to always spawn at the same location as the spawner.")]
        [SerializeField]
        Vector3 _spawnAreaSize = Vector3.zero;


        /// <summary>
        /// How to select the prefab to spawn
        /// </summary>
        public SelectionMode SelectPrefab { get { return _selectPrefab; } set { _selectPrefab = value; } }
        [Tooltip("How to select the prefab to spawn")]
        [SerializeField]
        SelectionMode _selectPrefab = SelectionMode.InOrder;


        /// <summary>
        /// The prefab sto Spawn
        /// </summary>
        public List<SpawnablePrefab> SpawnablePrefabs { get { return _spawnablePrefabs; } set { _spawnablePrefabs = value; } }
        [Tooltip("The prefabs to Spawn.")]
        [SerializeField]
        List<SpawnablePrefab> _spawnablePrefabs;


        /// <summary>
        /// What type of location to use for spawning
        /// </summary>
        public LocationTypeEnum LocationType { get { return _locationType; } set { _locationType = value; } }
        [Tooltip("How to select the prefab to spawn")]
        [SerializeField]
        LocationTypeEnum _locationType = LocationTypeEnum.SpawnersLocation;


        /// <summary>
        /// How to select the locations to spawn
        /// </summary>
        public SelectionMode SelectLocation { get { return _selectLocation; } set { _selectLocation = value; } }
        [Tooltip("How to select the prefab to spawn")]
        [SerializeField]
        SelectionMode _selectLocation = SelectionMode.InOrder;


        /// <summary>
        /// Whether Spaln location positions are global or local
        /// </summary>
        public bool SpawnLocationPositionsAreLocal { get { return _spawnLocationPositionsAreLocal; } set { _spawnLocationPositionsAreLocal = value; } }
        [Tooltip("The locations where to spawn")]
        [SerializeField]
        bool _spawnLocationPositionsAreLocal;


        /// <summary>
        /// The locations where to spawn
        /// </summary>
        public List<SpawnLocationPosition> SpawnLocationPositions { get { return _spawnLocationPositions; } set { _spawnLocationPositions = value; } }
        [Tooltip("The locations where to spawn")]
        [SerializeField]
        List<SpawnLocationPosition> _spawnLocationPositions;


        /// <summary>
        /// The locations where to spawn
        /// </summary>
        public List<SpawnLocationTransform> SpawnLocationTransforms { get { return _spawnLocationTransforms; } set { _spawnLocationTransforms = value; } }
        [Tooltip("The locations where to spawn")]
        [SerializeField]
        List<SpawnLocationTransform> _spawnLocationTransforms;

        #endregion Inspector Variables

        // Whether the configuration has been validated
        public bool IsConfigurationValidated { get; private set; }

        // The Owner
        public MonoBehaviour Owner { get; set; }

        // A default location that will override using the Owner.transform position
        public Transform SpawnersPosition { get; set; }

        Vector3 _halfSpawnAreaSize;

        int _currentPrefabIndex;
        int _totalPrefabWeight;

        int _currentLocationIndex;
        int _totalLocationWeight;

        #region Public API - Lifecycle

        /// <summary>
        /// Perform any initialisation.
        /// </summary>
        /// <param name="owner"></param>
        public void Initialise(MonoBehaviour owner)
        {
            Owner = owner;
            UpdateCachedValues();
            ValidateConfiguration(owner.gameObject.name);
        }

        /// <summary>
        /// Reset the status
        /// </summary>
        public void Reset()
        {
            _currentPrefabIndex = 0;
            _currentLocationIndex = 0;
        }

        #endregion Public API - Lifecycle

        #region Public API - Spawning

        /// <summary>
        /// Spawn a burst of the given size
        /// </summary>
        public void SpawnBurst(int size = 1)
        {
            for (int i = 0; i < size; i++)
            {
                SpawnOne();
            }
        }


        /// <summary>
        /// Spawn a burst of items between the specified minimum and maximum values
        /// </summary>
        public void SpawnBurst(int minimum, int maximum)
        {
            var burstSize = Random.Range(minimum, maximum);
            SpawnBurst(burstSize);
        }


        /// <summary>
        /// Spawn a single new item.
        /// </summary>
        public void SpawnOne()
        {
            if (Owner == null || !IsConfigurationValidated) return;

            // Determine prefab. Check if found (but should always be so)
            GameObject prefab = GetNextPrefab();
            if (prefab != null)
            {
                // determine position
                Vector3 position = Vector3.zero;
                switch (LocationType)
                {
                    case LocationTypeEnum.SpawnersLocation:
                        position = SpawnersPosition == null ? Owner.transform.position : SpawnersPosition.position;
                        if (SpawnAreaSize != Vector3.zero)
                            position += RandomInBoxFromHalfSize(_halfSpawnAreaSize);
                        break;
                    case LocationTypeEnum.Transforms:
                        var spawnLocationTransform = GetNextSpawnLocationTransform();
                        if (spawnLocationTransform != null)
                        {
                            position = spawnLocationTransform.Transform.position;
                            if (spawnLocationTransform.SpawnAreaSize != Vector3.zero)
                                position += RandomInBoxFromHalfSize(spawnLocationTransform.HalfSpawnAreaSize);
                        }
                        break;
                    case LocationTypeEnum.Positions:
                        var spawnLocationPosition = GetNextSpawnLocationPosition();
                        if (spawnLocationPosition != null)
                        {
                            position = spawnLocationPosition.Position;
                            if (SpawnLocationPositionsAreLocal)
                                position += SpawnersPosition == null ? Owner.transform.position : SpawnersPosition.position;
                            if (spawnLocationPosition.SpawnAreaSize != Vector3.zero)
                                position += RandomInBoxFromHalfSize(spawnLocationPosition.HalfSpawnAreaSize);
                        }
                        break;
                }

                // determine rotation
                Quaternion rotation = Quaternion.identity;
                switch (RotationMode)
                {
                    case RotationModeEnum.FromPrefab:
                        rotation = prefab.transform.rotation;
                        break;
                    case RotationModeEnum.Constant:
                        rotation = Quaternion.Euler(Rotation);
                        break;
                    case RotationModeEnum.RandomBetweenTwoConstants:
                        Quaternion.Euler(RandomBetween(MinimumRotation, MaximumRotation));
                        break;
                }

                // spawn
                GameObject spawnedGameObject = null;
                switch (SpawnFrom)
                {
                    case SpawnFromEnum.Instantiate:
                        spawnedGameObject = (GameObject)GameObject.Instantiate(prefab, position, rotation);
                        spawnedGameObject.transform.SetParent(Owner.gameObject.transform, false);
                        break;
                    case SpawnFromEnum.GlobalPools:
                        spawnedGameObject = GlobalPools.Instance.Spawn(prefab, position, rotation); //, Owner.gameObject.transform);
                        break;
                    case SpawnFromEnum.PoolsComponent:
                        spawnedGameObject = SpawnPools.Spawn(prefab, position, rotation); //, Owner.gameObject.transform);
                        break;
                }

                if (spawnedGameObject != null)
                {
                    // update scale if needed
                    switch (ScaleMode)
                    {
                        case ScaleModeEnum.Constant:
                            spawnedGameObject.transform.localScale = Scale;
                            break;
                        case ScaleModeEnum.ConstantMultiplier:
                            spawnedGameObject.transform.localScale =
                                Vector3.Scale(spawnedGameObject.transform.localScale, Scale);
                            break;
                        case ScaleModeEnum.RandomBetweenTwoConstants:
                            if (UseConstantAxisScale)
                                spawnedGameObject.transform.localScale = RandomVectorBetween(MinimumScaleAllAxis, MaximumScaleAllAxis);
                            else
                                spawnedGameObject.transform.localScale = RandomBetween(MinimumScale, MaximumScale);
                            break;
                        case ScaleModeEnum.RandomBetweenTwoConstantsMultiplier:
                            if (UseConstantAxisScale)
                                spawnedGameObject.transform.localScale =
                                    Vector3.Scale(spawnedGameObject.transform.localScale, RandomVectorBetween(MinimumScaleAllAxis, MaximumScaleAllAxis));
                            else
                                spawnedGameObject.transform.localScale =
                                    Vector3.Scale(spawnedGameObject.transform.localScale, RandomBetween(MinimumScale, MaximumScale));
                            break;
                    }
                }
            }
        }
        #endregion Public API - Spawning

        #region Public API - Helper Methods

        /// <summary>
        /// Update any cached values to reflect changes.
        /// </summary>
        /// Pro Pooling caches several values for performance reasons. Be sure to call this if you 
        /// make any changes to the properties.
        /// Note that this method does not do full validation or warn on issues with the state.
        public void UpdateCachedValues()
        {
            _currentPrefabIndex = Mathf.Clamp(_currentPrefabIndex, 0, SpawnablePrefabs.Count - 1);

            if (LocationType == LocationTypeEnum.Positions)
                _currentLocationIndex = Mathf.Clamp(_currentLocationIndex, 0, SpawnLocationPositions.Count - 1);
            else if (LocationType == LocationTypeEnum.Transforms)
                _currentLocationIndex = Mathf.Clamp(_currentLocationIndex, 0, SpawnLocationTransforms.Count - 1);

            _halfSpawnAreaSize = SpawnAreaSize / 2;

            _totalPrefabWeight = 0;
            foreach (var prefab in SpawnablePrefabs)
            {
                _totalPrefabWeight += prefab.Weight;
            }

            _totalLocationWeight = 0;
            if (LocationType == LocationTypeEnum.Transforms)
            {
                foreach (var spawnLocation in SpawnLocationTransforms)
                {
                    _totalLocationWeight += spawnLocation.Weight;
                    spawnLocation.HalfSpawnAreaSize = spawnLocation.SpawnAreaSize / 2;
                }
            }
            else if (LocationType == LocationTypeEnum.Positions)
            {
                foreach (var spawnLocation in SpawnLocationPositions)
                {
                    _totalLocationWeight += spawnLocation.Weight;
                    spawnLocation.HalfSpawnAreaSize = spawnLocation.SpawnAreaSize / 2;
                }
            }
        }

        #endregion Public API - Helper Methods

        #region Other Helper Methods

        /// <summary>
        /// Check if the current configuration is valid for spawning and warn if not.
        /// </summary>
        public bool ValidateConfiguration(string gameObjectName)
        {
            IsConfigurationValidated = false;

            // Valid pool set.
            if (SpawnFrom == SpawnFromEnum.GlobalPools && !GlobalPools.IsActive)
            {
                Debug.LogWarningFormat("Pro Pooling: GlobalPools is not active when referenced from AddPools on GameObject '{0}' so pools could not be added. Make sure a GlobalPools is added to your scene, or if already added increase it's script execution order priority.", gameObjectName);
                return false;
            }
            else if (SpawnFrom == SpawnFromEnum.PoolsComponent && SpawnPools == null)
            {
                Debug.LogWarningFormat("Pro Pooling: No Pools component is set in the spawner on GameObject '{0}'. Nothing will be spawned.", gameObjectName);
                return false;
            }
            // validate at least one prefab
            if (SpawnablePrefabs.Count == 0)
            {
                Debug.LogWarningFormat("Pro Pooling: No Prefabs are added to the spawner on GameObject '{0}'. Nothing will be spawned.", gameObjectName);
                return false;
            }
            var hasPrefab = false;
            foreach (var spawnablePrefab in SpawnablePrefabs)
            {
                if (spawnablePrefab.Prefab != null)
                {
                    switch (SpawnFrom)
                    {
                        case SpawnFromEnum.Instantiate:
                            hasPrefab = true;
                            break;
                        case SpawnFromEnum.GlobalPools:
                            if (GlobalPools.Instance.ContainsPool(spawnablePrefab.Prefab))
                                hasPrefab = true;
                            else
                                Debug.LogWarningFormat("Pro Pooling: GlobalPools does not contain the prefab '{0} referenced from the Spawner on GameObject '{0}'", spawnablePrefab.Prefab.name, gameObjectName);
                            break;
                        case SpawnFromEnum.PoolsComponent:
                            if (SpawnPools.ContainsPool(spawnablePrefab.Prefab))
                                hasPrefab = true;
                            else
                                Debug.LogWarningFormat("Pro Pooling: The referenced PoolsComponent does not contain the prefab '{0} referenced from the Spawner on GameObject '{0}'", spawnablePrefab.Prefab.name, gameObjectName);
                            break;
                    }
                }
            }
            if (!hasPrefab)
            {
                Debug.LogWarningFormat("Pro Pooling: No prefab was found on the spawner on GameObject '{0}'. Nothing will be spawned.", gameObjectName);
                return false;
            }
            if (SelectPrefab == SelectionMode.RandomWeighted && _totalPrefabWeight == 0)
            {
                Debug.LogWarningFormat("Pro Pooling: When using Random Weighted spawning mode on GameObject '{0}' some of the prefabs have to have a none zero weight. Nothing will be spawned.", gameObjectName);
                return false;
            }

            // At least one location
            if (LocationType == LocationTypeEnum.Transforms)
            {
                var hasLocation = false;
                foreach (var spawnLocation in SpawnLocationTransforms)
                {
                    if (spawnLocation.Transform != null)
                        hasLocation = true;
                }
                if (!hasLocation)
                {
                    Debug.LogWarningFormat("Pro Pooling: No Location transform was found on the spawner on GameObject '{0}'. Nothing will be spawned.", gameObjectName);
                    return false;
                }
            }
            else if (LocationType == LocationTypeEnum.Positions && SpawnLocationPositions.Count == 0)
            {
                Debug.LogWarningFormat("Pro Pooling: No Location position was found on the spawner on GameObject '{0}'. Nothing will be spawned.", gameObjectName);
            }
            // weights set if random weighted mode.
            if ((LocationType == LocationTypeEnum.Transforms || LocationType == LocationTypeEnum.Positions) && 
                SelectLocation == SelectionMode.RandomWeighted && _totalLocationWeight == 0)
            {
                Debug.LogWarningFormat("Pro Pooling: When using Random Weighted location mode on GameObject '{0}' some of the locations have to have a none zero weight. Nothing will be spawned.", gameObjectName);
                return false;
            }

            IsConfigurationValidated = true;
            return IsConfigurationValidated;
        }


        /// <summary>
        /// Get the next prefab to spawn.
        /// </summary>
        /// <returns></returns>
        private GameObject GetNextPrefab()
        {
            GameObject prefab = null;
            switch (SelectPrefab)
            {
                case SelectionMode.InOrder:
                    prefab = SpawnablePrefabs[_currentPrefabIndex].Prefab;
                    _currentPrefabIndex++;
                    if (_currentPrefabIndex >= SpawnablePrefabs.Count) _currentPrefabIndex = 0;
                    break;
                case SelectionMode.Random:
                    prefab = SpawnablePrefabs[Random.Range(0, SpawnablePrefabs.Count)].Prefab;
                    break;
                case SelectionMode.RandomWeighted:
                    int chosenWeight = Random.Range(1, _totalPrefabWeight);
                    int accumulativeWeight = 0;
                    foreach (var spawnPrefab in SpawnablePrefabs)
                    {
                        accumulativeWeight += spawnPrefab.Weight;
                        if (accumulativeWeight >= chosenWeight)
                        {
                            prefab = spawnPrefab.Prefab;
                            break;
                        }
                    }
                    break;
            }

            return prefab;
        }


        /// <summary>
        /// Get the next spawnable location position
        /// </summary>
        /// <returns></returns>
        private SpawnLocationPosition GetNextSpawnLocationPosition()
        {
            SpawnLocationPosition spawnableLocation = null;
            switch (SelectLocation)
            {
                case SelectionMode.InOrder:
                    if (_currentLocationIndex >= SpawnLocationPositions.Count) _currentLocationIndex = 0;
                    spawnableLocation = SpawnLocationPositions[_currentLocationIndex];
                    _currentLocationIndex++;
                    break;
                case SelectionMode.Random:
                    spawnableLocation = SpawnLocationPositions[Random.Range(0, SpawnLocationPositions.Count)];
                    break;
                case SelectionMode.RandomWeighted:
                    int chosenWeight = Random.Range(1, _totalLocationWeight);
                    int accumulativeWeight = 0;
                    foreach (var spawnLocation in SpawnLocationPositions)
                    {
                        accumulativeWeight += spawnLocation.Weight;
                        if (accumulativeWeight >= chosenWeight)
                        {
                            spawnableLocation = spawnLocation;
                            break;
                        }
                    }
                    break;
            }

            return spawnableLocation;
        }


        /// <summary>
        /// Get the next spawnable location transform
        /// </summary>
        /// <returns></returns>
        private SpawnLocationTransform GetNextSpawnLocationTransform()
        {
            SpawnLocationTransform spawnableLocation = null;
            switch (SelectLocation)
            {
                case SelectionMode.InOrder:
                    if (_currentLocationIndex >= SpawnLocationTransforms.Count) _currentLocationIndex = 0;
                    spawnableLocation = SpawnLocationTransforms[_currentLocationIndex];
                    _currentLocationIndex++;
                    break;
                case SelectionMode.Random:
                    spawnableLocation = SpawnLocationTransforms[Random.Range(0, SpawnLocationTransforms.Count)];
                    break;
                case SelectionMode.RandomWeighted:
                    int chosenWeight = Random.Range(1, _totalLocationWeight);
                    int accumulativeWeight = 0;
                    foreach (var spawnLocation in SpawnLocationTransforms)
                    {
                        accumulativeWeight += spawnLocation.Weight;
                        if (accumulativeWeight >= chosenWeight)
                        {
                            spawnableLocation = spawnLocation;
                            break;
                        }
                    }
                    break;
            }

            return spawnableLocation;
        }


        /// <summary>
        /// Return a random value within a box given by it's half size
        /// </summary>
        /// <param name="halfSize"></param>
        /// <returns></returns>
        private static Vector3 RandomInBoxFromHalfSize(Vector3 halfSize)
        {
            return new Vector3(Random.Range(-halfSize.x, halfSize.x),
                                                Random.Range(-halfSize.y, halfSize.y),
                                                Random.Range(-halfSize.z, halfSize.z));
        }


        /// <summary>
        /// Return a random vector between specified values
        /// </summary>
        /// <param name="halfSpawnAreaSize"></param>
        /// <returns></returns>
        private static Vector3 RandomBetween(Vector3 minimum, Vector3 maximum)
        {
            return new Vector3(Random.Range(minimum.x, maximum.x),
                                Random.Range(minimum.y, maximum.y),
                                Random.Range(minimum.z, maximum.z));
        }


        /// <summary>
        /// Return a random vector with same value for all axis between specified values.
        /// </summary>
        /// <param name="halfSpawnAreaSize"></param>
        /// <returns></returns>
        private static Vector3 RandomVectorBetween(float minimum, float maximum)
        {
            var value = Random.Range(minimum, maximum);
            return new Vector3(value, value, value);
        }
        #endregion Other Helper Methods

        #region Debugging

        /// <summary>
        /// Draw system gizmos to show spawn location.
        /// </summary>
        public void DrawGizmos(Vector3 position)
        {
            if (LocationType == LocationTypeEnum.SpawnersLocation)
            {
                if (SpawnAreaSize != Vector3.zero)
                {
                    Gizmos.color = new Color(.5f, .5f, 1f, 0.2F);
                    Gizmos.DrawCube(position, SpawnAreaSize);
                }
            }
            else if (LocationType == LocationTypeEnum.Transforms)
            {
                foreach (var location in SpawnLocationTransforms)
                {
                    if (location.Transform != null)
                    {
                        var locationPosition = location.Transform.position;
                        Gizmos.color = new Color(.5f, 1f, .5f, 0.6F);
                        Gizmos.DrawSphere(locationPosition, 0.05f);

                        if (location.SpawnAreaSize != Vector3.zero)
                        {
                            Gizmos.color = new Color(.5f, .5f, 1f, 0.2F);
                            Gizmos.DrawCube(locationPosition, location.SpawnAreaSize);
                        }
                    }
                }
            }
            else if (LocationType == LocationTypeEnum.Positions)
            {
                foreach (var location in SpawnLocationPositions)
                {
                    var locationPosition = location.Position;
                    if (!SpawnLocationPositionsAreLocal) locationPosition+= position;
                    Gizmos.color = new Color(.5f, 1f, .5f, 0.6F);
                    Gizmos.DrawSphere(locationPosition, 0.05f);

                    if (location.SpawnAreaSize != Vector3.zero)
                    {
                        Gizmos.color = new Color(.5f, .5f, 1f, 0.2F);
                        Gizmos.DrawCube(locationPosition, location.SpawnAreaSize);
                    }
                }
            }
        }

        #endregion Debugging

        #region Utility Classes

        /// <summary>
        /// Class for the prefabs that can be spawned.
        /// </summary>
        [System.Serializable]
        public class SpawnablePrefab
        {
            /// <summary>
            /// The prefab to spawn
            /// </summary>
            public GameObject Prefab { get { return _prefab; } set { _prefab = value; } }
            [Tooltip("The prefab to Spawn.")]
            [SerializeField]
            GameObject _prefab;

            /// <summary>
            /// A relative weight to use when determining the prefab to spawn
            /// </summary>
            public int Weight { get { return _weight; } set { _weight = value; } }
            [Tooltip("A relative weight to use when determining the prefab to spawn")]
            [SerializeField]
            int _weight;
        }


        /// <summary>
        /// Class for the locations where spawning can occur.
        /// </summary>
        [System.Serializable]
        public abstract class SpawnLocation
        {
            /// <summary>
            /// A relative weight to use when determining which location to use
            /// </summary>
            public int Weight { get { return _weight; } set { _weight = value; } }
            [Tooltip("A relative weight to use when determining which location to use")]
            [SerializeField]
            int _weight;


            /// <summary>
            /// The size of the spawn area
            /// </summary>
            /// This represents a box from within which the spawn position will be randomly chosen. Use 0,0,0 to always spawn at the same location as the spawner.
            public Vector3 SpawnAreaSize { get { return _spawnAreaSize; } set { _spawnAreaSize = value; } }
            [Tooltip("The size of the spawn area. This represents a box from within which the spawn position will be randomly chosen. Use 0,0,0 to always spawn at the same location as the spawner.")]
            [SerializeField]
            Vector3 _spawnAreaSize = Vector3.zero;

            internal Vector3 HalfSpawnAreaSize { get; set; }
        }


        /// <summary>
        /// Class for the locations where spawning can occur when using a Vector3 position.
        /// </summary>
        [System.Serializable]
        public class SpawnLocationPosition : SpawnLocation
        {
            /// <summary>
            /// Vector3 that contains position on where to Spawn
            /// </summary>
            public Vector3 Position { get { return _position; } set { _position = value; } }
            [Tooltip("Vector3 that contains position on where to Spawn")]
            [SerializeField]
            Vector3 _position;
        }


        /// <summary>
        /// Class for the locations where spawning can occur when using a Transform.
        /// </summary>
        [System.Serializable]
        public class SpawnLocationTransform : SpawnLocation
        {
            /// <summary>
            /// Transform that contains information on where to Spawn
            /// </summary>
            public Transform Transform { get { return _transform; } set { _transform = value; } }
            [Tooltip("The prefab to Spawn.")]
            [SerializeField]
            Transform _transform;
        }
        #endregion Utility Classes
    }
}