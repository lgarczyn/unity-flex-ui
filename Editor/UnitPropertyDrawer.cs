using Gilzoide.FlexUi.Yoga;
using UnityEditor;
using UnityEngine;

namespace Gilzoide.FlexUi.Editor
{
    [CustomPropertyDrawer(typeof(Unit))]
    public class UnitPropertyDrawer : PropertyDrawer
    {
        internal static bool NoAuto { get; set;}

        static readonly string[] _labels = { "px", "%", "auto" };
        static readonly string[] _noAutoLabels = { "px", "%" };

        private (Rect, Rect) GetToggleAndPopupRects(Rect position)
        {
            Rect toggleRect = position;
            toggleRect.width = toggleRect.height;
            Rect popupRect = position;
            popupRect.xMin += toggleRect.width + EditorGUIUtility.standardVerticalSpacing;
            return (toggleRect, popupRect);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            (Rect toggleRect, Rect popupRect) = GetToggleAndPopupRects(position);

            int value = property.enumValueIndex;

            bool enabled = DrawToggle(toggleRect, ref value);
            DrawPopup(popupRect, label, ref value, enabled);

            if (EditorGUI.EndChangeCheck())
            {
                property.enumValueIndex = value;
            }
            EditorGUI.EndProperty();
        }

        private static bool DrawToggle(Rect toggleRect, ref int value)
        {
            bool enabled = value != 0;
            enabled = EditorGUI.Toggle(toggleRect, enabled);
            // if enabled just changed to true, set value to 1 (px)
            // if enabled just changed to false, set value to 0 (undefined)
            if (enabled != (value != 0))
            {
                value = enabled ? 1 : 0;
            }
            return enabled;
        }

        private static void DrawPopup(Rect popupRect, GUIContent label, ref int value, bool enabled)
        {
            EditorGUI.BeginDisabledGroup(!enabled);
            int index = enabled ? value - 1 : 0; // Adjust index for popup options
            index = EditorGUI.Popup(popupRect, label.text, index, NoAuto ? _noAutoLabels : _labels);

            if (enabled)
            {
                value = index + 1;
            }
            EditorGUI.EndDisabledGroup();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
