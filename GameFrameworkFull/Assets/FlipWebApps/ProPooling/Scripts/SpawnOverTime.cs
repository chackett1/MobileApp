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
using UnityEngine.Events;

namespace ProPooling
{
    /// <summary>
    /// Class for spawning items from a pool over time.
    /// </summary>
    [System.Serializable]
    public class SpawnOverTime
    {
        #region Enums


        /// <summary>
        /// How long to run the spawning for.
        /// </summary>
        public enum RunPeriodEnum
        {
            /// <summary>
            /// Spawn forever unless manually stopped.
            /// </summary>
            Forever,
            /// <summary>
            /// Sparn until the specified time is over.
            /// </summary>
            GivenNumberOfSeconds,
            /// <summary>
            /// Start the specified number of instances.
            /// </summary>
            GivenNumberOfTimes,
            /// <summary>
            /// No automatic spawning - trigger manually through SpawnXxx().
            /// </summary>
            Manually,
        }

        /// <summary>
        /// What spawn interval to use
        /// </summary>
        public enum SpawnIntervalModeEnum
        {
            /// <summary>
            /// Use a constant spawn interval
            /// </summary>
            Constant,
            /// <summary>
            /// Use a random interval between two constants
            /// </summary>
            RandomBetweenTwoConstants
        }

        #endregion Enums

        #region Inspector Variables

        /// <summary>
        /// The spawn instance with configuration and helper code
        /// </summary>
        public Spawn Spawn { get { return _spawn; } set { _spawn = value; } }
        [Tooltip("The Spawn instance with configuration and helper code")]
        [SerializeField]
        Spawn _spawn;


        /// <summary>
        /// An optional delay in seconds before starting spawning
        /// </summary>
        public float Delay { get { return _delay; } set { _delay = value; } }
        [Tooltip("A optional delay in seconds before starting spawning")]
        [SerializeField]
        float _delay = 0f;


        /// <summary>
        /// How long to spawn for
        /// </summary>
        public RunPeriodEnum RunPeriod { get { return _runPeriod; } set { _runPeriod = value; } }
        [Tooltip("How long to spawn for.")]
        [SerializeField]
        RunPeriodEnum _runPeriod = RunPeriodEnum.Forever;


        /// <summary>
        /// The number of instances to spawn if using RunModeEnum.Count
        /// </summary>
        public int SpawnCount { get { return _spawnCount; } set { _spawnCount = value; } }
        [Tooltip("The number of instances to spawn")]
        [SerializeField]
        int _spawnCount = 1;


        /// <summary>
        /// The time in seconds to spawn items for if using RunModeEnum.Time
        /// </summary>
        public float SpawnTime { get { return _spawnTime; } set { _spawnTime = value; } }
        [Tooltip("The time in seconds to spawn items for if using RunModeEnum.Time")]
        [SerializeField]
        float _spawnTime = 1f;


        /// <summary>
        /// Whether to loop for RunModeEnum.Count and RunModeEnum.Time when spawning is completed
        /// </summary>
        public bool Loop { get { return _loop; } set { _loop = value; } }
        [Tooltip("Whether to loop for RunModeEnum.Count and RunModeEnum.Time when spawning is completed")]
        [SerializeField]
        bool _loop = false;


        /// <summary>
        /// Now to determine the time interval between spawning new items
        /// </summary>
        public SpawnIntervalModeEnum SpawnIntervalMode { get { return _spawnIntervalMode; } set { _spawnIntervalMode = value; } }
        [Tooltip("Now to determine the time interval between spawning new items")]
        [SerializeField]
        SpawnIntervalModeEnum _spawnIntervalMode = SpawnIntervalModeEnum.Constant;


        /// <summary>
        /// The time interval between spawning new items
        /// </summary>
        public float SpawnInterval { get { return _spawnInterval; } set { _spawnInterval = value; } }
        [Tooltip("The time interval between spawning new items")]
        [SerializeField]
        float _spawnInterval = 1f;


