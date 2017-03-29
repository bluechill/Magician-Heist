using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsPrompt : MonoBehaviour {

	public bool kill = false;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (kill) {
			DestroyThis ();
		}
	}
	void DestroyThis(){
		transform.localScale = Vector3.Lerp (transform.localScale, Vector3.zero, 0.1f);
		if (transform.localScale.magnitude <= 0.0001f) {
			Destroy (this.gameObject);
		}
	}
	public void Kill(){
		kill = true;
	}
}
