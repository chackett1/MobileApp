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
using UnityEngine;
using UnityEngine.Assertions;

namespace ProPooling.GameActions
{
    /// <summary>
    /// Game Framework GameAction class to despawn the specified gameobject
    /// </summary>
    [System.Serializable]
    [ClassDetails("Despawn", "Pro Pooling/Despawn", "Despawn the specified gameobject.")]
    public class GameActionDespawn : GameActionTarget
    {
        // enum to determine when to despawn to.
        public enum DespawnToEnum
        {
            /// <summary>
            /// Spawn from the GlobalPools
            /// </summary>
            GlobalPools,
            /// <summary>
            /// Spawn from a Pools component
            /// </summary>
            PoolsComponent,
        }


#region Inspector Variables

        /// <summary>
        /// When to despawn to
        /// </summary>
        public DespawnToEnum DespawnTo { get { return _despawnTo; } set { _despawnTo = value; } }
        [Tooltip("When to despawn to")]
        [SerializeField]
        DespawnToEnum _despawnTo = DespawnToEnum.GlobalPools;


        /// <summary>
        /// If Despawn To is set to PoolsComponent then a reference to the component to use.
        /// </summary>
        public Pools DespawnPools { get { return _despawnPools; } set { _despawnPools = value; } }
        [Tooltip("If Despawn To is set to PoolsComponent then a reference to the component to use.")]
        [SerializeField]
        Pools _despawnPools;

#endregion Inspector Variables

        protected override void Initialise()
        {
            Assert.IsFalse(DespawnTo == DespawnToEnum.GlobalPools && (!GlobalPools.IsActive || !GlobalPools.Instance.IsInitialised), string.Format("Pro Pooling: GlobalPools is not active when referenced from the Despawn Action on GameObject '{0}'. Make sure a GlobalPools is added to your scene, or if already added increase it's script execution order priority.", Owner.name));
            Assert.IsNotNull(DespawnPools, string.Format("Pro Pooling: Despawn Pools is not set in the Despawn Action on GameObject '{0}'.", Owner.name));
        }

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <returns></returns>
        protected override void Execute(bool isStart)
        {
            var targetFinal = GameActionHelper.ResolveTarget(TargetType, this, Target);
            if (targetFinal != null)
            {
                switch (DespawnTo)
                {
                    case DespawnToEnum.GlobalPools:
                        if (!GlobalPools.IsActive || !GlobalPools.Instance.IsInitialised) return;
                        GlobalPools.Instance.Despawn(targetFinal);
                        break;
                    case DespawnToEnum.PoolsComponent:
                        if (DespawnPools == null) return;
                        DespawnPools.Despawn(targetFinal);
                        break;
                }
            }
        }
    }
}
#endif