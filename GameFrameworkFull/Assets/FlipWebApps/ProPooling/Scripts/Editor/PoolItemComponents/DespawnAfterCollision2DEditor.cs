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
    [CustomEditor(typeof(DespawnAfterCollision2D))]
    public class DespawnAfterCollision2DEditor : UnityEditor.Editor
    {
        SerializedProperty _collisionEnter2D;
        SerializedProperty _collisionStay2D;
        SerializedProperty _collisionExit2D;
        SerializedProperty _triggerEnter2D;
        SerializedProperty _triggerStay2D;
        SerializedProperty _triggerExit2D;
        SerializedProperty _tags;

        DespawnAfterCollision2D _despawnAfterCollision2D;

        void OnEnable()
        {
            _despawnAfterCollision2D = target as DespawnAfterCollision2D;

            _collisionEnter2D = serializedObject.FindProperty("_collisionEnter2D");
            _collisionStay2D = serializedObject.FindProperty("_collisionStay2D");
            _collisionExit2D = serializedObject.FindProperty("_collisionExit2D");
            _triggerEnter2D = serializedObject.FindProperty("_triggerEnter2D");
            _triggerStay2D = serializedObject.FindProperty("_triggerStay2D");
            _triggerExit2D = serializedObject.FindProperty("_triggerExit2D");
            _tags = serializedObject.FindProperty("_tags");
        }


        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("See also the Game Framework asset's Collision Handler component for more advanced despawning with multiple configurable actions.", MessageType.Info);

            // Pull all the information from the target into the serializedObject.
            serializedObject.Update();

            GUILayout.Space(10f);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Despawn on", GUILayout.Width(80));
            EditorGUILayout.PropertyField(_collisionEnter2D, GUIContent.none, GUILayout.Width(16));
            EditorGUILayout.LabelField("CollisionEnter2D", GUILayout.Width(100));
            EditorGUILayout.PropertyField(_collisionStay2D, GUIContent.none, GUILayout.Width(16));
            EditorGUILayout.LabelField("CollisionStay2D", GUILayout.Width(100));
            EditorGUILayout.PropertyField(_collisionExit2D, GUIContent.none, GUILayout.Width(16));
            EditorGUILayout.LabelField("CollisionExit2D", GUILayout.Width(100));

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            GUILayout.Space(84f);
            EditorGUILayout.PropertyField(_triggerEnter2D, GUIContent.none, GUILayout.Width(16));
            EditorGUILayout.LabelField("TriggerEnter2D", GUILayout.Width(100));
            EditorGUILayout.PropertyField(_triggerStay2D, GUIContent.none, GUILayout.Width(16));
            EditorGUILayout.LabelField("TriggerStay2D", GUILayout.Width(100));
            EditorGUILayout.PropertyField(_triggerExit2D, GUIContent.none, GUILayout.Width(16));
            EditorGUILayout.LabelField("TriggerExit2D", GUILayout.Width(100));

            EditorGUILayout.EndHorizontal();

            int mask = 0;
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
            // fallback for int overflow (not likely we have that many tags).
            if (tags.Length > 30)
            {
                EditorGUILayout.PropertyField(_tags, new GUIContent("Collide With Tags"), true);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Collide with tags", GUILayout.Width(120));
                serializedObject.ApplyModifiedProperties();
                Undo.RecordObject(_despawnAfterCollision2D, "tags");

                // set mask
                for (var i = 0; i < tags.Length; i++)
                {
                    if (_despawnAfterCollision2D.Tags.Contains(tags[i]))
                        mask |= (1 << i);
                }

                mask = EditorGUILayout.MaskField(mask, tags);

                // parse mask
                var selectedOptions = new List<string>();
                for (var i = 0; i < tags.Length; i++)
                {
                    if ((mask & (1 << i)) != 0) selectedOptions.Add(tags[i]);
                }
                _despawnAfterCollision2D.Tags = selectedOptions;

                serializedObject.Update();

                EditorGUILayout.EndHorizontal();
            }

            // Push the information back from the serializedObject to the target.
            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(5f);
        }
    }
}