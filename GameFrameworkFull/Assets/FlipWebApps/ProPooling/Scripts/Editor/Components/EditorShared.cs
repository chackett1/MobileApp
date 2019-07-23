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
using UnityEngine;
using ProPooling.Shared.Editor;
using System.Collections.Generic;

namespace ProPooling.Editor
{
    internal class EditorShared
    {
        internal static void ShowPoolSummary(Pool pool)
        {
            var poolSummary = "#" + pool.InitialSize;
            if (pool.SizeExceededMode == Pool.SizeExceededModeType.IncreasePoolSize)
            {
                if (pool.HasMaximumSize)
                {
                    poolSummary += "-" + pool.MaximumSize;
                    if (pool.MaximumSizeExceededMode == Pool.MaximumSizeExceededModeType.CreateNonPoolItems)
                        poolSummary += "+";
                }
                else
                {
                    poolSummary += "-∞";
                }
            }
            else if (pool.SizeExceededMode == Pool.SizeExceededModeType.CreateNonPoolItems)
                poolSummary += "+";
            else if (pool.SizeExceededMode == Pool.SizeExceededModeType.ReturnNull)
                poolSummary += "";
            GUILayout.Label(poolSummary, FwaStyle.LabelRight);
        }


        internal static void ShowPool(Pool pool, SerializedProperty poolProperty, bool showPrefab = true, bool inUse = false)
        {
            var prefabProperty = poolProperty.FindPropertyRelative("_prefab");
            var initialSize = poolProperty.FindPropertyRelative("_initialSize");
            var progressiveFill = poolProperty.FindPropertyRelative("_progressiveFill");
            var progressiveFillInitialAmount = poolProperty.FindPropertyRelative("_progressiveFillInitialAmount");
            var progressiveFillFrameInterval = poolProperty.FindPropertyRelative("_progressiveFillFrameInterval");
            var progressiveFillIntervalAmount = poolProperty.FindPropertyRelative("_progressiveFillIntervalAmount");
            var sizeExceededMode = poolProperty.FindPropertyRelative("_sizeExceededMode");
            var hasMaximumSize = poolProperty.FindPropertyRelative("_hasMaximumSize");
            var maximumSize = poolProperty.FindPropertyRelative("_maximumSize");
            var maximumSizeExceededMode = poolProperty.FindPropertyRelative("_maximumSizeExceededMode");

            GUI.enabled = !inUse;
            if (showPrefab)
            {
                using (FwaGUI.BeginHorizontal()) {

                    Texture prefabPreview = null;
                    if (!EditorApplication.isPlaying && pool.Prefab != null)
                    {
                        prefabPreview = AssetPreview.GetAssetPreview(pool.Prefab);
                        // TODO - Don't loop forever as it can lock the editor
                        //while (AssetPreview.IsLoadingAssetPreview(pool.Prefab.GetInstanceID()))
                        //{
                        //    System.Threading.Thread.Sleep(5);
                        //}
                        //prefabPreview = AssetPreview.GetAssetPreview(pool.Prefab);
                    }

                    if (prefabPreview != null)
                    {
                        GUILayout.Space(16);
                        var rect = GUILayoutUtility.GetRect(40f, 40f, 40f, 40f, GUILayout.ExpandWidth(false));
                        GUI.DrawTexture(rect, prefabPreview);
                        EditorGUI.indentLevel--;
                        EditorGUIUtility.labelWidth -= 50;
                    }

                    using (FwaGUI.BeginVertical())
                    {
                        EditorGUILayout.PropertyField(prefabProperty, GUILayout.ExpandWidth(true));
                        EditorGUILayout.PropertyField(initialSize);
                    }

                    if (prefabPreview != null)
                    {
                        EditorGUIUtility.labelWidth += 50;
                        EditorGUI.indentLevel++;
                    }
                }
            }
            else
            {
                FwaGUI.PropertyField(initialSize);
            }

            GUI.enabled = true;
            FwaGUI.PropertyField(sizeExceededMode, "If Size Exceeded");
            if (sizeExceededMode.enumValueIndex == 0)
            {
                using (FwaGUI.BeginHorizontal(245))
                {
                    GUILayout.Space(4 + 16);
                    
                    if (FwaGUI.ToggleLeft(hasMaximumSize, 185, "Limit the max. pool size " + (hasMaximumSize.boolValue ? "to" : "")))
                    {
                        var oldLabelWidth = EditorGUIUtility.labelWidth;
                        var oldIndent = EditorGUI.indentLevel;
                        EditorGUIUtility.labelWidth = 0;
                        EditorGUI.indentLevel = 0;
                        FwaGUI.IntField(maximumSize, 40);
                        maximumSize.intValue = Mathf.Max(maximumSize.intValue, initialSize.intValue);
                        EditorGUI.indentLevel = oldIndent;
                        EditorGUIUtility.labelWidth = oldLabelWidth;
                    }
                }

                if (hasMaximumSize.boolValue)
                {
                    using (FwaGUI.BeginHorizontal())
                    {
                        EditorGUI.indentLevel++;
                        //GUILayout.Space(4 + 16);
                        FwaGUI.PropertyField(maximumSizeExceededMode, "If max. exceeded");
                        EditorGUI.indentLevel--;
                        //FwaGUI.ToggleLeft(createIfMaximumExceeded, "Create non pool items if limits exceeded");
                    }
                }
            }

            FwaGUI.PropertyField(progressiveFill, "Progressively Fill");
            if (progressiveFill.boolValue)
            {
                EditorGUI.indentLevel++;
                FwaGUI.PropertyField(progressiveFillInitialAmount, "Initial Amount");
                FwaGUI.PropertyField(progressiveFillIntervalAmount, "Subsequent Amount");
                FwaGUI.PropertyField(progressiveFillFrameInterval, "Interval");
                EditorGUI.indentLevel--;
            }
        }


        internal static void AddNewPoolToList(GameObject gameobject, List<Pool> pools, SerializedProperty poolsProperty, Pool defaultPoolSettings)
        {
            if (gameobject != null)
            {
                foreach (var pool in pools)
                {
                    if (pool.Prefab && pool.Prefab.name == gameobject.name)
                    {
                        EditorUtility.DisplayDialog("Pro Pooling",
                            string.Format(
                                "GlobalPools pools need to have unique names. There is already a pool named {0}.\n\nRename the prefab and try again!",
                                gameobject.name), "OK");
                        return;
                    }
                }
            }

            poolsProperty.arraySize++;
            var newElement =
                poolsProperty.GetArrayElementAtIndex(poolsProperty.arraySize - 1);
            newElement.isExpanded = true;
            var property = newElement.FindPropertyRelative("_prefab");
            property.objectReferenceValue = gameobject;
            property = newElement.FindPropertyRelative("_initialSize");
            property.intValue = defaultPoolSettings.InitialSize;
            property = newElement.FindPropertyRelative("_sizeExceededMode");
            property.enumValueIndex = (int)defaultPoolSettings.SizeExceededMode;
            property = newElement.FindPropertyRelative("_hasMaximumSize");
            property.boolValue = defaultPoolSettings.HasMaximumSize;
            property = newElement.FindPropertyRelative("_maximumSize");
            property.intValue = defaultPoolSettings.MaximumSize;
            property = newElement.FindPropertyRelative("_maximumSizeExceededMode");
            property.enumValueIndex = (int)defaultPoolSettings.MaximumSizeExceededMode;
        }

    }
}