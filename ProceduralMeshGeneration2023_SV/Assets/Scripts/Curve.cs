﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Curve : MonoBehaviour {
	public List<Vector3> points;

	public void Apply() {
		MeshCreator creator = GetComponent<MeshCreator>();
		if (creator!=null) {
			creator.RecalculateMesh();
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

