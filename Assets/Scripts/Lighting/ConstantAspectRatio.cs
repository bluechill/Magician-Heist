using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantAspectRatio : MonoBehaviour {

	private Camera c;

	// Use this for initialization
	void Start () {
		c = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		c.aspect = 1;
	}
}
