using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Location
{
    public string name;
    public Vector3 position;
    public Vector3 size = Vector3.one;
}

public class SceneDrawExample : MonoBehaviour
{
    public List<Location> locations;

    public bool drawAreaOutlines;
    private GUIStyle labelStyle = new GUIStyle();

    #region Gizmos and Handles
    
    // This code is always executed when this object is in the Scene
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 0.4f);
        for (int i = 0; i < locations.Count; i++)
        {
            Gizmos.DrawCube(locations[i].position, Vector3.one);
        }
    }

    // This code is executed when we have the object selected
    private void OnDrawGizmosSelected()
    {
        labelStyle.fontSize = 24;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = Color.white;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        for (int i = 0; i < locations.Count; i++)
        {
            Handles.Label(locations[i].position + Vector3.up * 5f, locations[i].name, labelStyle);
        }
        
    }

    #endregion

    private void Update()
    {
        DrawDebugLines();
    }

    private void DrawDebugLines()
    {
        for (int i = 1; i < locations.Count; i++)
        {
            Debug.DrawLine(locations[i - 1].position, locations [i].position, Color.red);
        }
    }
}

public class EpicGizmoDrawer
{
    // Another way to draw Gizmos. With the attribute we control under which circumstances these Gizmos should be drawn
    [DrawGizmo(GizmoType.Selected)]
    private static void IAlsoWantToDrawGizmosFrownyFace(SceneDrawExample script, GizmoType gizmoType)
    {
        if (script.drawAreaOutlines)
        {
            for (int i = 0; i < script.locations.Count; i++)
            {
                Gizmos.DrawWireCube(script.locations[i].position, script.locations[i].size);
            }
        }
    }
}