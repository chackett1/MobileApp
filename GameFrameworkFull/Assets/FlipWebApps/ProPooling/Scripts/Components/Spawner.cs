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

namespace ProPooling.Components
{
    /// <summary>
    /// Component for automatically spawning items from the Global Pools. 
    /// </summary>
    [AddComponentMenu("Pro Pooling/Spawner", 10)]
    [HelpURL("http://www.flipwebapps.com/pro-pooling/")]
    public class Spawner : MonoBehaviour
    {
        #region Enums

        // enum to determine when to start spawning.
        public enum AutomaticallyRunEnum
        {
            /// <summary>
            /// Don't automatically spawn - start spawning through code or an event hooked up to the StartSpawning method.
            /// </summary>
            Never,
            /// <summary>
            /// Start spawning automatically in the Start method.
            /// </summary>
            OnStart,
        }

        #endregion Enums

        #region Inspector Variables

        /// <summary>
        /// When to automatically start spawning
        /// </summary>
        public AutomaticallyRunEnum AutomaticallyRun { get { return _automaticallyRun; } set { _automaticallyRun = value; } }
        [Tooltip("When to automatically start spawning.")]
        [SerializeField]
        AutomaticallyRunEnum _automaticallyRun = AutomaticallyRunEnum.OnStart;


        /// <summary>
        /// The SpawnOverTime instance with configuration and helper code
        /// </summary>
        public SpawnOverTime SpawnOverTime { get { return _spawnOverTime; } set { _spawnOverTime = value; } }
        [Tooltip("The SpawnOverTime instance with configuration and helper code")]
        [SerializeField]
        SpawnOverTime _spawnOverTime;

        #endregion Inspector Variables

        void Start()
        {
            SpawnOverTime.Initialise(this);

            // start spawning if OnStart mode
            if (AutomaticallyRun == AutomaticallyRunEnum.OnStart)
                SpawnOverTime.Start();
        }

        #region Public API - Lifecycle

        /// <summary>
        /// Start spawning items according to the current settings.
        /// </summary>
        public void StartSpawning()
        {
            SpawnOverTime.Start();
        }


        /// <summary>
        /// Stop spawning.
        /// </summary>
        public void StopSpawning()
        {
            SpawnOverTime.Stop();
        }


        /// <summary>
        /// Restart the spawning from the beginning.
        /// </summary>
        public void RestartSpawning()
        {
            SpawnOverTime.Restart();
        }


        /// <summary>
        /// Pause spawning. Note any running start delay / spawn wait intervals will still complete for performance reasons. 
        /// </summary>
        public void PauseSpawning()
        {
            SpawnOverTime.Pause();
        }


        /// <summary>
        /// Resume spawning. Note any running start delay / spawn wait intervals will still complete for performance reasons. 
        /// </summary>
        public void ResumeSpawning()
        {
            SpawnOverTime.Resume();
        }

        #endregion Public API - Lifecycle

        #region Public API - Ad hoc spawning

        /// <summary>
        /// Spawn a burst of the given size
        /// </summary>
        /// This might update next spawn location, next prefab etc. But won't affect any spawn counters
        public void SpawnBurst(int size = 1)
        {
            SpawnOverTime.SpawnBurst(size);
        }


        /// <summary>
        /// Spawn a burst of items between the specified minimum and maximum values
        /// </summary>
        /// This might update next spawn location, next prefab etc. But won't affect any spawn counters
        public void SpawnBurst(int minimum, int maximum)
        {
            SpawnOverTime.SpawnBurst(minimum, maximum);
        }

        /// <summary>
        /// Spawn a single new item.
        /// </summary>
        /// This might update next spawn location, next prefab etc. But won't affect any spawn counters
        public void SpawnOne()
        {
            SpawnOverTime.SpawnOne();
        }

        #endregion Public API - Ad hoc spawning

        #region Public API - Helper Methods

        /// <summary>
        /// Update any cached values to reflect changes.
        /// </summary>
        /// Pro Pooling caches several values for performance reasons. Be sure to call this if you 
        /// make any changes to the properties.
        /// Note that this method does not do full validation or warn on issues with the state.
        public void UpdateCachedValues()
        {
            SpawnOverTime.UpdateCachedValues();
        }


        /// <summary>
        /// Check if the current configuration is valid for spawning and warn if not.
        /// </summary>
        public bool ValidateConfiguration()
        {
            return SpawnOverTime.ValidateConfiguration(gameObject.name);
        }

        #endregion Public API - Helper Methods

        #region Debugging

        /// <summary>
        /// Draw system gizmos to show spawn location.
        /// </summary>
        void OnDrawGizmosSelected()
        {
            SpawnOverTime.DrawGizmos(this.transform.position);
        }

        #endregion Debugging

    }
}
