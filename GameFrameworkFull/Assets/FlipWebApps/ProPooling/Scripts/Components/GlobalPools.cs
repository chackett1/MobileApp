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

namespace ProPooling.Components
{
    /// <summary>
    /// Component for handling global pools. You can also manage your own pools directly if 
    /// you so wish by adding your own List&lt;Pool&gt; property to your component.
    /// </summary>
    [AddComponentMenu("Pro Pooling/Global Pools", 9)]
    [DisallowMultipleComponent]
    [HelpURL("http://www.flipwebapps.com/pro-pooling/")]
    public class GlobalPools : PoolsBase
    {
        #region Singleton
        // Static singleton property
        public static GlobalPools Instance { get; private set; }
        public static bool IsActive { get { return Instance != null; } }

        protected void Awake()
        {
            // Check if singleton is already created. If so there is possible a conflict somewhere
            if (Instance != null)
            {
                if (Instance != this)
                {
                    Debug.LogWarning("Pro Pooling: Multiple instances of GlobalPools were detected. Please ensure that you only have one copy added to your game.");
                    Destroy(gameObject);
                }
                return;
            }

            // Here we save our singleton instance
            Instance = this as GlobalPools;

            // Persist
            if (MarkAsDontDestroyOnload)
                DontDestroyOnLoad(gameObject);

            // setup specifics for instantiated object only.
            InitialisePools();
        }

        void OnDestroy()
        {
            // cleanup for instantiated object only.
            if (Instance == this) {
            }
        }
        #endregion Singleton

        #region Inspector Variables
        /// <summary>
        /// Mark as DontDestroyOnload
        /// </summary>
        public bool MarkAsDontDestroyOnload
        {
            get { return _markAsDontDestroyOnload; }
            set { _markAsDontDestroyOnload = value; }
        }
        [SerializeField]
        [Tooltip("GlobalPools typically will live across scenes. Enable this to have the singleton marked with DontDestroyOnLoad to have this happen automatically. If you are loading scenes additively and managing this yourself then you may not need this.")]
        bool _markAsDontDestroyOnload = true;

        #endregion Inspector Variables
    }
}
