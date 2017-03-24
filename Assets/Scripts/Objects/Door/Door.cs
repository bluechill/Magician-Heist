using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Director;

public class Door : LayerMonoBehavior {
	public Sprite[] sprites;
	public bool open = false;
	public GameObject wall;
	public bool locked = false;
	public void SwitchState(){
		if (!open) {
			if (locked) {

			} else {
				open = true;
			}
		} else {
			open = false;
		}
	}
	public void Unlock(){
		locked = false;
	}
	void Update(){
		if (open) {
			wall.SetActive (false);
			GetComponent<SpriteRenderer> ().sprite = sprites[1];
		} else {
			wall.SetActive (true);
			GetComponent<SpriteRenderer> ().sprite = sprites[0];
		}
	}
}
