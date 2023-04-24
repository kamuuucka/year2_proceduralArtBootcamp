using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSquare : MonoBehaviour
{
    public GameObject asset;
    public List<Vector3> points;
    public List<Vector3> buildingSpawns;
    public int floors = 1;
    [HideInInspector]public Vector3 assetSize;
    
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

    public void Generate()
    {
        for (int i = 0; i < floors; i++)
        {
            foreach (var building in buildingSpawns)
            {
                int buildingIndex = 0;
                GameObject newBuilding = Instantiate(asset, transform);

                // Place it in the grid:
                newBuilding.transform.position = new Vector3(building.x, building.y + assetSize.y * i, building.z);
            }
        }
        
    }
    

    public void RemoveAllChildren()
    {
        foreach (Transform child in this.transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public void Destroy()
    {
        //For some reason you have to destroy the children five times, those shitlings are immune to my powers
        for (int i = 0; i < 4 + floors; i++)
        {
            RemoveAllChildren();
        }
    }
    
    public Vector3 GetPoint(int pointIndex) {
        if (pointIndex<0 || pointIndex>=points.Count) {
            Debug.Log("Curve.cs: WARNING: pointIndex out of range: " + pointIndex + " curve length: " + points.Count);
            return Vector3.zero;
        }
        return transform.TransformPoint(points[pointIndex]);
    }
}
