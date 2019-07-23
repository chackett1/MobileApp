//----------------------------------------------
// Flip Web Apps: Game Framework
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
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using ProPooling.Shared.Editor;
using ProPooling.Components;

namespace ProPooling.Editor.PoolItemComponents
{
    [CustomEditor(typeof(OnPoolItemLifecycleActions))]
    public class OnPoolItemLifecycleActionsEditor : UnityEditor.Editor
    {
#if GAME_FRAMEWORK
        SerializedProperty _onSpawnedActions;
        SerializedProperty _onDespawnedActions;

        List<GameFramework.Helper.ClassDetailsAttribute> _gameActionClassDetails;
        GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditor[] _onSpawnedActionsEditors;
        GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditor[] _onDespawnedActionsEditors;

        OnPoolItemLifecycleActions _onPoolItemLifecycleActions;
#endif

        void OnEnable()
        {
#if GAME_FRAMEWORK
            _onPoolItemLifecycleActions = target as OnPoolItemLifecycleActions;

            _onSpawnedActions = serializedObject.FindProperty("OnSpawnedActions");
            _onDespawnedActions = serializedObject.FindProperty("OnDespawnedActions");

            // setup actions types
            _gameActionClassDetails = GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditorHelper.FindTypesClassDetails();
#endif
        }

        protected void OnDisable()
        {
#if GAME_FRAMEWORK
            GameFramework.EditorExtras.Editor.EditorHelper.CleanupSubEditors(_onSpawnedActionsEditors);
            GameFramework.EditorExtras.Editor.EditorHelper.CleanupSubEditors(_onDespawnedActionsEditors);
            _onSpawnedActionsEditors = null;
            _onDespawnedActionsEditors = null;
#endif
        }

        public override void OnInspectorGUI()
        {
            // Pull all the information from the target into the serializedObject.
            serializedObject.Update();

            FwaGUI.Space(10f);

#if GAME_FRAMEWORK
            EditorGUI.indentLevel++;
            // Spawn Actions
            if (FwaGUI.Foldout(_onSpawnedActions, "On Spawn Actions (" + _onPoolItemLifecycleActions.OnSpawnedActions.Length + ")"))
            {
                EditorGUI.indentLevel--;
                using (FwaGUI.BeginHorizontal())
                {
                    FwaGUI.Space(15);
                    EditorGUILayout.BeginVertical();
                    GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditorHelper.DrawActions(serializedObject, _onSpawnedActions, _onPoolItemLifecycleActions.OnSpawnedActions,
                        ref _onSpawnedActionsEditors, _gameActionClassDetails, null, tooltip: _onSpawnedActions.tooltip);
                    EditorGUILayout.EndVertical();
                }
                EditorGUI.indentLevel++;
            }

            // Despawn Actions
            if (FwaGUI.Foldout(_onDespawnedActions, "On Despawn Actions (" + _onPoolItemLifecycleActions.OnDespawnedActions.Length + ")"))
            {
                EditorGUI.indentLevel--;
                using (FwaGUI.BeginHorizontal())
                {
                    FwaGUI.Space(15);
                    EditorGUILayout.BeginVertical();
                    GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditorHelper.DrawActions(serializedObject, _onDespawnedActions, _onPoolItemLifecycleActions.OnDespawnedActions,
                        ref _onDespawnedActionsEditors, _gameActionClassDetails, null, tooltip: _onDespawnedActions.tooltip);
                    EditorGUILayout.EndVertical();
                }
                EditorGUI.indentLevel++;
            }
            EditorGUI.indentLevel--;
#else
            EditorGUILayout.HelpBox("Add the FREE Game Framework asset from the asset store and start using Actions. With actions you can play audio, start transitions and much more when this pool item is spawned or despawned.", MessageType.Info);
#endif

            // Push the information back from the serializedObject to the target.
            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(5f);
        }
    }
}