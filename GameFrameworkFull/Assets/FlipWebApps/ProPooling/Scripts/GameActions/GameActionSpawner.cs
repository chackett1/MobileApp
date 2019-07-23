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
using GameFramework.GameStructure.Game;
using GameFramework.GameStructure.Game.ObjectModel.Abstract;
using GameFramework.Helper;
using ProPooling.Components;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProPooling.GameActions
{
    /// <summary>
    /// Game Framework GameAction class to spawn the specified gameobject
    /// </summary>
    [System.Serializable]
    [ClassDetails("Spawner", "Pro Pooling/Spawner", "Spawner")]
    public class GameActionSpawner : GameActionTarget
    {
        #region Inspector Variables

        /// <summary>
        /// The SpawnOverTime instance with configuration and helper code
        /// </summary>
        [Tooltip("The SpawnOverTime instance with configuration and helper code")]
        [SerializeField]
        SpawnOverTime _spawnOverTime;


        /// <summary>
        /// What type of location to use for spawning
        /// </summary>
        [Tooltip("What type of location to use for spawning")]
        [SerializeField]
        GameActionSpawnBurst.LocationTypeEnum _locationType;

        /// <summary>
        /// The coordinate space to use.
        /// </summary>
        [Tooltip("The coordinate space to use")]
        [SerializeField]
        GameActionSpawnBurst.LocationCoordinateSpaceEnum _locationCoordinateSpace;

        #endregion Inspector Variables

        protected override void Initialise()
        {
            GameActionSpawnBurst.ConfigureSpawn(this, _locationType, _locationCoordinateSpace, _spawnOverTime.Spawn);
            _spawnOverTime.Initialise(this.Owner);
        }

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <returns></returns>
        protected override void Execute(bool isStart)
        {
            GameActionSpawnBurst.ConfigureSpawn(this, _locationType, _locationCoordinateSpace, _spawnOverTime.Spawn);

#if UNITY_EDITOR
            UpdateCachedValues();
#endif

            _spawnOverTime.Start();
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
            _spawnOverTime.UpdateCachedValues();
        }


        /// <summary>
        /// Check if the current configuration is valid for spawning and warn if not.
        /// </summary>
        public bool ValidateConfiguration()
        {
            return _spawnOverTime.ValidateConfiguration(Owner.gameObject.name);
        }

        #endregion Public API - Helper Methods


        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        /// <param name="objectReferences"></param>
        public override void SetReferencesFromContainer(UnityEngine.Object[] objectReferences)
        {
            if (objectReferences != null && _spawnOverTime != null)
            {
                var index = 0;
                if (objectReferences.Length > index)
                    _spawnOverTime.Spawn.SpawnPools = objectReferences[index++] as Components.Pools;

                if (_spawnOverTime.Spawn != null)
                {
                    if (_spawnOverTime.Spawn.SpawnablePrefabs != null)
                        foreach (var spawnablePrefab in _spawnOverTime.Spawn.SpawnablePrefabs)
                            if (objectReferences.Length > index)
                                spawnablePrefab.Prefab = objectReferences[index++] as GameObject;

                    if (_spawnOverTime.Spawn.SpawnLocationTransforms != null)
                        foreach (var spawnLocationTransform in _spawnOverTime.Spawn.SpawnLocationTransforms)
                            if (objectReferences.Length > index)
                                spawnLocationTransform.Transform = objectReferences[index++] as Transform;
                }
            }
        }

        /// <summary>
        /// Workaround for ObjectReference issues with ScriptableObjects (See ScriptableObjectContainer for details)
        /// </summary>
        public override UnityEngine.Object[] GetReferencesForContainer()
        {
            var objectReferences = new Object[1 + _spawnOverTime.Spawn.SpawnablePrefabs.Count + _spawnOverTime.Spawn.SpawnLocationTransforms.Count];
            var index = 0;
            objectReferences[index++] = _spawnOverTime.Spawn.SpawnPools;

            foreach (var spawnablePrefab in _spawnOverTime.Spawn.SpawnablePrefabs)
                objectReferences[index++] = spawnablePrefab.Prefab;

            foreach (var spawnLocationTransform in _spawnOverTime.Spawn.SpawnLocationTransforms)
                objectReferences[index++] = spawnLocationTransform.Transform;

            return objectReferences;
        }
    }
}
#endif