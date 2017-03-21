using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour {

	public GameObject opened;
	public bool open = false;
	public SpriteRenderer sprend;
	public SpriteRenderer opened_sprend;

	public bool being_used = false;
	public GameObject player_inside;

	// Use this for initialization
	void Start () {
		sprend = GetComponent<SpriteRenderer> ();
		opened_sprend = opened.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (open) {
			sprend.enabled = false;
			opened_sprend.enabled = true;
		} else {
			sprend.enabled = true;
			opened_sprend.enabled = false;
		}
	}
	public void SwitchStates(){
		open = !open;
		if (open) {
			OpenCloset ();
		} 
	}
	public void EnterCloset(GameObject player){
		player_inside = player;
		open = false;
		being_used = true;
	}
	public void OpenCloset(){
		if (being_used) {
			player_inside.GetComponent<PlayerScript> ().ExitCloset();
		}
		player_inside = null;
		open = true;
		being_used = false;
	}
}
