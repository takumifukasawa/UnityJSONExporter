
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ObjectMoveAndLookAtBehaviour))]
public class ObjectMoveAndLookAtDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int fieldCount = 3;
        return fieldCount * EditorGUIUtility.singleLineHeight;
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty LocalPositionProp = property.FindPropertyRelative(ObjectMoveAndLookAtController.LocalPositionPropertyName);
        SerializedProperty UpVectorProp = property.FindPropertyRelative(ObjectMoveAndLookAtController.UpVectorPropertyName);
        SerializedProperty LookAtTargetProp = property.FindPropertyRelative(ObjectMoveAndLookAtController.LookAtTargetPropertyName);

        Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(singleFieldRect, LocalPositionProp);

        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, UpVectorProp);

        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, LookAtTargetProp);
    }
}
