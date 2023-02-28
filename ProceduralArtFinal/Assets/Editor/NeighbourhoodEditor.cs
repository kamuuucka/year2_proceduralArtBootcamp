using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Neighbrourhood))]
public class CurveEditor : Editor
{
    private Neighbrourhood curve;

    private void OnEnable()
    {
        curve = (Neighbrourhood)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Apply"))
        {
            curve.Apply();
        }
    }

    // This method is called by Unity whenever it renders the scene view.
    // We use it to draw gizmos, and deal with changes (dragging objects)
    void OnSceneGUI()
    {
        if (curve.points == null)
            return;

        DrawAndMoveCurve();

        // Add new points if needed:
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
        {
            Debug.Log("Space pressed - trying to add point to curve");
            e.Use(); // To prevent the event from being handled by other editor functionality
            AddPoint();
        }

        ShowAndMovePoints();
    }

    // Example: here's how to draw a handle:
    void DrawAndMoveCurve()
    {
        Transform handleTransform = curve.transform;
        Quaternion handleRotation =
            Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = Handles.PositionHandle(handleTransform.position, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve.transform, "Move curve");
            //EditorUtility.SetDirty(curve);		//Not necessary, undo does it by itself
            curve.transform.position = newPosition;
        }
    }

    // Tries to add a point to the curve, where the mouse is in the scene view.
    // Returns true if a change was made.
    void AddPoint()
    {
        Transform handleTransform = curve.transform;

        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        RaycastHit hit;
        EditorGUI.BeginChangeCheck();
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Adding spline point at mouse position: " + hit.point);
            //It's actually working, because RecordObject is already called in ShowAndMovePoints.
            //The point is being added so its position changes so the scene is marked dirty
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curve, "Save curve's points");
                curve.points.Add(handleTransform.InverseTransformPoint(hit.point));
                curve.Apply();
            }
        }
    }

    // Show points in scene view, and check if they're changed:
    void ShowAndMovePoints()
    {
        Transform handleTransform = curve.transform;

       
        Vector3 previousPoint = Vector3.zero;
        for (int i = 0; i < curve.points.Count; i++)
        {
            Vector3 currentPoint = curve.GetPoint(i);

            //Handles.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
            
            // Draw position handles (see the above example code)
            // Record in the undo list and mark the scene dirty when the handle is moved.
            
            //Begin change check needs to be here to check only changes that are happening when you move the point
            //Not, when you draw the lines!!!
            EditorGUI.BeginChangeCheck();
            currentPoint = Handles.PositionHandle(currentPoint, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(curve, "Save curve's positions");
                curve.points[i] = handleTransform.InverseTransformPoint(currentPoint);
                curve.Apply();
            }
        }
    }
}