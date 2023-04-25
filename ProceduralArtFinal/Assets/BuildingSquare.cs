using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BuildingSquare : MonoBehaviour
{
    public GameObject asset;
    public GameObject roof;
    public List<Vector3> points;
    public List<Vector3> buildingSpawns;
    [HideInInspector]public Vector3 assetSize;
    [SerializeField] private int floors = 1;
    [SerializeField] private int doorHeight;
    [SerializeField] private int direction;
    
    private Vector3 point1;
    private Vector3 point2;
    private Vector3 point3;
    private Vector3 point4;

    [SerializeField] private GameObject roof0;
    [SerializeField] private GameObject roof1;
    [SerializeField] private GameObject roof2;
    [SerializeField] private GameObject roof3;
    private void Start()
    {
        if (buildingSpawns != null)
        {
            Generate();
        }
    }

    public void Generate()
    {
        GameObject newBuilding;
        int line = buildingSpawns.Count / 4;
        int doorPosition = UnityEngine.Random.Range(1, line);
        Vector3 modifier = new Vector3(0, 0, 0);
        for (int i = 0; i < floors; i++)
        {
            GameObject newChild = new GameObject("Floor" + i);
            newChild.transform.parent = transform;
            for (var index = 0; index < buildingSpawns.Count; index++)
            {
                var building = buildingSpawns[index];

                if (index != doorPosition + line * direction || i > doorHeight-1)
                {
                    if (i == floors - 1)
                    {
                        if (index % line == 0)
                        {
                            newBuilding = Instantiate(roof0, newChild.transform.parent);
                            newBuilding.transform.Rotate(Vector3.up, 90*( index/line-2));
                            Debug.Log(index/line);
                            switch (index / line)
                            {
                                case 0:
                                    modifier = new Vector3(-1, 0, -1);
                                    break;
                                case 1:
                                    modifier = new Vector3(-1, 0, 0);
                                    break;
                                case 2:
                                    modifier = new Vector3(0, 0, 0);
                                    break;
                                case 3:
                                    modifier = new Vector3(0, 0, -1);
                                    break;
                                
                            }
                        }
                        else
                        {
                            //Classic roofs (not corners)
                            newBuilding = Instantiate(roof, newChild.transform.parent);
                            if ((index >= 1 && index < line) || (index >= line * 2 + 1 && index < line * 3))
                            {
                                newBuilding.transform.Rotate(Vector3.up, 90);
                                modifier = new Vector3(0,0,-1);
                            }
                            else
                            {
                                modifier = new Vector3(0, 0, 0);
                            }
                            
                        }

                    }
                    else
                    {
                        newBuilding = Instantiate(asset, newChild.transform.parent);
                    }
                    
                    newBuilding.transform.parent = newChild.transform;

                    // Place it in the grid:
                    newBuilding.transform.position = new Vector3(building.x, building.y + assetSize.y * i, building.z) + modifier;
                    
                }
            }
        }
    }

    private void MakeAGate(GameObject wall)
    {
        DestroyImmediate(wall);
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
