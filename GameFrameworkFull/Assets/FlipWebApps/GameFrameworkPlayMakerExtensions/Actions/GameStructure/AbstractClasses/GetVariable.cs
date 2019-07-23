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
using GameFramework.GameStructure.Variables.ObjectModel;
using GameFrameworkPlayMakerExtensions.Actions.AbstractClasses;
using HutongGames.PlayMaker;
using UnityEngine.Assertions;

namespace GameFrameworkPlayMakerExtensions.Actions.GameStructure.AbstractClasses
{
    /// <summary>
    /// Get a variable into a playmaker variable.
    /// </summary>
    public abstract class GetVariable : EveryFrameAction
    {
        /// <summary>
        /// Tag of the variable that you want to copy.
        /// </summary>
        [Tooltip("Tag of the variable that you want to copy.")]
        [RequiredField]
        public FsmString Tag;

        /// <summary>
        /// Type of this variable.
        /// </summary>
        [Tooltip("Type of the variable.")]
        [RequiredField]
        public Variables.VariableType VariableType;

        /// <summary>
        /// FSMVariable to which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable to which to copy the variable.")]
        [UIHint(UIHint.Variable)]
        public FsmBool StoreBool;

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

        /// <summary>
        /// FSMVariable to which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable to which to copy the variable.")]
        [UIHint(UIHint.Variable)]
        public FsmString StoreString;

        /// <summary>
        /// FSMVariable to which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable to which to copy the variable.")]
        [UIHint(UIHint.Variable)]
        public FsmVector2 StoreVector2;

        /// <summary>
        /// FSMVariable to which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable to which to copy the variable.")]
        [UIHint(UIHint.Variable)]
        public FsmVector3 StoreVector3;

        /// <summary>
        /// FSMVariable to which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable to which to copy the variable.")]
        [UIHint(UIHint.Variable)]
        public FsmColor StoreColor;

        public override void Reset()
        {
            base.Reset();
            Tag = "";
            VariableType = Variables.VariableType.Float;
            StoreBool = new FsmBool();
            StoreFloat = new FsmFloat();
            StoreInt = new FsmInt();
            StoreString = new FsmString();
            StoreVector2 = new FsmVector2();
            StoreVector3 = new FsmVector3();
            StoreColor = new FsmColor();
        }

        /// <summary>
        /// implement this method to return the variables object to use.
        /// </summary>
        /// <returns></returns>
        protected abstract Variables GetVariables();

        /// <summary>
        /// The actual method that does the work
        /// </summary>
        protected override void PerformAction()
        {
            //TODO - perhaps cache reference to the actual variable in OnEnable?
            switch (VariableType) {
                case Variables.VariableType.Bool:
                    if (StoreBool.IsNone) return;
                    var variableBool = GetVariables().GetBool(Tag.Value);
                    Assert.IsNotNull(variableBool, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    StoreBool.Value = variableBool.Value;
                    break;
                case Variables.VariableType.Float:
                    if (StoreFloat.IsNone) return;
                    var variableFloat = GetVariables().GetFloat(Tag.Value);
                    Assert.IsNotNull(variableFloat, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    StoreFloat.Value = variableFloat.Value;
                    break;
                case Variables.VariableType.Int:
                    if (StoreInt.IsNone) return;
                    var variable = GetVariables().GetInt(Tag.Value);
                    Assert.IsNotNull(variable, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    StoreInt.Value = variable.Value;
                    break;
                case Variables.VariableType.String:
                    if (StoreString.IsNone) return;
                    var variableString = GetVariables().GetString(Tag.Value);
                    Assert.IsNotNull(variableString, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    StoreString.Value = variableString.Value;
                    break;
                case Variables.VariableType.Vector2:
                    if (StoreVector2.IsNone) return;
                    var variableVector2 = GetVariables().GetVector2(Tag.Value);
                    Assert.IsNotNull(variableVector2, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    StoreVector2.Value = variableVector2.Value;
                    break;
                case Variables.VariableType.Vector3:
                    if (StoreVector3.IsNone) return;
                    var variableVector3 = GetVariables().GetVector3(Tag.Value);
                    Assert.IsNotNull(variableVector3, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    StoreVector3.Value = variableVector3.Value;
                    break;
                case Variables.VariableType.Color:
                    if (StoreColor.IsNone) return;
                    var variableColor = GetVariables().GetColor(Tag.Value);
                    Assert.IsNotNull(variableColor, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    StoreColor.Value = variableColor.Value;
                    break;
            }
        }
    }
}
#endif