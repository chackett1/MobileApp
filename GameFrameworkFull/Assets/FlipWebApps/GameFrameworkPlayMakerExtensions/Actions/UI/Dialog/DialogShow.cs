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
using GameFramework.GameStructure;
using GameFramework.GameStructure.Players;
using GameFramework.UI.Dialogs.Components;
using HutongGames.PlayMaker;
using UnityEngine.Assertions;

namespace GameFrameworkPlayMakerExtensions.Actions.UI.Dialog
{
    /// <summary>
    /// Shows an Dialog.
    /// </summary>
    [ActionCategory("Game Framework")]
    [Tooltip("Shows an Dialog.")]
    public class DialogShow : FsmStateAction
    {
        /// <summary>
        /// The title for the dialog
        /// </summary>
        [Tooltip("The title for the dialog.")]
        public FsmString Title;

        /// <summary>
        /// Whether the Title field is a localisation key
        /// </summary>
        [Tooltip("Whether the Title field is a localisation key.")]
        public FsmBool TitleIsLocalisationKey;

        /// <summary>
        /// The main text for the dialog
        /// </summary>
        [Tooltip("The main text for the dialog.")]
        public FsmString MainText;

        /// <summary>
        /// Whether the Text field is a localisation key
        /// </summary>
        [Tooltip("Whether the Text field is a localisation key.")]
        public FsmBool MainTextIsLocalisationKey;

        /// <summary>
        /// The minor text for the dialog
        /// </summary>
        [Tooltip("The minor text for the dialog.")]
        public FsmString MinorText;

        /// <summary>
        /// Whether the MinorText field is a localisation key
        /// </summary>
        [Tooltip("Whether the MinorText field is a localisation key.")]
        public FsmBool MinorTextIsLocalisationKey;

        /// <summary>
        /// A Sprite to show
        /// </summary>
        [Tooltip("A Sprite to show.")]
        public UnityEngine.Sprite Sprite;

        /// <summary>
        /// The type of buttons to show in the dialog
        /// </summary>
        [Tooltip("The type of buttons to show in the dialog.")]
        public DialogInstance.DialogButtonsType Buttons;

        /// <summary>
        /// FSMVariable from which to copy the result.
        /// </summary>
        [Tooltip("FSM Variable from which to copy the result.")]
        [UIHint(UIHint.Variable)]
        public FsmInt Result;

        /// <summary>
        /// Event to sent when the dialog is closed
        /// </summary>
        [Tooltip("Event to sent when the dialog is closed.")]
        public FsmEvent DialogClosedEvent;

        public override void Reset()
        {
            base.Reset();
            Title = null;
            TitleIsLocalisationKey = false;
            MainText = null;
            MainTextIsLocalisationKey = false;
            MinorText = null;
            MinorTextIsLocalisationKey = false;
            Sprite = null;
            Buttons = DialogInstance.DialogButtonsType.Ok;
            Result = new FsmInt();
            DialogClosedEvent = null;
        }

        public override void OnEnter()
        {
            PerformAction();
            Finish();
        }

        /// <summary>
        /// The actual method that does the work
        /// </summary>
        void PerformAction()
        {
            Assert.IsTrue(GameManager.IsActive, "Ensure that you have a GameManager added to your scene before using the DialogShowError action!");
            Assert.IsTrue(DialogManager.IsActive, "Ensure that you have a DialogManager added to your scene before using the DialogShowError action!");

            DialogManager.Instance.Show(null,
                TitleIsLocalisationKey.Value ? null : Title.Value,
                TitleIsLocalisationKey.Value ? Title.Value : null,
                MainTextIsLocalisationKey.Value ? null : MainText.Value,
                MainTextIsLocalisationKey.Value ? MainText.Value : null,
                MinorTextIsLocalisationKey.Value ? null : MinorText.Value,
                MinorTextIsLocalisationKey.Value ? MinorText.Value : null,
                Sprite, DoneCallback, Buttons);
        }

        void DoneCallback(DialogInstance dialogInstance)
        {
            if (Result != null)
            {
                Result.Value = dialogInstance.DialogResult == DialogInstance.DialogResultType.Custom ?
                    dialogInstance.DialogResultCustom : (int)dialogInstance.DialogResult;
            }

            if (DialogClosedEvent != null)
            {
                Fsm.Event(DialogClosedEvent);
            }
        }
    }
}
#endif