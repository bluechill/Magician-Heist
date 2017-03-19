using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

	public bool open = false;
	public GameObject door_open;
	public List<GameObject> players_in;
	public bool has_players = false;
	bool mode = false;
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
	public void SwitchState(){
		open = !open;
	}
	public void GetIn(GameObject player){
		players_in.Add (player);
	}
	public void GetOut(GameObject player){
		players_in.Remove (player);
	}
	public void Use(bool up, GameObject player){
		if (!open)
			return;
		open = false;
		mode = up;
		Invoke ("Use2", 1f);
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
			MoveFloor (player, 7f, true); 
		}
	}
	public void MoveDown(){
		foreach (GameObject player in players_in) {
			MoveFloor (player, 7f, false); 
		}
	}
	public void MoveFloor(GameObject player, float distance, bool up){
		if(up) player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + distance,0f);
		else player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y - distance,0f);
	}

}