        /// <summary>
        /// The minimum time interval between spawning new items if using a random interval between two constants
        /// </summary>
        public float MinimumSpawnInterval { get { return _minimumSpawnInterval; } set { _minimumSpawnInterval = value; } }
        [Tooltip("The minimum time interval between spawning new items if using a random interval between two constants")]
        [SerializeField]
        float _minimumSpawnInterval = 1f;


        /// <summary>
        /// The maximum time interval between spawning new items if using a random interval between two constants
        /// </summary>
        public float MaximumSpawnInterval { get { return _maximumSpawnInterval; } set { _maximumSpawnInterval = value; } }
        [Tooltip("The maximum time interval between spawning new items if using a random interval between two constants")]
        [SerializeField]
        float _maximumSpawnInterval = 1f;


        /// <summary>
        /// Bursts that should be spawned
        /// </summary>
        public List<SpawnBurstSettings> SpawnBursts { get { return _spawnBursts; } set { _spawnBursts = value; } }
        [Tooltip("Bursts that should be spawned.")]
        [SerializeField]
        List<SpawnBurstSettings> _spawnBursts;


        /// <summary>
        /// Methods that should be called when spawning is started (after any initial delay)
        /// </summary>
        public UnityEvent OnSpawnStart { get { return _onSpawnStart; } set { _onSpawnStart = value; } }
        [Tooltip("Methods that should be called when spawning is started (after any initial delay).")]
        [SerializeField]
        UnityEvent _onSpawnStart;


        /// <summary>
        /// Methods that should be called for each spawn loop.
        /// </summary>
        public UnityEvent OnSpawnLoop { get { return _onSpawnLoop; } set { _onSpawnLoop = value; } }
        [Tooltip("Methods that should be called for each spawn loop.")]
        [SerializeField]
        UnityEvent _onSpawnLoop;


        /// <summary>
        /// "Methods that should be called when spawning has completed.
        /// </summary>
        public UnityEvent OnSpawnComplete { get { return _onSpawnComplete; } set { _onSpawnComplete = value; } }
        [Tooltip("Methods that should be called when spawning has completed.")]
        [SerializeField]
        UnityEvent _onSpawnComplete;

#if GAME_FRAMEWORK
        [Tooltip("A list of actions that should be run when spawning is started.")]
        public GameFramework.GameStructure.Game.ObjectModel.GameActionReference[] OnSpawnStartActionReferences = new GameFramework.GameStructure.Game.ObjectModel.GameActionReference[0];
        [Tooltip("A list of actions that should be run for each spawn loop.")]
        public GameFramework.GameStructure.Game.ObjectModel.GameActionReference[] OnSpawnLoopActionReferences = new GameFramework.GameStructure.Game.ObjectModel.GameActionReference[0];
        [Tooltip("A list of actions that should be run when spawning completes.")]
        public GameFramework.GameStructure.Game.ObjectModel.GameActionReference[] OnSpawnCompleteActionReferences = new GameFramework.GameStructure.Game.ObjectModel.GameActionReference[0];
#endif

        #endregion Inspector Variables

        #region Public Properties

        /// <summary>
        /// The number of items currently spawned if using RunModeEnum.Count
        /// </summary>
        public int ItemsSpawned { get { return _iterationsCounter; } }


        /// <summary>
        /// The number of items remaining to spawn if using RunModeEnum.Count
        /// </summary>
        public int ItemsRemaining { get { return SpawnCount - _iterationsCounter; } }


        /// <summary>
        /// The time in seconds we have been spawning for if using RunModeEnum.Time
        /// </summary>
        public float TimeSpawning { get { return _totalTimeElapsed; } }


        /// <summary>
        /// The time in seconds remaining to spawn for if using RunModeEnum.Time
        /// </summary>
        public float TimeRemaining { get { return SpawnTime - _totalTimeElapsed; } }

        #endregion Public Properties

        Coroutine _spawnCoroutine;
        WaitForSeconds _waitForSecondsDelay;

        bool _isPaused;
        int _iterationsCounter;
        float _totalTimeElapsed;
        double _intervalTimeElapsed;
        List<SpawnBurstSettings> _sortedBursts;
        int _burstIndex;

