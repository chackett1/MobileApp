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

using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using ProPooling.Shared.Editor;
using ProPooling;

namespace ProPooling.Editor
{
    public abstract class PoolsBaseEditor : UnityEditor.Editor
    {
        SerializedProperty _defaultPoolSettingsProperty;
        SerializedProperty _poolsProperty;

        PoolsBase _poolsBase;

        bool _showDefaultPoolSettings;

        protected virtual void OnEnable()
        {
            _poolsBase = target as PoolsBase;
            _defaultPoolSettingsProperty = serializedObject.FindProperty("_defaultPoolSettings");
            _poolsProperty = serializedObject.FindProperty("_pools");
        }


        public override void OnInspectorGUI()
        {
            // Pull all the information from the target into the serializedObject.
            serializedObject.Update();

            FwaGUI.Space(10f);

            ShowHeading();

            Rect addPoolDropRect = new Rect();
            if (!EditorApplication.isPlaying)
            {
                addPoolDropRect = FwaGUI.Box("Drag a Prefab here to add a pool", 40, FwaStyle.DropAreaStyle);
                FwaGUI.Space();
            }

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical("Box");
            _showDefaultPoolSettings = EditorGUILayout.Foldout(_showDefaultPoolSettings, new GUIContent("Default Pool Settings", "Default settings for new Pools and Pools that are automatically created"));
            if (_showDefaultPoolSettings)
            {
                EditorShared.ShowPool(null, _defaultPoolSettingsProperty, false);
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            FwaGUI.Seperator();

            if (_poolsProperty.arraySize > 0)
            {
                //GUILayout.Label("Pools", EditorStyles.boldLabel);

                for (var i = 0; i < _poolsProperty.arraySize; i++)
                {
                    var pool = _poolsBase.Pools[i];
                    var poolProperty = _poolsProperty.GetArrayElementAtIndex(i);
                    var prefabProperty = poolProperty.FindPropertyRelative("_prefab");
                    var deleted = false;
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginVertical("Box");

                    using (FwaGUI.BeginHorizontal())
                    {
                        var title = prefabProperty.objectReferenceValue == null ? "<missing prefab>" : prefabProperty.objectReferenceValue.name;
                        FwaGUI.Foldout(poolProperty, title);

                        EditorShared.ShowPoolSummary(pool);

                        if (!EditorApplication.isPlaying)
                        {
                            if (FwaGUI.Button(FwaStyle.IconButtonDeleteStyle, 16, 16) &&
                                EditorUtility.DisplayDialog("Delete Pool?", "Are you sure you want to delete this pool?", "Yes",
                                "No"))
                            {
                                _poolsProperty.DeleteArrayElementAtIndex(i);
                                deleted = true;
                            }
                        }
                    }

                    if (!deleted && poolProperty.isExpanded)
                    {
                        EditorShared.ShowPool(pool, poolProperty, true, EditorApplication.isPlaying);
                    }

                    if (EditorApplication.isPlaying)
                    {
                        if (pool.AddedToGlobalPools)
                            GUILayout.Label(string.Format("{0} In Use | {1} Available | Total = {2} | Most = {3}", pool.SpawnedCount, pool.InactiveCount, pool.Count, pool.SpawnedAndNonPoolCountMaximum), FwaStyle.CenteredInfoLabelStyle);
                    }

                    EditorGUILayout.EndVertical();
                    EditorGUI.indentLevel--;
                    GUILayout.Space(5f);

                }
            }

            if (!EditorApplication.isPlaying)
            {
                if (FwaGUI.ButtonCentered("Add Pool"))
                    AddNewPool();
            }

            // process drag and drop
            if (!EditorApplication.isPlaying)
                FwaEditorHelper.CheckPrefabDragAndDrop(addPoolDropRect, AddNewPool);


            // Push the information back from the serializedObject to the target.
            serializedObject.ApplyModifiedProperties();
        }


        void AddNewPool(GameObject gameobject = null)
        {
            EditorShared.AddNewPoolToList(gameobject, _poolsBase.Pools, _poolsProperty, _poolsBase.DefaultPoolSettings);
        }

        /// <summary>
        /// Implement this to show any custom heading.
        /// </summary>
        protected virtual void ShowHeading() { }
    }
}