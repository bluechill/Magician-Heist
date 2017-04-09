using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsPrompt : MonoBehaviour {

	public bool kill = false;
	public bool up;
	TextMesh txt;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(!txt) txt = GetComponentInChildren<TextMesh> ();
		txt.text = "";
		if (up)
			txt.text = "Up Stairs";
		else
			txt.text = "Down Stairs";
		
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
