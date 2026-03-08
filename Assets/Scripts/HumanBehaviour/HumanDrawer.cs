// 
// using UnityEditor;
// using UnityEngine;
// 
// [CustomPropertyDrawer(typeof(HumanBehaviour))]
// public class HumanDrawer : PropertyDrawer
// {
//     public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//     {
//         int fieldCount = 1;
//         return fieldCount * EditorGUIUtility.singleLineHeight;
//     }
//     
//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         // SerializedProperty LocalPositionProp = property.FindPropertyRelative(HumanController.LocalPositionPropertyName);
//         // SerializedProperty LookAtTargetProp = property.FindPropertyRelative(HumanController.LookAtTargetPropertyName);
//         // 
//         // Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
//         // EditorGUI.PropertyField(singleFieldRect, LocalPositionProp);
//         // 
//         // singleFieldRect.y += EditorGUIUtility.singleLineHeight;
//         // EditorGUI.PropertyField(singleFieldRect, LookAtTargetProp);
//         
//         SerializedProperty LeftShoulderRotationProp = property.FindPropertyRelative(HumanController.LeftShoulderRotationPropertyName);
//         // SerializedProperty LookAtTargetProp = property.FindPropertyRelative(HumanController.LookAtTargetPropertyName);
//         
//         Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
//         EditorGUI.PropertyField(singleFieldRect, LeftShoulderRotationProp);
//     }
// }
// 
