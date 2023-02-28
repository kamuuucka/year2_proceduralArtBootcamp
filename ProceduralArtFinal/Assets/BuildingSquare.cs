using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSquare : MonoBehaviour
{
    public GameObject asset;
    public List<Vector3> points = new List<Vector3>();
    public List<Vector3> buildingSpawns;
    
    private Vector3 point1;
    private Vector3 point2;
    private Vector3 point3;
    private Vector3 point4;


    private void Start()
    {
        if (buildingSpawns != null)
        {
            Generate();
        }
    }

    private void Generate()
    {
        foreach (var building in buildingSpawns)
        {
            
                int buildingIndex = 0;
                GameObject newBuilding = Instantiate(asset, transform);

                // Place it in the grid:
                newBuilding.transform.localPosition = new Vector3(building.x, building.y, building.z);
        }
        
    }
    
    public Vector3 GetPoint(int pointIndex) {
        if (pointIndex<0 || pointIndex>=points.Count) {
            Debug.Log("Curve.cs: WARNING: pointIndex out of range: " + pointIndex + " curve length: " + points.Count);
            return Vector3.zero;
        }
        return transform.TransformPoint(points[pointIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
