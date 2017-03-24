using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
    public bool locked = false;
	public bool open = false;

	public List<GameObject> players_in;
	public List<GameObject> players_moving;
	public bool has_players = false;
	bool mode = false;
	public bool choice;
	public GameObject choice_menu;
	public GameObject up_connection;
	public GameObject down_connection;

	public Sprite[] sprites;
	// Update is called once per frame
	void Update () {

		if (open) {
			GetComponent<SpriteRenderer> ().sprite = sprites[1];
		} else {
			GetComponent<SpriteRenderer> ().sprite = sprites[0];
		}

		ProcessIn ();
	}
	void ProcessIn(){
		int i = 0;
		foreach (GameObject player in players_in) {
			if(open) player.GetComponent<PlayerScript> ().ReappearBody();
			else player.GetComponent<PlayerScript> ().DisappearBody();
			player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			player.transform.position = new Vector3 (transform.position.x - 0.4f + (0.3f * i), transform.position.y - 0.1f, 0f);
			i++;
		}

		if (i > 0 && !open) {
			ShowChoice ();
			ChangeChoice ();
		} else {
			HideChoice ();
		}

	}
	void ShowChoice(){
		choice_menu.SetActive (true);
	}	
	void HideChoice(){
		choice_menu.SetActive (false);
	}
	void ChangeChoice(){
		if(players_in [0].GetComponent<PlayerScript> ().up) choice = true;
		if(players_in [0].GetComponent<PlayerScript> ().down) choice = false;
	}
	void ProcessOut(){
		foreach (GameObject player in players_in) {
			player.GetComponent<PlayerScript> ().ReappearBody();
		}
	}
	public void SwitchState(GameObject player){

		open = !open;
	}
	public void GetIn(GameObject player){
		players_in.Add (player);
	}
	public void GetOut(GameObject player){
		players_in.Remove (player);
	}
	public void OpenUp(){
		up_connection.GetComponent<Elevator> ().open = true;
	}
	public void OpenDown(){
		down_connection.GetComponent<Elevator> ().open = true;
	}
	public void CloseUp(){
		up_connection.GetComponent<Elevator> ().open = false;
	}
	public void CloseDown(){
		down_connection.GetComponent<Elevator> ().open = false;
	}
	public void Use(){
		if (choice) {
			Invoke("UseUp", 2f);
		} else {
			Invoke("UseDown", 2f);
		}
	}
	public void UseUp(){
		if (!up_connection)
			return;
		CloseUp ();
		foreach (GameObject player in players_in) {
			player.transform.position = up_connection.transform.position;
			up_connection.GetComponent<Elevator> ().GetIn (player);
		}
		players_in.Clear ();
	}
	public void UseDown(){
		if (!down_connection)
			return;
		CloseDown ();
		foreach (GameObject player in players_in) {
			player.transform.position = down_connection.transform.position;
			down_connection.GetComponent<Elevator> ().GetIn (player);
		}
		players_in.Clear ();
	}
}
