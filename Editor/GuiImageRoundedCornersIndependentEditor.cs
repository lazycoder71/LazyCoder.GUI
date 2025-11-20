using UnityEditor;
using UnityEngine;

namespace LazyCoder.Gui.Editor
{
    [CustomEditor(typeof(GuiRoundedCornersIndependent))]
    public class GuiImageRoundedCornersIndependentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
        
            SerializedProperty vector4Prop = serializedObject.FindProperty("_r");
        
            EditorGUILayout.PropertyField(vector4Prop.FindPropertyRelative("x"), new GUIContent("Top Left Corner"));
            EditorGUILayout.PropertyField(vector4Prop.FindPropertyRelative("y"), new GUIContent("Top Right Corner"));
            EditorGUILayout.PropertyField(vector4Prop.FindPropertyRelative("w"), new GUIContent("Bottom Left Corner"));
            EditorGUILayout.PropertyField(vector4Prop.FindPropertyRelative("z"), new GUIContent("Bottom Right Corner"));
        
            serializedObject.ApplyModifiedProperties();
        }
    }
}