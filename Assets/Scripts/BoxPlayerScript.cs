using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
public class BoxPlayerScript : PlayerScript {


	public List<GameObject> players_in_box;


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
			animator.SetBool ("box ability", true);
			animator.SetBool ("ability", true);
			capsule.center = new Vector3 (0,0,0);
			Hide ();
		} else {
			body.SetActive (true);
			ability.SetActive (false);
			animator.SetBool ("box ability", false);
			animator.SetBool ("ability", false);
			capsule.center = new Vector3(0f,-0.175f, 0f);
			touching_box = null;
			is_touching_box = false;
			RemoveAllFromBox ();
			Reveal ();
		}

	}
	void RemoveAllFromBox(){
		List<GameObject> to_remove = new List<GameObject>();
		foreach (GameObject player in players_in_box) {
			to_remove.Add (player);
		}
		foreach (GameObject player in to_remove) {
			player.GetComponent<PlayerScript> ().ExitBox ();
		}
	}



}
