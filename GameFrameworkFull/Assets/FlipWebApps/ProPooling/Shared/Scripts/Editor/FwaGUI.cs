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
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Internal shared functions - Game Framework is Master
namespace ProPooling.Shared.Editor
{
    /// <summary>
    /// Helper functions for dealing with gui...
    /// </summary>
    internal static class FwaGUI
    {

        #region BeginHorizontal

        /// <summary>
        /// Begin a horizontal group.
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static BeginHorizontalDisposable BeginHorizontal()
        {
            return new BeginHorizontalDisposable();
        }

        /// <summary>
        /// Begin a horizontal group with the given width.
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static BeginHorizontalDisposable BeginHorizontal(int width)
        {
            return new BeginHorizontalDisposable(width);
        }

        public class BeginHorizontalDisposable : System.IDisposable
        {
            public Rect Rect;
            public BeginHorizontalDisposable()
            {
                Rect = EditorGUILayout.BeginHorizontal();
            }
            public BeginHorizontalDisposable(int width)
            {
                Rect = EditorGUILayout.BeginHorizontal(GUILayout.Width(width));
            }

            public void Dispose()
            {
                EditorGUILayout.EndHorizontal();
                System.GC.SuppressFinalize(this);
            }
        }
        #endregion BeginHorizontal

        #region BeginVertical

        /// <summary>
        /// Begin a horizontal group.
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static BeginVerticalDisposable BeginVertical()
        {
            return new BeginVerticalDisposable();
        }

        /// <summary>
        /// Begin a horizontal group with the given width.
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static BeginVerticalDisposable BeginVertical(int width)
        {
            return new BeginVerticalDisposable(width);
        }

        public class BeginVerticalDisposable : System.IDisposable
        {
            public Rect Rect;
            public BeginVerticalDisposable()
            {
                Rect = EditorGUILayout.BeginVertical();
            }
            public BeginVerticalDisposable(int width)
            {
                Rect = EditorGUILayout.BeginVertical(GUILayout.Width(width));
            }

            public void Dispose()
            {
                EditorGUILayout.EndVertical();
                System.GC.SuppressFinalize(this);
            }
        }
        #endregion BeginVertical

        #region Box
        /// <summary>
        /// Show a box with the given style and size.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Rect Box(string label, float height, GUIStyle style)
        {
            Rect rect = GUILayoutUtility.GetRect(0f, height, GUILayout.ExpandWidth(true));
            GUI.Box(rect, label, style);
            return rect;
        }

        #endregion Box

        #region Button
        /// <summary>
        /// Show a button with the given style and size.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool Button(GUIStyle style)
        {
            return GUILayout.Button(GUIContent.none, style);
        }

        /// <summary>
        /// Show a button with the given style and size.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool Button(GUIStyle style, float width, float height) {
            return GUILayout.Button(GUIContent.none, style, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// Show a button with the given style and size.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool ButtonCentered(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var clicked = GUILayout.Button(new GUIContent(text), GUILayout.ExpandWidth(false));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return clicked;
        }
        #endregion Button

        #region Foldout
        /// <summary>
        /// Show a button with the given style and size.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool Foldout(SerializedProperty property, string title = null)
        {
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, title == null ? property.name : title);
            return property.isExpanded;
        }
        #endregion Foldout

        #region IntField
        /// <summary>
        /// Show a button with the given style and size.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool IntField(SerializedProperty property, string title = null)
        {
            property.intValue = EditorGUILayout.IntField(property.intValue, title == null ? property.name : title);
            return property.isExpanded;
        }

        /// <summary>
        /// Show a button with the given style and size.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool IntField(SerializedProperty property, int width)
        {
            property.intValue = EditorGUILayout.IntField(property.intValue, GUILayout.Width(width));
            return property.isExpanded;
        }
        #endregion IntField

        #region Label
        /// <summary>
        /// Show a H2 label.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static void H2(string text)
        {
            GUILayout.Label(text, FwaStyle.H2);
        }
        #endregion Label

        #region Popup
        /// <summary>
        /// Show a popup with the given style and size.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static int Popup(SerializedProperty property, string[] displayOptions, string label = null, string tooltip = null)
        {
            property.enumValueIndex = EditorGUILayout.Popup(label == null ? property.name : label, property.enumValueIndex, displayOptions);
            return property.enumValueIndex;
        }
        #endregion

        #region PropertyField
        /// <summary>
        /// Show a property field with the given label and tooltip
        /// </summary>
        /// <param name="property"></param>
        /// <param name="title"></param>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        public static bool PropertyField(SerializedProperty property)
        {
            return EditorGUILayout.PropertyField(property);
        }

        /// <summary>
        /// Show a property field with the given label and tooltip
        /// </summary>
        /// <param name="property"></param>
        /// <param name="title"></param>
        /// <param name="tooltip"></param>
        /// <returns></returns>
        public static bool PropertyField(SerializedProperty property, string title, string tooltip = null)
        {
            return EditorGUILayout.PropertyField(property, new GUIContent(title, tooltip == null ? property.tooltip : tooltip));
        }
        #endregion PropertyField

        #region Seperator
        /// <summary>
        /// Show a seperator covering the full width with spacing before and after.
        /// </summary>
        /// <returns></returns>
        public static Rect Seperator()
        {
            GUILayout.Space(5);
            var rect = GUILayoutUtility.GetRect(0f, 2f, GUILayout.ExpandWidth(true));
            GUI.Box(rect, GUIContent.none, FwaStyle.SeperatorStyle);
            GUILayout.Space(5);
            return rect;
        }

        #endregion Seperator

        #region Space
        /// <summary>
        /// Show a seperator covering the full width with spacing before and after.
        /// </summary>
        /// <returns></returns>
        public static void Space(float pixels = 5)
        {
            GUILayout.Space(pixels);
        }

        #endregion Space

        #region ToggleLeft
        /// <summary>
        /// Show a toggle left.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool ToggleLeft(SerializedProperty property, string label = null, string tooltip = null)
        {
            property.boolValue = EditorGUILayout.ToggleLeft(new GUIContent(label == null ? property.name : label, tooltip == null ? property.tooltip : tooltip), property.boolValue);
            return property.boolValue;
        }

        /// <summary>
        /// Show a toggle left.
        /// </summary>
        /// <param name="style"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool ToggleLeft(SerializedProperty property, int width, string label = null, string tooltip = null)
        {
            property.boolValue = EditorGUILayout.ToggleLeft(new GUIContent(label == null ? property.name : label, tooltip == null ? property.tooltip : tooltip), property.boolValue, GUILayout.Width(width));
            return property.boolValue;
        }
        #endregion ToggleLeft

        /// <summary>
        /// Helper function to return whether an enum value equals one of the values contained in the specified list of items.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool EnumValueEquals(SerializedProperty property, params int[] list)
        {
            foreach (var i in list)
                if (property.enumValueIndex == i)
                    return true;
            return false;
        }
    }
}