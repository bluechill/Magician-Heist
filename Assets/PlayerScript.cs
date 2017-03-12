using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public bool is_hiding = false;
	public bool is_in_box = false;
	public bool is_in_closet = false;
	public bool using_up_escalator = false;
	public bool is_holding = false;
	public GameObject held_object;
	public bool is_being_held = false;
	public GameObject being_held_by;
	public bool is_touching_box = false;
	public GameObject touching_box;
	public GameObject inside_box;

	public GameObject ability;
	public GameObject body;
	public CapsuleCollider capsule;
	public bool is_ability = false;


	public void Drop(){
		if (!is_holding)
			return;

		held_object = null;
		is_holding = false;
	}

	public void GetPickedUp(GameObject obj){
		is_being_held = true;
		being_held_by = obj;
	}
	public void GetDropped(){
		is_being_held = false;
		being_held_by.GetComponent<PlayerScript> ().Drop ();
		being_held_by = null;
	}
	public void UseUpEscalator(){
		using_up_escalator = true;
	}
	public void TouchBox(GameObject box){
		is_touching_box = true;
		touching_box = box;
	}
	public void StopTouchBox(GameObject box){
		is_touching_box = false;
		touching_box = null;
	}
	public void EnterBox(){
		is_hiding = true;
		is_in_box = true;
		body.SetActive (false);
		inside_box = touching_box;
		StopTouchBox (touching_box);
	}
	public void ExitBox(){
		is_hiding = false;
		is_in_box = false;
		body.SetActive (true);
	}
}