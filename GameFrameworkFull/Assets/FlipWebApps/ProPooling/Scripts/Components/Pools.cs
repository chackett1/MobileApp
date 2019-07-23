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
    /// Component for adding additional Pools to the Global Pools based on the lifecycle of this component. 
    /// </summary>
    /// Pools will be added / removed to the Global Pools when this component is enabled / disabled so you
    /// may use this for adding any per scene pools that you need. 
    /// Pooled instances will be added as children of the containing GameObject and so will be destroyed
    /// the GameObject to which this component is added is destroyed.
    [AddComponentMenu("Pro Pooling/Pools", 1)]
    [HelpURL("http://www.flipwebapps.com/pro-pooling/")]
    public class Pools : PoolsBase
    {

        #region Inspector Variables

        /// <summary>
        /// Whether to add pools to the GlobalPools component
        /// </summary>
        public bool AddToGlobalPools
        {
            get { return _addToGlobalPools; }
            set { _addToGlobalPools = value; }
        }
        [SerializeField]
        [Tooltip("Whether to add pools to the GlobalPools component.")]
        bool _addToGlobalPools;

        #endregion Inspector Variables

        bool _deferAddPoolsToStart;

        void OnEnable()
        {
            if (AddToGlobalPools)
            {
                if (GlobalPools.IsActive)
                {
                    AddPoolsToGlobalPools();
                }
                else
                {
                    // Unity doesn't do all awake() and then all OnEnable(). For each script that it loads 
                    // it does Awake() and then OnEnable() before moving on to the next script.
                    // If the GlobalPools is in the same scene then there is a change that this script will 
                    // run before GlobalPools is setup. This can be solved by script execution order, but 
                    // we try and avoid failing if that isn't setup..
                    // Ideally we add pools in OnEnable before the game starts, but we put in a fall back
                    // with a warning so things still run.
                    _deferAddPoolsToStart = true;
                    Debug.LogWarningFormat("Pro Pooling: Please increase the Global Poolss script execution order priority above so Pool Items can be added in the OnEnable phase for best effect (things will still work without).", name);
                    //StartCoroutine(DelayedAddPoolsToGlobalPools());
                }
            }
            else
            {
                InitialisePools();
            }
        }

        void Start()
        {
            // see description above.
            if (_deferAddPoolsToStart)
            {
                if (!GlobalPools.IsActive)
                    Debug.LogWarningFormat("Pro Pooling: GlobalPools is not active when referenced from AddPools on GameObject '{0}' so pools could not be added. Make sure a GlobalPools is added to your scene, or if already added increase it's script execution order priority.", name);
                else
                    AddPoolsToGlobalPools();
                _deferAddPoolsToStart = false;
            }
        }

        void OnDisable()
        {
            if (AddToGlobalPools)
                RemovePoolsFromGlobalPools();
        }


        /// <summary>
        /// Add the pools to the pool manager
        /// </summary>
        public void AddPoolsToGlobalPools() {
            foreach (var pool in Pools)
            {
                if (pool.Prefab == null)
                {
                    Debug.LogWarningFormat("Pro Pooling: A pools on the GameObject '{0}' does not have a valid prefab or gameobject. It will not be added to the Global Pools", name);
                    continue;
                }

                if (GlobalPools.Instance.ContainsPool(pool.Prefab.name))
                {
                    Debug.LogWarningFormat("Pro Pooling: A pool with the name {0} defined in the AddPools component on the GameObject '{1}' already exists in the Global Pools and will not be added. Rename the prefab to a unique name.", pool.Prefab.name, name);
                    continue;
                }

                pool.ItemParent = transform;
                pool.Initialise(this);
                GlobalPools.Instance.AddPool(pool);
            }
        }


        //System.Collections.IEnumerator DelayedAddPoolsToGlobalPools()
        //{
        //    yield return new WaitForEndOfFrame();
        //    if (GlobalPools.IsActive)
        //    {
        //        Debug.LogWarningFormat("Pro Pooling: Please increase the Global Poolss script execution order priority above so Pool Items can be added in the OnEnable phase for best effect (things will still work without).", name);
        //        AddPoolsToGlobalPools();
        //    }
        //    else
        //        Debug.LogWarningFormat("Pro Pooling: GlobalPools is not active when referenced from AddPools on GameObject '{0}' so pools could not be added. Make sure a GlobalPools is added to your scene, or if already added increase it's script execution order priority.", name);
        //}


        /// <summary>
        /// Remove the pools from the pool manager
        /// </summary>
        public void RemovePoolsFromGlobalPools()
        {
            foreach (var pool in Pools)
            {
                if (pool.Prefab == null)
                    continue;

                GlobalPools.Instance.RemovePool(pool);
            }
        }
    }
}
