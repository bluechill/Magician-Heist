using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
public class RopePlayerScript : PlayerScript {

	// Update is called once per frame
	void Update () {
		age = Time.time - birthtime;
		if (!started) {
			started = true;
			pointsText = GameObject.FindGameObjectWithTag ("Points Text 4").GetComponent<Text> ();
		}
		ProcessMovement ();
		ProcessRotation ();
		ProcessActions ();
		ProcessHold ();
		ProcessTransformed ();

		this.GetComponent<PlayerScript> ().Update ();

		if (is_ability && is_being_held)
		{
			if(!ability.GetComponent<Item>().thrown) transform.position =  Vector3.Lerp(transform.position, ability.transform.position, 0.1f);
		}
	}

	//	public override void UseAbility(){
	//
	//		is_ability = !is_ability;
	//
	//		if (is_ability) {
	//			body.SetActive (false);
	//			ability = MonoBehaviour.Instantiate (key_prefab);
	//
	//			animator.SetBool ("key ability", true);
	//			animator.SetBool ("ability", true);
	//			ability.transform.position = transform.position;
	//			transform.parent = ability.transform;
	//			ability.GetComponent<Item> ().current_player = this.gameObject;
	//
	//			Hide ();
	//		} else {
	//
	//			if (nearestActionObject && nearestActionObject.tag == "Door") {
	//				nearestActionObject.GetComponent<Door> ().Unlock ();
	//				nearestActionObject.GetComponent<Door> ().SwitchState ();
	//				is_ability = true;
	//				return;
	//			}
	//
	//
	//			body.SetActive (true);
	//			ability.SetActive (false);
	//
	//			animator.SetBool ("key ability", false);
	//			animator.SetBool ("ability", false);
	//            ability.GetComponent<Item>().enabled = true;
	//
	//			transform.position = new Vector3 (ability.transform.position.x, ability.transform.position.y + 0.5f, 0f);
	//			if (player_holding_me) {
	//				player_holding_key.GetComponent<PlayerScript> ().Drop ();
	//			}
	//			RemoveTouchingPlayers ();
	//			Reveal ();
	//			transform.parent = null;
	//			Destroy (ability.gameObject);
	//			ability = null;
	//
	//		}
	//
	//	}


}
