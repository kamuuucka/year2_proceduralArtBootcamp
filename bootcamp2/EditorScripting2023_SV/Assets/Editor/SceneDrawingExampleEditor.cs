using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(SceneDrawExample))]
public class SceneDrawingExampleEditor : Editor
{
    private void OnSceneGUI()
    {
        SceneDrawExample example = (SceneDrawExample)target;

        for (int i = 0; i < example.locations.Count; i++)
        {
            Vector3 position = example.locations[i].position;
            Vector3 scale = example.locations[i].size;

            EditorGUI.BeginChangeCheck();
            position = Handles.PositionHandle(position, Quaternion.identity);
            scale = Handles.ScaleHandle(scale, position, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(example, "Updated location");
                example.locations[i].position = position;
                example.locations[i].size = scale;
                EditorUtility.SetDirty(example);
                // or EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
}