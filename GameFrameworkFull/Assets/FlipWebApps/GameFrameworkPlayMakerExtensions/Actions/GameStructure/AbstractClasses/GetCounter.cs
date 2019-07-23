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

#if PLAYMAKER
using GameFramework.GameStructure.Game.ObjectModel;
using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFrameworkPlayMakerExtensions.Actions.AbstractClasses;
using HutongGames.PlayMaker;
using UnityEngine.Assertions;

namespace GameFrameworkPlayMakerExtensions.Actions.GameStructure.AbstractClasses
{
    /// <summary>
    /// Get a counter value into a playmaker variable.
    /// </summary>
    public abstract class GetCounter : EveryFrameAction
    {
        /// <summary>
        /// Name of the variable that you want to copy.
        /// </summary>
        [Tooltip("Name of the variable that you want to copy.")]
        [RequiredField]
        public FsmString CounterName;

        /// <summary>
        /// FSMVariable to which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable to which to copy the variable.")]
        [UIHint(UIHint.Variable)]
        public FsmFloat StoreFloat;

        /// <summary>
        /// FSMVariable to which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable to which to copy the variable.")]
        [UIHint(UIHint.Variable)]
        public FsmInt StoreInt;

        public override void Reset()
        {
            base.Reset();
            CounterName = new FsmString();
            StoreFloat = new FsmFloat();
            StoreInt = new FsmInt();
        }

        /// <summary>
        /// implement this method to return the Counter object to use.
        /// </summary>
        /// <returns></returns>
        public abstract Counter GetCounterReference(string name);

        /// <summary>
        /// The actual method that does the work
        /// </summary>
        protected override void PerformAction()
        {
            var counter = GetCounterReference(CounterName.Value);
            Assert.IsNotNull(counter, "A counter with the Name " + CounterName.Value + " was not found. Please fix!");

            switch (counter.Configuration.CounterType)
            {
                case CounterConfiguration.CounterTypeEnum.Float:
                    if (StoreFloat.IsNone) return;
                    StoreFloat.Value = counter.FloatAmount;
                    break;
                case CounterConfiguration.CounterTypeEnum.Int:
                    if (StoreInt.IsNone) return;
                    StoreInt.Value = counter.IntAmount;
                    break;
            }
        }
    }
}
#endif