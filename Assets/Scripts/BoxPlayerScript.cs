using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
public class BoxPlayerScript : PlayerScript {


	public List<GameObject> players_in_box;

	public GameObject box_prefab;

	// Update is called once per frame
	void Update () {
		age = Time.time - birthtime;
		if (!started) {
			started = true;
			pointsText = GameObject.FindGameObjectWithTag ("Points Text 1").GetComponent<Text> ();
		}

		ProcessMovement ();
		ProcessRotation ();
		ProcessActions ();
		ProcessHold ();
		ProcessTransformed ();
		ProcessBox ();

		this.GetComponent<PlayerScript> ().Update ();
	}
	void ProcessBox(){
		if (is_ability) {
			ability.transform.position = transform.position;
		}
	}
//	public override void UseAbility(){
//		is_ability = !is_ability;
//
//		if (is_ability) {
//			this.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 0f));
//
//			body.SetActive (false);
//			ability = MonoBehaviour.Instantiate (box_prefab);
//			ability.transform.position = transform.position;
//			animator.SetBool ("ability", true);
//			ability.transform.parent = transform;
//			capsule.center = new Vector3(0f,0.4f, 0f);
//
//			Hide ();
//		} else {
//			body.SetActive (true);
//			animator.SetBool ("ability", false);
//			capsule.center = new Vector3(0f,-0.175f, 0f);
//			is_touching_box = false;
//			RemoveAllFromBox ();
//			Reveal ();
//			transform.parent = null;
//			Destroy (ability.gameObject);
//			ability = null;
//		}
//
//	}
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
