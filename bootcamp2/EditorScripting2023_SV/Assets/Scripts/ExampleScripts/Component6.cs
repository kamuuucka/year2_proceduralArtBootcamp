using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableWaypoint
{
    public string name;
    public Vector3 position;
    public float waitTime;
}

public class Component6 : MonoBehaviour
{
    public List<int> ints = new List<int>();  // does get shown... unless we add [HideInInspector] to it
    public List<SerializableWaypoint> waypoints = new List<SerializableWaypoint>();     // does not get shown... unless we add [System.Serializable] to the Waypoint class
}