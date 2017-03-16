using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {

	public bool activated;
	public bool Up = true;
	public float height;
	SpriteRenderer sprend;
	public GameObject arrow;
	bool arrow_set = false;
	public bool arrow_started;
	public bool started = false;
	void Start () {
		sprend = GetComponent<SpriteRenderer> ();
		arrow_started = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (activated && !arrow_started) {
			if(!started) BlinkArrow ();
		}
	}
	void BlinkArrow(){
		started = true;
		if (!activated) {
			arrow_started = false;
			arrow.SetActive (false);
			return;
		}

		arrow.SetActive (arrow_set);
		arrow_set = !arrow_set;
		Invoke ("BlinkArrow", 1f);
	}
}
