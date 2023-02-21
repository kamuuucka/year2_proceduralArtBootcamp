using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Component5))]
public class Editor5 : Editor
{
    public override void OnInspectorGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Q)
        {
            Debug.Log("HELLO");
            e.Use();
        }
    }

    private void OnSceneGUI()
    {
        Event e = Event.current;
        // usually pressing space in the scene view spawns a context menu
        // we can Use the event to avoid that
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
        {
            e.Use();
        }
    }
}
