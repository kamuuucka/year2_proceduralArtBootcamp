using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Component4)), CanEditMultipleObjects]
public class Editor4 : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty valueProperty = serializedObject.FindProperty("value");
        int value = valueProperty.intValue;
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.PropertyField(valueProperty);
            if (GUILayout.Button("-"))
            {
                value--;
                valueProperty.intValue = value;
            }
            if (GUILayout.Button("+"))
            {
                value++;
                valueProperty.intValue = value;
            }
        }
        EditorGUILayout.EndHorizontal();

        /*SerializedProperty listProperty = serializedObject.FindProperty("list");
        EditorGUILayout.PropertyField(listProperty);
        if (GUILayout.Button("Add a number!"))
        {
            listProperty.InsertArrayElementAtIndex(listProperty.arraySize);
            SerializedProperty newIntProperty = listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1);
            newIntProperty.intValue = listProperty.arraySize;
        }*/

        serializedObject.ApplyModifiedProperties();
    }
}
