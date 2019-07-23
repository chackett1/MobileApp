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
using ProPooling;
using ProPooling.Components;
using ProPooling.Shared.Editor;
using System.Collections.Generic;

namespace ProPooling.Editor
{
    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : UnityEditor.Editor
    {
        SerializedProperty _automaticallyRun;

        SerializedProperty _spawnOverTime;
        SerializedProperty _delay;
        SerializedProperty _runPeriod;
        SerializedProperty _spawnCount;
        SerializedProperty _spawnTime;
        SerializedProperty _loop;
        SerializedProperty _spawnIntervalMode;
        SerializedProperty _spawnInterval;
        SerializedProperty _minimumSpawnInterval;
        SerializedProperty _maximumSpawnInterval;
        SerializedProperty _spawnBursts;

        SerializedProperty _onSpawnStart;
        SerializedProperty _onSpawnLoop;
        SerializedProperty _onSpawnComplete;
#if GAME_FRAMEWORK
        SerializedProperty _onSpawnStartActionReferences;
        SerializedProperty _onSpawnLoopActionReferences;
        SerializedProperty _onSpawnCompleteActionReferences;
#endif
        SerializedProperty _spawn;
        SerializedProperty _spawnFrom;
        SerializedProperty _spawnPools;
        SerializedProperty _scaleMode;
        SerializedProperty _scale;
        SerializedProperty _useConstantAxisScale;
        SerializedProperty _minimumScaleAllAxis;
        SerializedProperty _maximumScaleAllAxis;
        SerializedProperty _minimumScale;
        SerializedProperty _maximumScale;
        SerializedProperty _rotationMode;
        SerializedProperty _rotation;
        SerializedProperty _minimumRotation;
        SerializedProperty _maximumRotation;
        SerializedProperty _spawnAreaSize;
        SerializedProperty _selectPrefab;
        SerializedProperty _spawnablePrefabs;
        SerializedProperty _locationType;
        SerializedProperty _selectLocation;
        SerializedProperty _spawnLocationPositions;
        SerializedProperty _spawnLocationPositionsAreLocal;
        SerializedProperty _spawnLocationTransforms;


#if GAME_FRAMEWORK
        GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditor[] onStartActionEditors;
        GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditor[] onLoopActionEditors;
        GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditor[] onCompleteActionEditors;
#endif

        Spawner _spawner;

        void OnEnable()
        {
            _spawner = target as Spawner;

            _automaticallyRun = serializedObject.FindProperty("_automaticallyRun");
            _spawnOverTime = serializedObject.FindProperty("_spawnOverTime");
            _delay = _spawnOverTime.FindPropertyRelative("_delay");
            _runPeriod =_spawnOverTime.FindPropertyRelative("_runPeriod");
            _spawnCount =_spawnOverTime.FindPropertyRelative("_spawnCount");
            _spawnTime =_spawnOverTime.FindPropertyRelative("_spawnTime");
            _loop = _spawnOverTime.FindPropertyRelative("_loop");
            _spawnIntervalMode = _spawnOverTime.FindPropertyRelative("_spawnIntervalMode");
            _spawnInterval =_spawnOverTime.FindPropertyRelative("_spawnInterval");
            _minimumSpawnInterval =_spawnOverTime.FindPropertyRelative("_minimumSpawnInterval");
            _maximumSpawnInterval =_spawnOverTime.FindPropertyRelative("_maximumSpawnInterval");
            _spawnBursts = _spawnOverTime.FindPropertyRelative("_spawnBursts");

            _onSpawnStart = _spawnOverTime.FindPropertyRelative("_onSpawnStart");
            _onSpawnLoop = _spawnOverTime.FindPropertyRelative("_onSpawnLoop");
            _onSpawnComplete = _spawnOverTime.FindPropertyRelative("_onSpawnComplete");
#if GAME_FRAMEWORK
            _onSpawnStartActionReferences = _spawnOverTime.FindPropertyRelative("OnSpawnStartActionReferences");
            _onSpawnLoopActionReferences = _spawnOverTime.FindPropertyRelative("OnSpawnLoopActionReferences");
            _onSpawnCompleteActionReferences = _spawnOverTime.FindPropertyRelative("OnSpawnCompleteActionReferences");
#endif
            _spawn = _spawnOverTime.FindPropertyRelative("_spawn");
            _spawnFrom = _spawn.FindPropertyRelative("_spawnFrom");
            _spawnPools = _spawn.FindPropertyRelative("_spawnPools");
            _scaleMode = _spawn.FindPropertyRelative("_scaleMode");
            _scale = _spawn.FindPropertyRelative("_scale");
            _useConstantAxisScale = _spawn.FindPropertyRelative("_useConstantAxisScale");
            _minimumScaleAllAxis = _spawn.FindPropertyRelative("_minimumScaleAllAxis");
            _maximumScaleAllAxis = _spawn.FindPropertyRelative("_maximumScaleAllAxis");
            _minimumScale = _spawn.FindPropertyRelative("_minimumScale");
            _maximumScale = _spawn.FindPropertyRelative("_maximumScale");
            _rotationMode = _spawn.FindPropertyRelative("_rotationMode");
            _rotation = _spawn.FindPropertyRelative("_rotation");
            _minimumRotation = _spawn.FindPropertyRelative("_minimumRotation");
            _maximumRotation = _spawn.FindPropertyRelative("_maximumRotation");
            _spawnAreaSize = _spawn.FindPropertyRelative("_spawnAreaSize");
            _selectPrefab = _spawn.FindPropertyRelative("_selectPrefab");
            _spawnablePrefabs = _spawn.FindPropertyRelative("_spawnablePrefabs");
            _locationType = _spawn.FindPropertyRelative("_locationType");
            _selectLocation = _spawn.FindPropertyRelative("_selectLocation");
            _spawnLocationPositions = _spawn.FindPropertyRelative("_spawnLocationPositions");
            _spawnLocationPositionsAreLocal = _spawn.FindPropertyRelative("_spawnLocationPositionsAreLocal");
            _spawnLocationTransforms = _spawn.FindPropertyRelative("_spawnLocationTransforms");
        }


        protected void OnDisable()
        {
#if GAME_FRAMEWORK
            GameFramework.EditorExtras.Editor.EditorHelper.CleanupSubEditors(onStartActionEditors);
            GameFramework.EditorExtras.Editor.EditorHelper.CleanupSubEditors(onLoopActionEditors);
            GameFramework.EditorExtras.Editor.EditorHelper.CleanupSubEditors(onCompleteActionEditors);
            onStartActionEditors = null;
            onLoopActionEditors = null;
            onCompleteActionEditors = null;
#endif
        }


        public override void OnInspectorGUI()
        {
            // Pull all the information from the target into the serializedObject.
            serializedObject.Update();

            FwaGUI.Space();

            DrawSpawnFrom(_spawnFrom, _spawnPools);

            EditorGUILayout.PropertyField(_automaticallyRun);
            if (_runPeriod.enumValueIndex != 3)
                FwaGUI.PropertyField(_delay, "Start Delay");

            DrawSpawnSetupHeader();
            DrawRunPeriod(_runPeriod, _spawnTime, _spawnCount, _loop);
            if (_runPeriod.enumValueIndex != 3)
            {
                DrawSpawnInterval(_spawnIntervalMode, _spawnInterval, _minimumSpawnInterval, _maximumSpawnInterval);
                DrawBursts(_spawnBursts);
            }

            // prefabs section
            DrawPrefabHeader(_spawnablePrefabs, _selectPrefab);
            Rect addPrefabDropRect = DrawPrefabDragDrop();
            DrawPrefabEntries(_spawnablePrefabs, _selectPrefab);

            DrawLocationHeader();

            FwaGUI.PropertyField(_locationType, "Spawn At");

            if (_locationType.enumValueIndex == 0)
            {
                FwaGUI.Space(); ;
                FwaGUI.PropertyField(_spawnAreaSize);
            }
            else
            {

                SerializedProperty spawnLocationArray = null;
                if (_locationType.enumValueIndex == 1)
                    spawnLocationArray = _spawnLocationPositions;
                else if (_locationType.enumValueIndex == 2)
                    spawnLocationArray = _spawnLocationTransforms;

                if (spawnLocationArray.arraySize > 1)
                    FwaGUI.PropertyField(_selectLocation, "Select");

                if (_locationType.enumValueIndex == 1)
                    FwaGUI.ToggleLeft(_spawnLocationPositionsAreLocal, "Positions are local to Spawner");

                DrawLocationEntries(spawnLocationArray, _locationType.enumValueIndex == 1, _selectLocation);

                if (FwaGUI.ButtonCentered(_locationType.enumValueIndex == 1 ? "Add Position" : "Add Transform"))
                    AddNewLocation(spawnLocationArray);
            }

            DrawTransformHeader();
            DrawScale(_scaleMode, _scale, _useConstantAxisScale, _minimumScaleAllAxis, _maximumScaleAllAxis, _minimumScale, _maximumScale);
            DrawRotation(_rotationMode, _rotation, _minimumRotation, _maximumRotation);

            DrawCallbacks(_onSpawnStart, _onSpawnLoop, _onSpawnComplete);
#if GAME_FRAMEWORK
            DrawGameActions(serializedObject, _spawner.SpawnOverTime, ref onStartActionEditors, ref onLoopActionEditors, ref onCompleteActionEditors, 
                _onSpawnStartActionReferences, _onSpawnLoopActionReferences, _onSpawnCompleteActionReferences);
#else
            EditorGUILayout.HelpBox("Add the FREE Game Framework asset from the asset store and start using Actions. With actions you can play audio, start other spawners and much more when this spawner starts or completes.", MessageType.Info);
#endif

            // process drag and drop
            FwaEditorHelper.CheckPrefabDragAndDrop(addPrefabDropRect, x => AddNewPrefab(_spawnablePrefabs, x));

            // Push the information back from the serializedObject to the target.
            serializedObject.ApplyModifiedProperties();

            if (EditorApplication.isPlaying && GUI.changed)
            {
                _spawner.UpdateCachedValues();
                _spawner.ValidateConfiguration();
            }
        }

        internal static void DrawSpawnFrom(SerializedProperty _spawnFrom, SerializedProperty _spawnPools)
        {
            GUI.enabled = !EditorApplication.isPlaying;
            FwaGUI.Space();
            EditorGUILayout.PropertyField(_spawnFrom);
            if (_spawnFrom.enumValueIndex == 1)
            {
                EditorGUI.indentLevel++;
                FwaGUI.PropertyField(_spawnPools, "Pools");
                EditorGUI.indentLevel--;
            }
            GUI.enabled = true;
        }

        internal static void DrawRunPeriod(SerializedProperty _runPeriod, SerializedProperty _spawnTime, SerializedProperty _spawnCount, SerializedProperty _loop)
        {
            FwaGUI.PropertyField(_runPeriod, "Spawn For");
            if (_runPeriod.enumValueIndex == 1)
            {
                EditorGUI.indentLevel++;
                FwaGUI.PropertyField(_spawnTime, "Seconds");
                if (_loop != null)
                    FwaGUI.PropertyField(_loop, "Loop When Done");
                EditorGUI.indentLevel--;
            }
            else if (_runPeriod.enumValueIndex == 2)
            {
                EditorGUI.indentLevel++;
                FwaGUI.PropertyField(_spawnCount, "Count");
                if (_loop != null)
                    FwaGUI.PropertyField(_loop, "Loop When Done");
                EditorGUI.indentLevel--;
            }
        }

        internal static void DrawSpawnSetupHeader()
        {
            FwaGUI.Space(); ;
            FwaGUI.H2("Setup");
        }

        internal static void DrawSpawnInterval(SerializedProperty _spawnIntervalMode, SerializedProperty _spawnInterval, SerializedProperty _minimumSpawnInterval, SerializedProperty _maximumSpawnInterval)
        {
            FwaGUI.Space(); ;
            FwaGUI.PropertyField(_spawnIntervalMode, "Spawn Interval");
            EditorGUI.indentLevel++;
            if (_spawnIntervalMode.enumValueIndex == 0)
            {
                FwaGUI.PropertyField(_spawnInterval, "Seconds");
            }
            else if (_spawnIntervalMode.enumValueIndex == 1)
            {
                FwaGUI.PropertyField(_minimumSpawnInterval, "Minimum Seconds");
                FwaGUI.PropertyField(_maximumSpawnInterval, "Maximum Seconds");
            }
            EditorGUI.indentLevel--;
        }

        internal static void DrawBursts(SerializedProperty _spawnBursts)
        {
            FwaGUI.Space();
            //EditorGUILayout.BeginHorizontal();
            //GUILayout.FlexibleSpace();
            //EditorGUILayout.EndHorizontal();
            //Rect scale = GUILayoutUtility.GetLastRect();    // workaround for EditorGUIUtility.currentViewWidth not accounting for scroll bars
            //Debug.Log(scale.right + " " + EditorGUIUtility.currentViewWidth + " " + Screen.width);
            EditorGUI.indentLevel++;
            _spawnBursts.isExpanded = FwaGUI.Foldout(_spawnBursts, string.Format("Bursts ({0})", _spawnBursts.arraySize));
            if (_spawnBursts.isExpanded)
            {
                EditorGUI.indentLevel--;
                using (FwaGUI.BeginHorizontal())
                {
                    GUILayout.Space(16);
                    EditorGUILayout.BeginVertical(FwaStyle.BoxLightStyle);
                    if (_spawnBursts.arraySize > 0)
                    {
                        using (FwaGUI.BeginHorizontal())
                        {
                            GUILayout.Label("Time");
                            GUILayout.Label("Min");
                            GUILayout.Label("Max");
                            FwaGUI.Space(16);
                        }

                        for (var i = 0; i < _spawnBursts.arraySize; i++)
                        {
                            var spawnBurstProperty = _spawnBursts.GetArrayElementAtIndex(i);
                            var timeProperty = spawnBurstProperty.FindPropertyRelative("_time");
                            var minimumProperty = spawnBurstProperty.FindPropertyRelative("_minimum");
                            var maximumProperty = spawnBurstProperty.FindPropertyRelative("_maximum");

                            using (FwaGUI.BeginHorizontal())
                            {
                                EditorGUILayout.PropertyField(timeProperty, GUIContent.none);
                                EditorGUILayout.PropertyField(minimumProperty, GUIContent.none);
                                EditorGUILayout.PropertyField(maximumProperty, GUIContent.none);

                                if (FwaGUI.Button(FwaStyle.IconButtonDeleteStyle, 16, 16))
                                {
                                    _spawnBursts.DeleteArrayElementAtIndex(i);
                                }
                            }
                        }
                    }
                    if (FwaGUI.ButtonCentered("Add Burst"))
                        AddNewBurst(_spawnBursts);
                    EditorGUILayout.EndVertical();
                }
                EditorGUI.indentLevel++;
            }
            EditorGUI.indentLevel--;
        }

        internal static void DrawScale(SerializedProperty _scaleMode, SerializedProperty _scale, SerializedProperty _useConstantScale, SerializedProperty _minimumScaleAllAxis, SerializedProperty _maximumScaleAllAxis, SerializedProperty _minimumScale, SerializedProperty _maximumScale)
        {
            FwaGUI.Space(); ;
            FwaGUI.PropertyField(_scaleMode, "Scale");
            EditorGUI.indentLevel++;
            if (_scaleMode.enumValueIndex == 1 || _scaleMode.enumValueIndex == 2)
            {
                FwaGUI.PropertyField(_scale, "");
            }
            else if (_scaleMode.enumValueIndex == 3 || _scaleMode.enumValueIndex == 4)
            {
                FwaGUI.PropertyField(_useConstantScale, "Constant Axis Scale");
                if (_useConstantScale.boolValue)
                {
                    FwaGUI.PropertyField(_minimumScaleAllAxis, "Minimum");
                    FwaGUI.PropertyField(_maximumScaleAllAxis, "Maximum");
                }
                else
                {
                    FwaGUI.PropertyField(_minimumScale, "Minimum");
                    FwaGUI.PropertyField(_maximumScale, "Maximum");
                }
            }
            EditorGUI.indentLevel--;
        }

        internal static void DrawRotation(SerializedProperty _rotationMode, SerializedProperty _rotation, SerializedProperty _minimumRotation, SerializedProperty _maximumRotation)
        {
            FwaGUI.Space(); ;
            FwaGUI.PropertyField(_rotationMode, "Rotation");
            EditorGUI.indentLevel++;
            if (_rotationMode.enumValueIndex == 1)
            {
                FwaGUI.PropertyField(_rotation, "");
            }
            else if (_rotationMode.enumValueIndex == 2)
            {
                FwaGUI.PropertyField(_minimumRotation, "Minimum");
                FwaGUI.PropertyField(_maximumRotation, "Maximum");
            }
            EditorGUI.indentLevel--;
        }

        internal static void DrawPrefabHeader(SerializedProperty spawnablePrefabs, SerializedProperty selectPrefab)
        {
            FwaGUI.Space(); ;
            FwaGUI.H2("Prefabs");
            if (spawnablePrefabs.arraySize > 1)
                FwaGUI.PropertyField(selectPrefab, "Select");
        }

        internal static Rect DrawPrefabDragDrop()
        {
            FwaGUI.Space();
            Rect addPrefabDropRect = new Rect();
            if (!EditorApplication.isPlaying)
            {
                addPrefabDropRect = FwaGUI.Box("Drag a Prefab here to add", 24f, FwaStyle.DropAreaStyle);
                FwaGUI.Space(); ;
            }

            return addPrefabDropRect;
        }

        internal static void DrawPrefabEntries(SerializedProperty spawnablePrefabs, SerializedProperty selectPrefab)
        {
            if (spawnablePrefabs.arraySize > 0)
            {
                FwaGUI.Space(); ;
                for (var i = 0; i < spawnablePrefabs.arraySize; i++)
                {
                    var spawnerPrefabProperty = spawnablePrefabs.GetArrayElementAtIndex(i);
                    var prefabProperty = spawnerPrefabProperty.FindPropertyRelative("_prefab");
                    var weightProperty = spawnerPrefabProperty.FindPropertyRelative("_weight");

                    EditorGUILayout.BeginHorizontal(FwaStyle.BoxLightStyle);
                    EditorGUILayout.PropertyField(prefabProperty, GUIContent.none);
                    if (spawnablePrefabs.arraySize > 1 && selectPrefab.enumValueIndex == 2)
                    {
                        GUILayout.Label("Weight");
                        weightProperty.intValue = EditorGUILayout.IntSlider(weightProperty.intValue, 0, 100);
                    }

                    if (FwaGUI.Button(FwaStyle.IconButtonDeleteStyle, 16, 16))
                        spawnablePrefabs.DeleteArrayElementAtIndex(i);

                    EditorGUILayout.EndHorizontal();
                }
            }

            if (FwaGUI.ButtonCentered("Add Prefab"))
                AddNewPrefab(spawnablePrefabs);
        }

        internal static void DrawLocationHeader()
        {
            FwaGUI.Space(); ;
            FwaGUI.H2("Location");
        }

        internal static void DrawTransformHeader()
        {
            FwaGUI.Space(); ;
            FwaGUI.H2("Transformation");
        }

        static internal void DrawLocationEntries(SerializedProperty spawnLocationArray, bool showPositions, SerializedProperty selectLocation)
        {
            if (spawnLocationArray != null && spawnLocationArray.arraySize > 0)
            {
                FwaGUI.Space();
                for (var i = 0; i < spawnLocationArray.arraySize; i++)
                {
                    var spawnLocationProperty = spawnLocationArray.GetArrayElementAtIndex(i);
                    var weightProperty = spawnLocationProperty.FindPropertyRelative("_weight");
                    var spawnAreaSizeProperty = spawnLocationProperty.FindPropertyRelative("_spawnAreaSize");
                    var deleted = false;

                    EditorGUILayout.BeginVertical(FwaStyle.BoxLightStyle);

                    EditorGUILayout.BeginHorizontal();
                    if (showPositions)
                    {
                        var positionProperty = spawnLocationProperty.FindPropertyRelative("_position");
                        EditorGUILayout.PropertyField(positionProperty, GUIContent.none);
                    }
                    else
                    {
                        var transformProperty = spawnLocationProperty.FindPropertyRelative("_transform");
                        EditorGUILayout.PropertyField(transformProperty, GUIContent.none);

                    }

                    if (spawnLocationArray.arraySize > 1 && selectLocation.enumValueIndex == 2)
                    {
                        GUILayout.Label("Weight");
                        weightProperty.intValue = EditorGUILayout.IntSlider(weightProperty.intValue, 0, 100);
                    }

                    if (FwaGUI.Button(FwaStyle.IconButtonDeleteStyle, 16, 16))
                    {
                        spawnLocationArray.DeleteArrayElementAtIndex(i);
                        deleted = true;
                    }
                    EditorGUILayout.EndHorizontal();

                    if (!deleted)
                    {
                        using (FwaGUI.BeginHorizontal())
                        {
                            EditorGUI.indentLevel++;
                            EditorGUILayout.PropertyField(spawnAreaSizeProperty);
                            EditorGUI.indentLevel--;
                            FwaGUI.Space(16f);
                        }
                    }
                    EditorGUILayout.EndVertical();

                    GUILayout.Space(2f);
                }
            }
        }


        internal static void DrawCallbacks(SerializedProperty onSpawnStart, SerializedProperty onSpawnLoop, SerializedProperty onSpawnComplete)
        {
            FwaGUI.Space(); ;
            FwaGUI.H2("Events & Actions");

            EditorGUI.indentLevel++;
            onSpawnStart.isExpanded = FwaGUI.Foldout(onSpawnStart, "Events");
            EditorGUI.indentLevel--;
            if (onSpawnStart.isExpanded)
            {
                using (FwaGUI.BeginHorizontal())
                {
                    FwaGUI.Space(15);
                    using (FwaGUI.BeginVertical())
                    {
                        FwaGUI.PropertyField(onSpawnStart);
                        FwaGUI.PropertyField(onSpawnLoop);
                        FwaGUI.PropertyField(onSpawnComplete);
                    }
                }
            }
        }


#if GAME_FRAMEWORK
        internal static void DrawGameActions(SerializedObject serializedObject, 
            SpawnOverTime spawnOverTime,
            ref GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditor[] onStartActionEditors,
            ref GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditor[] onLoopActionEditors,
            ref GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditor[] onCompleteActionEditors,
            SerializedProperty onSpawnStartActionReferences, 
            SerializedProperty onSpawnLoopActionReferences, 
            SerializedProperty onSpawnCompleteActionReferences)
        {
            EditorGUI.indentLevel++;
            DrawGameFrameworkActions(serializedObject, onSpawnStartActionReferences, spawnOverTime.OnSpawnStartActionReferences, ref onStartActionEditors, "Start Actions ({0})");
            DrawGameFrameworkActions(serializedObject, onSpawnLoopActionReferences, spawnOverTime.OnSpawnLoopActionReferences, ref onLoopActionEditors,  "Loop Actions ({0})");
            DrawGameFrameworkActions(serializedObject, onSpawnCompleteActionReferences, spawnOverTime.OnSpawnCompleteActionReferences, ref onCompleteActionEditors,  "Complete Actions ({0})");
            EditorGUI.indentLevel--;
        }

        static void DrawGameFrameworkActions(
            SerializedObject serializedObject,
            SerializedProperty actionsProperty,
            GameFramework.GameStructure.Game.ObjectModel.GameActionReference[] actionReferences,
            ref GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditor[] actionEditors, 
            string foldoutName)
        {
            // Start Actions
            actionsProperty.isExpanded = FwaGUI.Foldout(actionsProperty, string.Format(foldoutName, actionReferences.Length));
            if (actionsProperty.isExpanded)
            {
                EditorGUI.indentLevel--;
                using (FwaGUI.BeginHorizontal())
                {
                    GUILayout.Space(15f);
                    using (FwaGUI.BeginVertical())
                    {
                        GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditorHelper.DrawActions(
                            serializedObject, actionsProperty, actionReferences,
                            ref actionEditors, GameFramework.GameStructure.Game.Editor.GameActions.GameActionEditorHelper.GameActionClassDetails,
                            null, tooltip: actionsProperty.tooltip);
                    }
                }
                EditorGUI.indentLevel++;
            }
        }
#endif


        internal static void AddNewPrefab(SerializedProperty spawnablePrefabsArray, GameObject gameobject = null)
        {
            spawnablePrefabsArray.arraySize++;
            var newElement = spawnablePrefabsArray.GetArrayElementAtIndex(spawnablePrefabsArray.arraySize - 1);
            var property = newElement.FindPropertyRelative("_prefab");
            property.objectReferenceValue = gameobject;
            property = newElement.FindPropertyRelative("_weight");
            property.intValue = 100;
        }

        internal static void AddNewLocation(SerializedProperty spawnLocationArray, GameObject gameobject = null)
        {
            spawnLocationArray.arraySize++;
            var newElement = spawnLocationArray.GetArrayElementAtIndex(spawnLocationArray.arraySize - 1);
            var property = newElement.FindPropertyRelative("_weight");
            property.intValue = 100;
            property = newElement.FindPropertyRelative("_spawnAreaSize");
            property.vector3Value = Vector3.zero;

            // only for transform locations
            property = newElement.FindPropertyRelative("_transform");
            if (property != null)
                property.objectReferenceValue = gameobject;

            // only for position locations
            property = newElement.FindPropertyRelative("_position");
            if (property != null)
                property.vector3Value = Vector3.zero;
        }

        internal static void AddNewBurst(SerializedProperty spawnBurstArray, GameObject gameobject = null)
        {
            spawnBurstArray.arraySize++;
            var newElement = spawnBurstArray.GetArrayElementAtIndex(spawnBurstArray.arraySize - 1);
            var property = newElement.FindPropertyRelative("_minimum");
            property.intValue = 10;
            property = newElement.FindPropertyRelative("_maximum");
            property.intValue = 10;
        }
    }
}