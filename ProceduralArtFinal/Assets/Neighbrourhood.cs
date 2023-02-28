using System.Collections.Generic;
using UnityEngine;
using System;

public class Neighbrourhood : MonoBehaviour {
	public List<Vector3> points;
	public GameObject[] buildingPrefabs;

	public event Action OnApply;

	public int NumPoints() {
		return points.Count;
	}

	public Vector3 GetPoint(int pointIndex) {
		if (pointIndex<0 || pointIndex>=points.Count) {
			Debug.Log("Curve.cs: WARNING: pointIndex out of range: " + pointIndex + " curve length: " + points.Count);
			return Vector3.zero;
		}
		return transform.TransformPoint(points[pointIndex]);
	}

	public void Apply() {
		Debug.Log("Applying curve");
		if (OnApply != null) OnApply();

		foreach (var point in points)
		{
			int buildingIndex = 0;
			GameObject newBuilding = Instantiate(buildingPrefabs[buildingIndex], transform);

			// Place it in the grid:
			newBuilding.transform.localPosition = new Vector3(point.x, point.y, point.z);

		}
	}
}

