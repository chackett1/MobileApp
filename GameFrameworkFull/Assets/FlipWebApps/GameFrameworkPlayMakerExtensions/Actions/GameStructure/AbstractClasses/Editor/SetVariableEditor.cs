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
using GameFramework.GameStructure.Variables.ObjectModel;
using HutongGames.PlayMakerEditor;

namespace GameFrameworkPlayMakerExtensions.Actions.GameStructure.AbstractClasses.Editor
{
    /// <summary>
    /// Set a variable from a playmaker variable.
    /// </summary>
    public class SetVariableEditor : CustomActionEditor
    {
        public override bool OnGUI()
        {
            var action = target as SetVariable;

            EditField("VariableType");
            EditField("Tag");
            switch (action.VariableType)
            {
                case Variables.VariableType.Bool:
                    EditField("SetBool");
                    break;
                case Variables.VariableType.Float:
                    EditField("SetFloat");
                    break;
                case Variables.VariableType.Int:
                    EditField("SetInt");
                    break;
                case Variables.VariableType.String:
                    EditField("SetString");
                    break;
                case Variables.VariableType.Vector2:
                    EditField("SetVector2");
                    break;
                case Variables.VariableType.Vector3:
                    EditField("SetVector3");
                    break;
                case Variables.VariableType.Color:
                    EditField("SetColor");
                    break;
            }
            EditField("EveryFrame");

            return GUI.changed;
        }
    }
}
#endif