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

using UnityEngine;

namespace ProPooling.Components
{
    /// <summary>
    /// Component to run Game Framework actions when the item is pulled from the pool.
    /// </summary>
    [AddComponentMenu("Pro Pooling/On PoolItem Lifecycle Actions", 8)]
    [HelpURL("http://www.flipwebapps.com/pro-pooling/")]
    public class OnPoolItemLifecycleActions : MonoBehaviour, IPoolComponent
    {
#if GAME_FRAMEWORK
            [Tooltip("A list of actions that should be run when this item is spawned.")]
            public GameFramework.GameStructure.Game.ObjectModel.GameActionReference[] OnSpawnedActions = new GameFramework.GameStructure.Game.ObjectModel.GameActionReference[0];

            [Tooltip("A list of actions that should be run when this item is despawned.")]
            public GameFramework.GameStructure.Game.ObjectModel.GameActionReference[] OnDespawnedActions = new GameFramework.GameStructure.Game.ObjectModel.GameActionReference[0];
#endif

        void Start()
        {
#if GAME_FRAMEWORK
            GameFramework.GameStructure.Game.GameActionHelper.InitialiseGameActions(OnSpawnedActions, this);
            GameFramework.GameStructure.Game.GameActionHelper.InitialiseGameActions(OnDespawnedActions, this);
#endif
        }

        #region IPoolComponent

        /// <summary>
        /// Activate all child gameobjects when spawned from a pool
        /// </summary>
        /// <param name="poolItem"></param>
        public void OnSpawned(PoolItem poolItem)
        {
#if GAME_FRAMEWORK
            GameFramework.GameStructure.Game.GameActionHelper.ExecuteGameActions(OnSpawnedActions, false);
#endif
        }

        public void OnDespawned(PoolItem poolItem) {
#if GAME_FRAMEWORK
            GameFramework.GameStructure.Game.GameActionHelper.ExecuteGameActions(OnDespawnedActions, false);
#endif
        }

        #endregion IPoolComponent
    }
}