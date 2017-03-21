using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor {

	void OnSceneGUI() {
		FieldOfView fov = (FieldOfView)target;
		Handles.color = Color.white;
		Handles.DrawWireArc (fov.transform.position, Vector3.forward, Vector3.up, 360, fov.viewRadius);

		Vector3 viewAngleA = fov.DirectionFromAngle (-fov.viewAngle / 2f, false);
		Vector3 viewAngleB = fov.DirectionFromAngle (fov.viewAngle / 2f, false);

		Handles.DrawLine (fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
		Handles.DrawLine (fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

		Handles.color = Color.red;
		foreach (Transform visibleTarget in fov.visibleTargets)
			Handles.DrawLine (fov.transform.position, visibleTarget.position);
	}
}