        public MonoBehaviour Owner { get; set; }

        #region Public API - Lifecycle

        /// <summary>
        /// Perform any initialisation.
        /// </summary>
        /// <param name="owner"></param>
        public void Initialise(MonoBehaviour owner)
        {
            Spawn.Initialise(owner);

            Owner = owner;
            SetCachedValues();
            ValidateConfiguration(owner.gameObject.name);

#if GAME_FRAMEWORK
            GameFramework.GameStructure.Game.GameActionHelper.InitialiseGameActions(OnSpawnStartActionReferences, Owner);
            GameFramework.GameStructure.Game.GameActionHelper.InitialiseGameActions(OnSpawnLoopActionReferences, Owner);
            GameFramework.GameStructure.Game.GameActionHelper.InitialiseGameActions(OnSpawnCompleteActionReferences, Owner);
#endif
        }


        /// <summary>
        /// Reset the status
        /// </summary>
        public void Reset()
        {
            _iterationsCounter = 0;
            _totalTimeElapsed = 0;
            _intervalTimeElapsed = 0;
            _burstIndex = 0;
            Spawn.Reset();
        }


        /// <summary>
        /// Start spawning items according to the current settings.
        /// </summary>
        public void Start()
        {
            if (_spawnCoroutine != null) return;
            if (!Spawn.IsConfigurationValidated)
            {
                Debug.LogWarningFormat("Pro Pooling: The current configuration on the spawner on GameObject '{0}' is not validated and nothing will be spawned. See previous warnings for details.", Owner.gameObject.name);
                return;
            }

            _isPaused = false;
            Reset();
            // don't need to start if manual mode.
            if (RunPeriod != RunPeriodEnum.Manually)
                _spawnCoroutine = Owner.StartCoroutine(SpawnCoroutine());
        }


        /// <summary>
        /// Stop spawning.
        /// </summary>
        public void Stop()
        {
            if (_spawnCoroutine == null) return;
            Owner.StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }


        /// <summary>
        /// Restart the spawning from the beginning.
        /// </summary>
        public void Restart()
        {
            Stop();
            Start();
        }


        /// <summary>
        /// Pause spawning. Note any running start delay / spawn wait intervals will still complete for performance reasons. 
        /// </summary>
        public void Pause()
        {
            _isPaused = true;
        }


        /// <summary>
        /// Resume spawning. Note any running start delay / spawn wait intervals will still complete for performance reasons. 
        /// </summary>
        public void Resume()
        {
            _isPaused = false;
        }


        System.Collections.IEnumerator SpawnCoroutine()
        {
            if (Delay > 0)
                yield return _waitForSecondsDelay;

            // run start events / actions
            if (OnSpawnStart != null)
                OnSpawnStart.Invoke();
#if GAME_FRAMEWORK
            GameFramework.GameStructure.Game.GameActionHelper.ExecuteGameActions(OnSpawnStartActionReferences, false);
#endif

            do
            {
                while (RunPeriod == RunPeriodEnum.Forever ||
                    (RunPeriod == RunPeriodEnum.GivenNumberOfTimes && _iterationsCounter < SpawnCount) ||
                    (RunPeriod == RunPeriodEnum.GivenNumberOfSeconds && _totalTimeElapsed < SpawnTime))
                {
                    // run per loop events / actions
                    if (OnSpawnLoop != null)
                        OnSpawnLoop.Invoke();
#if GAME_FRAMEWORK
                    GameFramework.GameStructure.Game.GameActionHelper.ExecuteGameActions(OnSpawnLoopActionReferences, false);
#endif
                    Spawn.SpawnOne();

                    // spawn interval
                    var spawnInterval = 0d;
                    switch (SpawnIntervalMode)
                    {
                        case SpawnIntervalModeEnum.Constant:
                            spawnInterval = SpawnInterval;
                            break;
                        case SpawnIntervalModeEnum.RandomBetweenTwoConstants:
                            spawnInterval = Random.Range(MinimumSpawnInterval, MaximumSpawnInterval);
                            break;
                    }

                    // if paused or waiting for an interval to complete then we wait.
                    while (_intervalTimeElapsed < spawnInterval || _isPaused)
                    {
                        yield return null;

                        // if not paused increase total time and interval counters and proces bursts
                        if (!_isPaused)
                        {
                            _totalTimeElapsed += Time.deltaTime;
                            _intervalTimeElapsed += Time.deltaTime;

                            // check for bursts        
                            if (_burstIndex < _sortedBursts.Count)
                            {
                                var currentBurst = _sortedBursts[_burstIndex];
                                if (_totalTimeElapsed >= currentBurst.Time)
                                {
                                    Spawn.SpawnBurst(currentBurst.Minimum, currentBurst.Maximum);
                                    _burstIndex++;
                                }
                            }
                        }
                    }

                    _iterationsCounter++;
                    _intervalTimeElapsed = 0;
                }

                // reset and loop if needed.
                Reset();
            } while (Loop);

            // run complete events / actions
            if (OnSpawnComplete != null)
                OnSpawnComplete.Invoke();
#if GAME_FRAMEWORK
            GameFramework.GameStructure.Game.GameActionHelper.ExecuteGameActions(OnSpawnCompleteActionReferences, false);
#endif

            _spawnCoroutine = null;
        }

