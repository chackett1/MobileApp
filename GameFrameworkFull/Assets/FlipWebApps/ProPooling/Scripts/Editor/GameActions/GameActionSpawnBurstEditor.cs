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

#if GAME_FRAMEWORK
using GameFramework.EditorExtras.Editor;
using GameFramework.GameStructure.Game.Editor.GameActions;
using ProPooling.GameActions;
using ProPooling.Shared.Editor;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace ProPooling.Editor.GameActions
{

    [CustomEditor(typeof(GameActionSpawnBurst))]
    public class GameActionSpawnBurstEditor : GameActionEditor
    {
        // names for possible targets with / without ColliderOptions set.
        public static readonly string[] PositionNames = { "This GameObject", "Positions", "Transforms" };
        public static readonly string[] PositionNamesCollider = { "This GameObject", "Colliding GameObject", "Positions", "Transforms" };
        // names for positions coordinate space with / without ColliderOptions set.
        public static readonly string[] PositionsAreNames = { "Global", "Local to This GameObject" };
        public static readonly string[] PositionsAreNamesCollider = { "Global", "Local to This GameObject", "Local to Colliding GameObject" };

        SerializedProperty _burstMinimum;
        SerializedProperty _burstMaximum;
        SerializedProperty _locationType;
        SerializedProperty _locationCoordinateSpace;

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
        SerializedProperty _selectLocation;
        SerializedProperty _spawnLocationPositions;
        SerializedProperty _spawnLocationTransforms;

        GameActionSpawnBurst _gameActionSpawnBurst;

        protected override void Initialise() {
            _gameActionSpawnBurst = target as GameActionSpawnBurst;

            _burstMinimum = serializedObject.FindProperty("_burstMinimum");
            _burstMaximum = serializedObject.FindProperty("_burstMaximum");
            _locationType = serializedObject.FindProperty("_locationType");
            _locationCoordinateSpace = serializedObject.FindProperty("_locationCoordinateSpace");

            _spawn = serializedObject.FindProperty("_spawn");
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
            _selectLocation = _spawn.FindPropertyRelative("_selectLocation");
            _spawnLocationPositions = _spawn.FindPropertyRelative("_spawnLocationPositions");
            _spawnLocationTransforms = _spawn.FindPropertyRelative("_spawnLocationTransforms");
        }

        /// <summary>
        /// Draw the Editor GUI
        /// </summary>
        protected override void DrawGUI()
        {
            // Pull all the information from the target into the serializedObject.
            serializedObject.Update();

            EditorHelper.DrawProperties(serializedObject, new List<string>() { "_delay" });
            SpawnerEditor.DrawSpawnFrom(_spawnFrom, _spawnPools);

            using (FwaGUI.BeginHorizontal())
            {
                FwaGUI.Space();
                EditorGUILayout.PrefixLabel("Spawn Count");
                var oldLabelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 25;
                FwaGUI.PropertyField(_burstMinimum, "Min");
                FwaGUI.PropertyField(_burstMaximum, "Max");
                EditorGUIUtility.labelWidth = oldLabelWidth;
            }

            // prefabs section
            SpawnerEditor.DrawPrefabHeader(_spawnablePrefabs, _selectPrefab);
            //Rect addPrefabDropRect = SpawnerEditor.DrawPrefabDragDrop();
            SpawnerEditor.DrawPrefabEntries(_spawnablePrefabs, _selectPrefab);

            // location section
            SpawnerEditor.DrawLocationHeader();

            if (IsColliderEditor())
            {
                _locationType.enumValueIndex = EditorGUILayout.Popup("Spawn At", _locationType.enumValueIndex, PositionNamesCollider);
            }
            else
            {
                var index = _locationType.enumValueIndex == 0 ? 0 : _locationType.enumValueIndex == 2 ? 1 : 2; // map
                index = EditorGUILayout.Popup("Spawn At", index, PositionNames);
                _locationType.enumValueIndex = index == 0 ? 0 : index == 1 ? 2 : 3; // unmap
            }

            if (FwaGUI.EnumValueEquals(_locationType, 0, 1))
            {
                FwaGUI.Space(); ;
                FwaGUI.PropertyField(_spawnAreaSize);
            }
            else
            {
                SerializedProperty spawnLocationArray = null;
                if (_locationType.enumValueIndex == 2)
                    spawnLocationArray = _spawnLocationPositions;
                else
                    spawnLocationArray = _spawnLocationTransforms;

                if (spawnLocationArray.arraySize > 1)
                    FwaGUI.PropertyField(_selectLocation, "Select");

                if (_locationType.enumValueIndex == 2)
                    _locationCoordinateSpace.enumValueIndex = EditorGUILayout.Popup("Positions Are", _locationCoordinateSpace.enumValueIndex, IsColliderEditor() ? PositionsAreNamesCollider : PositionsAreNames);

                SpawnerEditor.DrawLocationEntries(spawnLocationArray, _locationType.enumValueIndex == 2, _selectLocation);

                if (FwaGUI.ButtonCentered(_locationType.enumValueIndex == 2 ? "Add Position" : "Add Transform"))
                    SpawnerEditor.AddNewLocation(spawnLocationArray);
            }

            SpawnerEditor.DrawTransformHeader();
            SpawnerEditor.DrawScale(_scaleMode, _scale, _useConstantAxisScale, _minimumScaleAllAxis, _maximumScaleAllAxis, _minimumScale, _maximumScale);
            SpawnerEditor.DrawRotation(_rotationMode, _rotation, _minimumRotation, _maximumRotation);

            // process drag and drop
            //FwaEditorHelper.CheckPrefabDragAndDrop(addPrefabDropRect, x => SpawnerEditor.AddNewPrefab(_spawnablePrefabs, x));

            // Push the information back from the serializedObject to the target.
            serializedObject.ApplyModifiedProperties();

            if (EditorApplication.isPlaying && GUI.changed)
            {
                _gameActionSpawnBurst.UpdateCachedValues();
                _gameActionSpawnBurst.ValidateConfiguration();
            }
        }
    }
}
#endif