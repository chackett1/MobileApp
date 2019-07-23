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
using UnityEngine;
using HutongGames.PlayMakerEditor;
using GameFramework.GameStructure.Game.ObjectModel;
using System.Collections.Generic;

namespace GameFrameworkPlayMakerExtensions.Actions.GameStructure.AbstractClasses.Editor
{
    /// <summary>
    /// Get a counter into a playmaker variable.
    /// </summary>
    public abstract class GetCounterEditor : CustomActionEditor
    {
        string[] _counters;
        int _counterIndex;
        string counterName;
        GetCounter action;

        public override void OnEnable()
        {
            action = target as GetCounter;

            var counterConfiguration = GetCounterConfiguration();
            _counters = new string[counterConfiguration.Count];
            for (int i = 0; i < counterConfiguration.Count; i++)
            {
                _counters[i] = GetCounterConfiguration()[i].Name;
                if (_counters[i] == action.CounterName.Value)
                    _counterIndex = i;
            }

            base.OnEnable();
        }

        public override bool OnGUI()
        {
            if (action.CounterName.Value.Length == 0)
            {
                action.CounterName = new HutongGames.PlayMaker.FsmString(_counters[0]);
                _counterIndex = 0;
            }

            int newIndex = UnityEditor.EditorGUILayout.Popup("Counter", _counterIndex, _counters);
            if (newIndex != _counterIndex)
            {
                _counterIndex = newIndex;
                action.CounterName = _counters[newIndex];
            }

            switch (GetCounterConfiguration()[_counterIndex].CounterType)
            {
                case CounterConfiguration.CounterTypeEnum.Float:
                    EditField("StoreFloat");
                    break;
                case CounterConfiguration.CounterTypeEnum.Int:
                    EditField("StoreInt");
                    break;
            }
            EditField("EveryFrame");

            return GUI.changed;
        }


        /// <summary>
        /// Override in subclasses to return a list of custom counter configuration entries that should be used
        /// </summary>
        /// <returns></returns>
        public abstract List<CounterConfiguration> GetCounterConfiguration();
    }
}
#endif