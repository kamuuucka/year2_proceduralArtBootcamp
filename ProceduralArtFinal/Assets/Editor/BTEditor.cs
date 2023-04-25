using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;

[CustomEditor(typeof(BuildingTriangle))]
public class BTEditor : Editor
{
    private BuildingTriangle building;

    private void OnEnable()
    {
        building = (BuildingTriangle)target;
    }
    
    //TODO: Use draw lines to make the angled lines
    //TODO: warp quad for pillars in the corners
    
    void OnSceneGUI()
    {
        if (building.points == null)
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
        Transform handleTransform = building.transform;
        Quaternion handleRotation =
            Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = Handles.PositionHandle(handleTransform.position, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(building.transform, "Move curve");
            //EditorUtility.SetDirty(curve);		//Not necessary, undo does it by itself
            building.transform.position = newPosition;
        }
    }

    // Tries to add a point to the curve, where the mouse is in the scene view.
    // Returns true if a change was made.
    void AddPoint()
    {
        Transform handleTransform = building.transform;

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
                Undo.RecordObject(building, "Save curve's points");
                building.points.Add(handleTransform.InverseTransformPoint(hit.point));
            }
        }
    }

    // Show points in scene view, and check if they're changed:
    void ShowAndMovePoints()
    {
        Transform handleTransform = building.transform;


        Vector3 previousPoint = Vector3.zero;
        Vector3 nextPoint = Vector3.zero;
        building.buildingSpawns.Clear();
        for (int i = 0; i < 4; i++)
        {
            Vector3 currentPoint = building.GetPoint(i);
            nextPoint = i != 3 ? building.GetPoint(i + 1) : building.GetPoint(0);

            Handles.color = Color.yellow;
            Vector3 assetSize = building.asset.GetComponentInChildren<Renderer>().bounds.size;


            ShowRowOfBuildings(currentPoint, nextPoint, assetSize);


            // Draw position handles (see the above example code)
            // Record in the undo list and mark the scene dirty when the handle is moved.

            //Begin change check needs to be here to check only changes that are happening when you move the point
            //Not, when you draw the lines!!!
            EditorGUI.BeginChangeCheck();
            currentPoint = Handles.PositionHandle(currentPoint, Quaternion.identity);
            

            

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(building, "Save curve's positions");
     
                building.points[i] = handleTransform.InverseTransformPoint(currentPoint);
                Vector3 point0 = handleTransform.TransformPoint(building.points[0]);
                Vector3 point1 = handleTransform.TransformPoint(building.points[1]);
                Vector3 point2 = handleTransform.TransformPoint(building.points[2]);
                Vector3 point3 = handleTransform.TransformPoint(building.points[3]);
                //Vector3 point4 = handleTransform.TransformPoint(building.points[4]);
                //Vector3 point5 = handleTransform.TransformPoint(building.points[5]);
                //Moving the points so the shape is always rectangle
                switch(i)
                {
                    case 0:
                        point1.x = currentPoint.x - assetSize.x/2;
                        point1.z = currentPoint.z + assetSize.z/2;
                        point2.x = point1.x - assetSize.x / 2;
                        point2.z = point1.z - assetSize.z / 2;
                        break;
                    case 1:
                        point0.x = currentPoint.x + assetSize.x / 2;
                        point0.z = currentPoint.z - assetSize.z / 2;
                        point2.x = currentPoint.x - assetSize.x / 2;
                        point2.z = currentPoint.z - assetSize.z / 2;
                        break;
                    case 2:
                        point1.x = currentPoint.x + assetSize.x/2;
                        point1.z = currentPoint.z + assetSize.z/2;
                        point0.x = point1.x + assetSize.x / 2;
                        point0.z = point1.z - assetSize.z / 2;
                        break;
                }
                
                building.points[0] = handleTransform.InverseTransformPoint(point0);
                building.points[1] = handleTransform.InverseTransformPoint(point1);
                building.points[2] = handleTransform.InverseTransformPoint(point2);
                building.points[3] = handleTransform.InverseTransformPoint(point3);
        
                //building.Apply();
            }
        }
    }

    private void ShowRowOfBuildings(Vector3 point1, Vector3 point2, Vector3 size)
    {
        float offset = size.x;
        int distance = (int)Vector3.Distance(point1, point2);
        //Bulding spawnpoint not on the point
        //Handles.DrawWireCube(new Vector3(point1.x, point1.y, point1.z - offset),size);

        
            Handles.DrawLine(new Vector3(point1.x, point1.y, point1.z),point2);
            // if (point1.z < point2.z && point1.x > point2.x)
            // {
            //     Handles.DrawLine(new Vector3(point1.x - size.x/2, point1.y, point1.z + size.z/2),point2);
            // }
            // if (Math.Abs(point1.x - point2.x) < 1)
            // {
            //     //Vector3 newPoint1 = Vector3.RotateTowards(new Vector3(0,0,point1.z),point2,2,0);
            //     //Handles.DrawWireCube(new Vector3(point1.x, point1.y, point1.z - offset * i), size);
            //     //Handles.DrawLine(point1, point2);
            // }
        
        // 
        // int distance = (int)Vector3.Distance(point1, point2);
        // for (int i = 0; i < distance + 1; i++)
        // {
        //     if (Math.Abs(point1.x - point2.x) < 1 && point1.z < point2.z)
        //     {
        //         Handles.DrawWireCube(new Vector3(point1.x, point1.y, point1.z + offset * i), size);
        //         building.buildingSpawns.Add(new Vector3(point1.x, point1.y, point1.z + offset * i));
        //     }
        //     else if (Math.Abs(point1.z - point2.z) < 1 && point1.x > point2.x)
        //     {
        //         Handles.DrawWireCube(new Vector3(point1.x - offset * i, point1.y, point1.z), size);
        //         building.buildingSpawns.Add(new Vector3(point1.x - offset * i, point1.y, point1.z));
        //     }
        //     else if (Math.Abs(point1.x - point2.x) < 1 && point1.z > point2.z)
        //     {
        //         Handles.DrawWireCube(new Vector3(point1.x, point1.y, point1.z - offset * i), size);
        //         building.buildingSpawns.Add(new Vector3(point1.x, point1.y, point1.z - offset * i));
        //     }
        //     else if (Math.Abs(point1.z - point2.z) < 1 && point1.x < point2.x)
        //     {
        //         Handles.DrawWireCube(new Vector3(point1.x + offset * i, point1.y, point1.z), size);
        //         building.buildingSpawns.Add(new Vector3(point1.x + offset * i, point1.y, point1.z));
        //     }
        // }
    }
}
