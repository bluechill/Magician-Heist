using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
public class KeyPlayerScript : PlayerScript {

	public List<GameObject> players_touching_key;
	public bool player_holding_me = false;
	public GameObject player_holding_key;
	// Update is called once per frame
	void Update () {
		age = Time.time - birthtime;

		TryInitializeController ();

		ProcessMovement ();
		ProcessRotation ();
		ProcessActions ();
		ProcessHold ();
		ProcessTransformed ();
	}

	public override void UseAbility(){
		is_ability = !is_ability;

		if (is_ability) {
			body.SetActive (false);
			ability.SetActive (true);
			animator.SetBool ("key ability", true);
			animator.SetBool ("ability", true);
			ability.transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);

			Hide ();
		} else {
			body.SetActive (true);
			ability.SetActive (false);
			animator.SetBool ("key ability", false);
			animator.SetBool ("ability", false);

			transform.position = new Vector3 (ability.transform.position.x, ability.transform.position.y + 0.5f, 0f);
			if (player_holding_me) {
				player_holding_key.GetComponent<PlayerScript> ().Drop ();
			}
			RemoveTouchingPlayers ();
			Reveal ();
		}

	}

	public void RemoveTouchingPlayers(){
		foreach (GameObject player in players_touching_key) {
			player.GetComponent<PlayerScript>().StopTouching (ability);
		}
	}

	public void DropMe(){
		player_holding_me = false;
		player_holding_key = null;
	}
	public void PickMeUp(GameObject player){
		player_holding_me = true;
		player_holding_key = player;

	}
}
