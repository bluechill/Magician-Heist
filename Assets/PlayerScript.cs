using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
	public bool right,left;
	public bool is_hiding = false;
	public bool is_in_box = false;
	public bool is_in_closet = false;
	public bool using_up_escalator = false;
	public bool is_holding = false;
	public GameObject held_object;
	public bool is_being_held = false;
	public GameObject being_held_by;
	public bool is_touching_box = false;
	public bool is_touching_up_stairs = false;
	public bool is_touching_down_stairs = false;
	public bool is_touching_door = false;
	public bool is_holding_briefcase = false;
	public GameObject touching_box;
	public GameObject touching_door;
	public GameObject inside_box;
	public GameObject touching_stairs;
	public GameObject win_menu;

	public GameObject ability;
	public GameObject body;
	public CapsuleCollider capsule;
	public bool is_ability = false;

	public bool is_knocked_out = false;


	public void Drop(){
		if (!is_holding)
			return;
		if (held_object.GetComponent<Item> ().briefcase) {
			is_holding_briefcase = false;
		}
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
		inside_box.GetComponentInParent<BoxPlayerScript> ().players_in_box.Add (this.gameObject);
		StopTouchBox (touching_box);
		Hide ();
	}
	public void ExitBox(){
		is_hiding = false;
		is_in_box = false;
		body.SetActive (true);
		inside_box.GetComponentInParent<BoxPlayerScript> ().players_in_box.Remove (this.gameObject);
		inside_box = null;
		Reveal ();
	}
	public void UseUpStairs(){
		transform.position = new Vector3 (transform.position.x, transform.position.y + 3.89f, 0f);
	}
	public void UseDownStairs(){
		transform.position = new Vector3 (transform.position.x, transform.position.y - 3.89f, 0f);
	}
	public void OpenDoor(){
		touching_door.GetComponentInChildren<Door> ().OpenDoor ();
	}
	public void KnockOut(){
		is_knocked_out = true;
		this.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,90f));
		right = false;
		left = false;
		Hide ();
		Invoke ("Wakeup", 5f);
	}
	public void Wakeup(){
		is_knocked_out = false;
		Reveal ();
		this.transform.rotation = Quaternion.Euler(new Vector3(0f,180f,0f));
		this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y + 0.5f,0f);
	}
	public void Hide(){
		is_hiding = true;
	}	
	public void Reveal(){
		is_hiding = false;
	}
	public void Win(){
		win_menu.SetActive (true);
		Invoke ("Restart", 5f);
	}
	public void Restart(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
}