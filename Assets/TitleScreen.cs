using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour {
	public bool kill = false;
	public bool grow = true;
	public GameObject selector;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (grow) {
			GrowThis ();
		}
		if (kill) {
			DestroyThis ();
		}
	}
	public void Kill(){
		grow = false;
		kill = true;
	}
	void DestroyThis(){
		transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(0f, 1f, 1f), 0.1f);
		if (transform.localScale.x <= 0.0001f) {
			selector.GetComponent<PlayerSelector> ().Grow ();
			Destroy (this.gameObject);
		}
	}
	void GrowThis(){
		transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(1f, 1f, 1f), 0.1f);
		if (transform.localScale.x >= 0.95f) {
			grow = false;
		}
	}
}
