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
    /// Helper functions for dealing with editor windows, inspectors etc...
    /// </summary>
    internal static class FwaStyle
    {
        internal enum StyleType
        {
            ButtonBorderless,
            IconButtonDelete,
            LabelCentred,
            LabelRight,
            LabelItalic,
            LabelBold,
            H1,
            H2,
            SeperatorStyle,
            DropAreaStyle,
            BoxLightStyle,
            Custom = 9999
        }

        static readonly Texture2D _deleteIcon = AssetDatabase.LoadAssetAtPath(@"Assets/FlipWebApps/ProPooling/Shared/Textures/Icons/delete.png", typeof(Texture2D)) as Texture2D;

        public static readonly GUIStyle ToolbarSearchField = "ToolbarSeachTextField";
        public static readonly GUIStyle ToolbarSearchFieldCancel = "ToolbarSeachCancelButton";
        public static readonly GUIStyle ToolbarSearchFieldCancelEmpty = "ToolbarSeachCancelButtonEmpty";

        public const float RemoveButtonWidth = 30f;

        // Store in list to allow for easy static (one time) setup of all styles.
        public static List<GUIStyle> Styles {
            get {
                if (_styles == null)
                {
                    _styles = new List<GUIStyle>();
                    _styles.Add(CreateButtonStyle());
                    _styles.Add(CreateIconButtonStyle(_deleteIcon));
                    _styles.Add(CreateLabelStyle(alignment: TextAnchor.MiddleCenter));
                    _styles.Add(CreateLabelStyle(alignment: TextAnchor.MiddleRight));
                    _styles.Add(CreateLabelStyle(fontStyle: FontStyle.Italic));
                    _styles.Add(CreateLabelStyle(fontStyle: FontStyle.Bold));
                    _styles.Add(CreateLabelStyle(fontSize: 16, fontStyle: FontStyle.Bold));
                    _styles.Add(CreateLabelStyle(fontSize: 14, fontStyle: FontStyle.Bold));
                    _styles.Add(CreateBoxStyle(EditorGUIUtility.isProSkin ? MakeColoredTexture(new Color(1f, 1f, 1f, 0.6f)) : MakeColoredTexture(new Color(0.4f, 0.4f, 0.4f, 0.6f))));
                    _styles.Add(CreateBoxStyle(EditorGUIUtility.isProSkin ? MakeColoredTexture(new Color(1f, 1f, 1f, 0.2f)) : MakeColoredTexture(new Color(1f, 1f, 1f, 0.6f)), fontSize: 14, alignment: TextAnchor.MiddleCenter, margin: new RectOffset(5,5,5,5)));
                    _styles.Add(CreateBoxStyle(EditorGUIUtility.isProSkin ? MakeColoredTexture(new Color(0.5f, 0.5f, 0.5f, 0.2f)) : MakeColoredTexture(new Color(1f, 1f, 1f, 0.4f))));
                }
                return _styles;
            }
            private set { }
        }
        static List<GUIStyle> _styles;


        public static Texture2D MakeColoredTexture(Color color)
        {
            var texture = new Texture2D(1, 1) {hideFlags = HideFlags.HideAndDontSave};
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        #region Create Styles
        public static GUIStyle CreateBoxStyle(Texture2D normalTexture, int fontSize = 11, FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.MiddleLeft, RectOffset padding = null, RectOffset border = null, RectOffset margin = null)
        {
            return new GUIStyle(GUI.skin.box)
            {
                normal = { background = normalTexture },
                fontSize = fontSize,
                fontStyle = fontStyle,
                alignment = alignment,
                padding = padding == null ? GUI.skin.box.padding : padding,
                border = border == null ? GUI.skin.box.border : border,
                margin = margin == null ? GUI.skin.box.margin : margin,
            };
        }

        public static GUIStyle CreateButtonStyle(int fontSize = 11, FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.MiddleLeft, RectOffset padding = null, RectOffset border = null, RectOffset margin = null)
        {
            return new GUIStyle(GUI.skin.button)
            {
                fontSize = fontSize,
                fontStyle = fontStyle,
                alignment = alignment,
                padding = padding == null ? GUI.skin.button.padding : padding,
                border = border == null ? GUI.skin.button.border : border,
                margin = margin == null ? GUI.skin.button.margin : margin,
            };
        }

        public static GUIStyle CreateIconButtonStyle(Texture2D normalTexture, Texture2D activeTexture = null, RectOffset padding = null, RectOffset border = null, RectOffset margin = null, TextAnchor alignment = TextAnchor.MiddleLeft)
        {
            return new GUIStyle
            {
                normal = { background = normalTexture },
                active = { background = activeTexture == null ? normalTexture : activeTexture },
                alignment = alignment,
                padding = padding == null ? GUI.skin.button.padding : padding,
                border = border == null ? GUI.skin.button.border : border,
                margin = margin == null ? GUI.skin.button.margin : margin,
            };
        }

        public static GUIStyle CreateLabelStyle(int fontSize = 11, FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.MiddleLeft, RectOffset padding = null, RectOffset border = null, RectOffset margin = null)
        {
            return new GUIStyle(GUI.skin.label)
            {
                fontSize = fontSize,
                fontStyle = fontStyle,
                alignment = alignment,
                padding = padding == null ? GUI.skin.label.padding : padding,
                border = border == null ? GUI.skin.label.border : border,
                margin = margin == null ? GUI.skin.label.margin : margin,
            };
        }
        #endregion Create Styles

        public static GUIStyle GetStyle(StyleType styleType)
        {
            return Styles[(int)styleType];
        }

        #region General Styles

        public static GUIStyle DropAreaStyle { get { return GetStyle(StyleType.DropAreaStyle); } }
        public static GUIStyle SeperatorStyle { get { return GetStyle(StyleType.SeperatorStyle); } }
        public static GUIStyle BoxLightStyle { get { return GetStyle(StyleType.BoxLightStyle); } }

        #endregion General Styles

        #region Button Styles

        public static GUIStyle ButtonBorderlessStyle { get { return GetStyle(StyleType.ButtonBorderless); } }
        public static GUIStyle IconButtonDeleteStyle { get { return GetStyle(StyleType.IconButtonDelete); } }

        #endregion Button Styles

        #region TextField Styles

        public static GUIStyle WordWrapStyle
        {
            get
            {
                if (_wordWrapStyle != null) return _wordWrapStyle;

                _wordWrapStyle = new GUIStyle(GUI.skin.textField)
                {
                    wordWrap = true
                };
                return _wordWrapStyle;
            }
        }
        static GUIStyle _wordWrapStyle;

        #endregion TextField Styles

        #region Label Styles

        public static GUIStyle LabelCentered { get { return GetStyle(StyleType.LabelCentred); } }
        public static GUIStyle LabelRight { get { return GetStyle(StyleType.LabelRight); } }
        public static GUIStyle LabelItalic { get { return GetStyle(StyleType.LabelItalic); } }
        public static GUIStyle LabelBold { get { return GetStyle(StyleType.LabelBold); } }
        public static GUIStyle H1 { get { return GetStyle(StyleType.H1); } }
        public static GUIStyle H2 { get { return GetStyle(StyleType.H2); } }

        public static GUIStyle CenteredInfoLabelStyle
        {
            get
            {
                if (_centeredInfoLabelStyle != null) return _centeredInfoLabelStyle;

                _centeredInfoLabelStyle = new GUIStyle(GUI.skin.label)
                {
                    normal = { textColor = EditorGUIUtility.isProSkin ? new Color(.8f, .8f, .8f) : new Color(.4f, .4f, .4f) },
                    alignment = TextAnchor.MiddleCenter
                };
                return _centeredInfoLabelStyle;
            }
        }
        static GUIStyle _centeredInfoLabelStyle;


        #endregion Label Styles
    }
}