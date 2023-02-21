using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Component1))]
public class Editor1 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // same as DrawDefaultInspector
        EditorGUILayout.HelpBox("Booh!", MessageType.Warning);
        // Debug.Log("Current event: " + Event.current.type);
    }
}
