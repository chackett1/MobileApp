//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
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

#if GAME_FRAMEWORK
using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using GameFramework.Helper;
using UnityEngine;

namespace ProPooling.GameActions
{
    /// <summary>
    /// Game Framework GameAction class to spawn a burst of the specified gameobjects
    /// </summary>
    [System.Serializable]
    [ClassDetails("Spawn Burst", "Pro Pooling/Spawn Burst", "Spawn a burst of item(s)")]
    public class GameActionSpawnBurst : GameAction
    {
        /// <summary>
        /// What type of location to use for spawning
        /// </summary>
        public enum LocationTypeEnum
        {
            ThisGameObject,
            CollidingGameObject,
            Positions,
            Transforms,
        }

        /// <summary>
        /// The coordinate space to use.
        /// </summary>
        public enum LocationCoordinateSpaceEnum
        {
            Global,
            LocalToThisGameObject,
            LocalToCollidingGameObject
        }

        #region Inspector Variables

        /// <summary>
        /// The Spawn instance with configuration and helper code
        /// </summary>
        [Tooltip("The Spawn instance with configuration and helper code")]
        [SerializeField]
        Spawn _spawn;


        /// <summary>
        /// The minimum number of instances to spawn
        /// </summary>
        [Tooltip("The minimum number of instances to spawn")]
        [SerializeField]
        int _burstMinimum = 1;


        /// <summary>
        /// The maximum number of instances to spawn
        /// </summary>
        [Tooltip("The maximum number of instances to spawn")]
        [SerializeField]
        int _burstMaximum = 1;


        /// <summary>
        /// What type of location to use for spawning
        /// </summary>
        [Tooltip("What type of location to use for spawning")]
        [SerializeField]
        LocationTypeEnum _locationType;

        /// <summary>
        /// The coordinate space to use.
        /// </summary>
        [Tooltip("The coordinate space to use")]
        [SerializeField]
        LocationCoordinateSpaceEnum _locationCoordinateSpace;

        #endregion Inspector Variables

        protected override void Initialise()
        {
            ConfigureSpawn(this, _locationType, _locationCoordinateSpace, _spawn);
            _spawn.Initialise(this.Owner);
        }

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <returns></returns>
        protected override void Execute(bool isStart)
        {
            ConfigureSpawn(this, _locationType, _locationCoordinateSpace, _spawn);
#if UNITY_EDITOR
            UpdateCachedValues();
#endif
            _spawn.SpawnBurst(_burstMinimum, _burstMaximum);
        }

        #region Public API - Helper Methods

        /// <summary>
        /// Update any cached values to reflect changes.
        /// </summary>
        /// Pro Pooling caches several values for performance reasons. Be sure to call this if you 
        /// make any changes to the properties.
        /// Note that this method does not do full validation or warn on issues with the state.
        public void UpdateCachedValues()
        {
            _spawn.UpdateCachedValues();
        }


        /// <summary>
        /// Check if the current configuration is valid for spawning and warn if not.
        /// </summary>
        public bool ValidateConfiguration()
        {
            return _spawn.ValidateConfiguration(Owner.gameObject.name);
        }

        #endregion Public API - Helper Methods

        internal static void ConfigureSpawn(GameAction gameAction, LocationTypeEnum _locationType, LocationCoordinateSpaceEnum _locationCoordinateSpace, Spawn _spawn)
        {
            switch (_locationType)
            {
                case LocationTypeEnum.ThisGameObject:
                    _spawn.LocationType = Spawn.LocationTypeEnum.SpawnersLocation;
                    _spawn.SpawnersPosition = gameAction.Owner.transform;
                    break;
                case LocationTypeEnum.CollidingGameObject:
                    _spawn.LocationType = Spawn.LocationTypeEnum.SpawnersLocation;
                    if (gameAction.InvocationContext.OtherGameObject != null)
                        _spawn.SpawnersPosition = gameAction.InvocationContext.OtherGameObject.transform;
                    break;
                case LocationTypeEnum.Positions:
                    _spawn.LocationType = Spawn.LocationTypeEnum.Positions;
                    if (_locationCoordinateSpace == LocationCoordinateSpaceEnum.LocalToThisGameObject)
                    {
                        _spawn.SpawnLocationPositionsAreLocal = true;
                        _spawn.SpawnersPosition = gameAction.Owner.transform;
                    }
                    else if (_locationCoordinateSpace == LocationCoordinateSpaceEnum.LocalToThisGameObject)
                    {
                        _spawn.SpawnLocationPositionsAreLocal = true;
                        if (gameAction.InvocationContext.OtherGameObject != null)
                            _spawn.SpawnersPosition = gameAction.InvocationContext.OtherGameObject.transform;
                    }
                    break;
                case LocationTypeEnum.Transforms:
                    _spawn.LocationType = Spawn.LocationTypeEnum.Transforms;
                    break;
            }
        }

        #region IScriptableObjectContainerSyncReferences

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        /// <param name="objectReferences"></param>
        public override void SetReferencesFromContainer(UnityEngine.Object[] objectReferences)
        {
            if (objectReferences != null && _spawn != null) {
                var index = 0;
                if (objectReferences.Length > index)
                    _spawn.SpawnPools = objectReferences[index++] as Components.Pools;

                if (_spawn.SpawnablePrefabs != null)
                    foreach (var spawnablePrefab in _spawn.SpawnablePrefabs)
                        if (objectReferences.Length > index)
                            spawnablePrefab.Prefab = objectReferences[index++] as GameObject;

                if (_spawn.SpawnLocationTransforms != null)
                    foreach (var spawnLocationTransform in _spawn.SpawnLocationTransforms)
                        if (objectReferences.Length > index)
                            spawnLocationTransform.Transform = objectReferences[index++] as Transform;
            }
        }

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        public override UnityEngine.Object[] GetReferencesForContainer()
        {
            var objectReferences = new Object[1 + _spawn.SpawnablePrefabs.Count + _spawn.SpawnLocationTransforms.Count];
            var index = 0;
            objectReferences[index++] = _spawn.SpawnPools;

            foreach (var spawnablePrefab in _spawn.SpawnablePrefabs)
                objectReferences[index++] = spawnablePrefab.Prefab;

            foreach (var spawnLocationTransform in _spawn.SpawnLocationTransforms)
                objectReferences[index++] = spawnLocationTransform.Transform;

            return objectReferences;
        }

        #endregion IScriptableObjectContainerSyncReferences
    }
}
#endif