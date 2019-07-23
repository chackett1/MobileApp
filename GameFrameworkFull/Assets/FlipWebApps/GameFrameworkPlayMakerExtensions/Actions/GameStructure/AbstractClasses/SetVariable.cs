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
    /// Set a variable from a playmaker variable.
    /// </summary>
    public abstract class SetVariable : EveryFrameAction
    {
        /// <summary>
        /// Tag of the variable that you want to copy to.
        /// </summary>
        [Tooltip("Tag of the variable that you want to copy to.")]
        [RequiredField]
        public FsmString Tag;

        /// <summary>
        /// Type of this variable.
        /// </summary>
        [Tooltip("Type of the variable.")]
        [RequiredField]
        public Variables.VariableType VariableType;

        /// <summary>
        /// FSMVariable from which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable from which to copy the variable.")]
        public FsmBool SetBool;

        /// <summary>
        /// FSMVariable from which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable from which to copy the variable.")]
        public FsmFloat SetFloat;

        /// <summary>
        /// FSMVariable from which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable from which to copy the variable.")]
        public FsmInt SetInt;

        /// <summary>
        /// FSMVariable from which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable from which to copy the variable.")]
        public FsmString SetString;

        /// <summary>
        /// FSMVariable from which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable from which to copy the variable.")]
        public FsmVector2 SetVector2;

        /// <summary>
        /// FSMVariable from which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable from which to copy the variable.")]
        public FsmVector3 SetVector3;

        /// <summary>
        /// FSMVariable from which to copy the variable.
        /// </summary>
        [Tooltip("FSM Variable from which to copy the variable.")]
        public FsmColor SetColor;

        public override void Reset()
        {
            base.Reset();
            Tag = "";
            VariableType = Variables.VariableType.Float;
            SetBool = new FsmBool();
            SetFloat = new FsmFloat();
            SetInt = new FsmInt();
            SetString = new FsmString();
            SetVector2 = new FsmVector2();
            SetVector3 = new FsmVector3();
            SetColor = new FsmColor();
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
                    if (SetBool.IsNone) return;
                    var variableBool = GetVariables().GetBool(Tag.Value);
                    Assert.IsNotNull(variableBool, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    variableBool.Value = SetBool.Value;
                    break;
                case Variables.VariableType.Float:
                    if (SetFloat.IsNone) return;
                    var variableFloat = GetVariables().GetFloat(Tag.Value);
                    Assert.IsNotNull(variableFloat, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    variableFloat.Value = SetFloat.Value;
                    break;
                case Variables.VariableType.Int:
                    if (SetInt.IsNone) return;
                    var variable = GetVariables().GetInt(Tag.Value);
                    Assert.IsNotNull(variable, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    variable.Value = SetInt.Value;
                    break;
                case Variables.VariableType.String:
                    if (SetString.IsNone) return;
                    var variableString = GetVariables().GetString(Tag.Value);
                    Assert.IsNotNull(variableString, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    variableString.Value = SetString.Value;
                    break;
                case Variables.VariableType.Vector2:
                    if (SetVector2.IsNone) return;
                    var variableVector2 = GetVariables().GetVector2(Tag.Value);
                    Assert.IsNotNull(variableVector2, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    variableVector2.Value = SetVector2.Value;
                    break;
                case Variables.VariableType.Vector3:
                    if (SetVector3.IsNone) return;
                    var variableVector3 = GetVariables().GetVector3(Tag.Value);
                    Assert.IsNotNull(variableVector3, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    variableVector3.Value = SetVector3.Value;
                    break;
                case Variables.VariableType.Color:
                    if (SetColor.IsNone) return;
                    var variableColor = GetVariables().GetColor(Tag.Value);
                    Assert.IsNotNull(variableColor, "A variable with the Tag " + Tag.Value + " was not found. Please fix!");
                    variableColor.Value = SetColor.Value;
                    break;
            }
        }
    }
}
#endif