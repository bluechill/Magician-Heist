using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Director;

public class Door : LayerMonoBehavior {

	public bool open = false;
	public GameObject wall;

	public void SwitchState(bool key){
		if (!open) {
			if (key) {
				open = true;

			}
		} else {
			open = false;
		}
	}

	void Update(){
		if (open) {
			wall.SetActive (false);
			GetComponentInParent<SpriteRenderer> ().enabled = false;
			GetComponent<SpriteRenderer> ().enabled = true;
		} else {
			wall.SetActive (true);
			GetComponentInParent<SpriteRenderer> ().enabled = false;
			GetComponent<SpriteRenderer> ().enabled = false;
		}
	}
}
