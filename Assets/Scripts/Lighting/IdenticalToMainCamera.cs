using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdenticalToMainCamera : MonoBehaviour {

	private Camera c;
	private Camera m;

	// Use this for initialization
	void Start () {
		c = GetComponent<Camera> ();
		m = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		c.aspect = m.aspect;
		c.cameraType = m.cameraType;
		c.projectionMatrix = m.projectionMatrix;
		c.backgroundColor = m.backgroundColor;
		c.orthographicSize = m.orthographicSize;
		c.cameraType = m.cameraType;
		c.fieldOfView = m.fieldOfView;
	}
}
