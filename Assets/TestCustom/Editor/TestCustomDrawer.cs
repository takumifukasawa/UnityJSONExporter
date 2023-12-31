using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using Custom;

[CustomPropertyDrawer(typeof(TestCustomBehaviour))]
public class TestCustomDrawer : PropertyDrawer
{
    public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
    {
        int fieldCount = 3;
        return fieldCount * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty CustomPropertyColorProp = property.FindPropertyRelative("CustomPropertyColor");
        SerializedProperty CustomPropertyFloatProp = property.FindPropertyRelative("CustomPropertyFloat");
        SerializedProperty CustomPropertyVector3Prop = property.FindPropertyRelative("CustomPropertyVector3");

        Rect singleFieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(singleFieldRect, CustomPropertyColorProp);

        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, CustomPropertyFloatProp);

        singleFieldRect.y += EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(singleFieldRect, CustomPropertyVector3Prop);
    }
}
