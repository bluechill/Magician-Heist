using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.x < 1.9f) {
			transform.localScale = Vector3.Lerp (transform.localScale, Vector3.one * 2, 0.1f);
		}
	}
}