        #endregion Public API - Lifecycle

        #region Public API - Ad hoc spawning

        /// <summary>
        /// Spawn a burst of the given size
        /// </summary>
        /// This might update next spawn location, next prefab etc. But won't affect any spawn counters
        public void SpawnBurst(int size = 1)
        {
            Spawn.SpawnBurst(size);
        }


        /// <summary>
        /// Spawn a burst of items between the specified minimum and maximum values
        /// </summary>
        /// This might update next spawn location, next prefab etc. But won't affect any spawn counters
        public void SpawnBurst(int minimum, int maximum)
        {
            Spawn.SpawnBurst(minimum, maximum);
        }

        /// <summary>
        /// Spawn a single new item.
        /// </summary>
        /// This might update next spawn location, next prefab etc. But won't affect any spawn counters
        public void SpawnOne()
        {
            Spawn.SpawnOne();
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
            Spawn.UpdateCachedValues();
            SetCachedValues();
        }


        /// <summary>
        /// Set cached values
        /// </summary>
        void SetCachedValues()
        {
            _waitForSecondsDelay = new WaitForSeconds(Delay);
            _sortedBursts = new List<SpawnBurstSettings>(SpawnBursts);
            _sortedBursts.Sort((x, y) => x.Time.CompareTo(y.Time));
        }


        /// <summary>
        /// Check if the current configuration is valid for spawning and warn if not.
        /// </summary>
        public bool ValidateConfiguration(string gameObjectName)
        {
            return Spawn.ValidateConfiguration(gameObjectName);
        }

        #endregion Other Helper Methods

        #region Debugging

        /// <summary>
        /// Draw system gizmos to show spawn location.
        /// </summary>
        public void DrawGizmos(Vector3 position)
        {
            Spawn.DrawGizmos(position);
        }

        #endregion Debugging

        #region Utility Classes

        /// <summary>
        /// Class for information about a SpawnBurst.
        /// </summary>
        [System.Serializable]
        public class SpawnBurstSettings
        {
            /// <summary>
            /// The time in seconds to spawn the burst at.
            /// </summary>
            public float Time { get { return _time; } set { _time = value; } }
            [Tooltip("The time in seconds to spawn the burst at.")]
            [SerializeField]
            float _time;

            /// <summary>
            /// The minimum number of itmes to spawn in this burst
            /// </summary>
            public int Minimum { get { return _minimum; } set { _minimum = value; } }
            [Tooltip("The minimum number of itmes to spawn in this burst")]
            [SerializeField]
            int _minimum;

            /// <summary>
            /// The maximum number of itmes to spawn in this burst
            /// </summary>
            public int Maximum { get { return _maximum; } set { _maximum = value; } }
            [Tooltip("The maximumnumber of itmes to spawn in this burst")]
            [SerializeField]
            int _maximum;
        }
        #endregion Utility Classes

    }
}
