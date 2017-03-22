using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
    public bool locked = false;
	public bool open = false;
	public GameObject door_open;
	public List<GameObject> players_in;
	public List<GameObject> players_moving;
	public bool has_players = false;
	bool mode = false;
	public GameObject up_connection;
	public GameObject down_connection;
	// Update is called once per frame
	void Update () {

		if (open) {
			door_open.GetComponent<SpriteRenderer> ().enabled = true;
			GetComponent<SpriteRenderer> ().enabled = false;
		} else {
			door_open.GetComponent<SpriteRenderer> ().enabled = false;			
			GetComponent<SpriteRenderer> ().enabled = true;

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
	public void Use(bool up, GameObject player){
		if (up && !up_connection)
			return;
		if (!up && !down_connection)
			return;

        if (!up && locked)
        {
            if (player.GetComponent<PlayerScript>().is_holding_key_card)
            {
                locked = false;

            } else
            {
                return;
            }


        }
		mode = up;
		open = false;
		Use2 ();
	}
	public void Use2(){
		if (mode) {
			MoveUp ();
		} else {
			MoveDown ();
		}
	}
	public void MoveUp(){
		foreach (GameObject player in players_in) {
			Move (player, true);
		}
		Invoke ("OpenUp", 1f);
		Invoke ("ReappearPlayers", 1f);
		Invoke ("CloseUp", 1.3f);
	}
	public void MoveDown(){
		foreach (GameObject player in players_in) {
			Move (player, false);

		}
		Invoke ("OpenDown", 1f);
		Invoke ("ReappearPlayers", 1.05f);
		Invoke ("CloseDown", 1.3f);


	}
	public void Move(GameObject player, bool mode_){
		MoveFloor2(player, mode_);
		players_moving.Add (player);
		player.GetComponent<PlayerScript> ().DisappearBody ();
		player.GetComponent<PlayerScript>().elevator_ready = false;
		player.GetComponent<PlayerScript>().is_touching_elevator = false;
		player.GetComponent<PlayerScript>().is_in_elevator = true;
	}
	public void MoveFloor(GameObject player, float distance, bool up){
		if(up) player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + distance,0f);
		else player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y - distance,0f);
	}
	public void MoveFloor2(GameObject player, bool up){
		if(up) player.transform.position = up_connection.transform.position;
		else player.transform.position = down_connection.transform.position;
	}
	public void ReappearPlayers(){

		foreach (GameObject player in players_moving) {
			player.GetComponent<PlayerScript> ().ReappearBody ();
			player.GetComponent<PlayerScript>().is_in_elevator = false;

		}
		players_moving.Clear ();
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
}
