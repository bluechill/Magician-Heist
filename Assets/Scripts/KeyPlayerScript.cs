using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
public class KeyPlayerScript : PlayerScript {

	public float action_cooldown = 0.5f;
	public bool cooldown = false;

	public float vel;
	public float escalator_speed;

	public int player_num;

	public bool is_touching = false;

	public int num_touching = 0;
	public List<GameObject> touching_objects;
	public GameObject transformed_object;
	public GameObject right_hand;

	public bool is_up_escalator = false;
	public GameObject touching_up_escalator;


	public List<GameObject> players_in_box;

	public bool keyboard_user;
	InputDevice controller;
	bool controller_set = false;

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
	//initialize a controller to this magician
	void TryInitializeController(){
		if (InputManager.Devices.Count > player_num) {
			controller = InputManager.Devices [player_num];
			controller_set = true;
		}
	}
	//uses InControl as InputManager
	void ProcessInputController(){
			rb.AddForce(Vector3.right * controller.LeftStickX * vel);
			if (controller.LeftStickX < 0) {
				left = true;
				right = false;
			}
			else if (controller.LeftStickX > 0) {
				right = true;
				left = false;
			}
			else {
				right = false;
				left = false;
			}

		if (controller.Action1) {
			if (!cooldown) {
				actions [0] = true;
				cooldown = true;
				Invoke ("ActionCooldown", action_cooldown);
			}
			return;
		}
		if (controller.Action2) {
			if (!cooldown) {
				actions [1] = true;
				cooldown = true;
				Invoke ("ActionCooldown", action_cooldown);
			}
			return;
		}

	}
	void Animate(){
		print ("animate");
		animator.speed = 1f;
		animator.SetBool ("idle", false);

	}
	void Idle(){
		print ("Idle");
		animator.speed = 1f;
		animator.SetBool ("idle", true);

	}

	void PickUp(){
		if (num_touching == 0 || is_holding)
			return;
		held_object = touching_objects [0];
		is_holding = true;
		if (held_object.GetComponent<Item> ().briefcase) {
			is_holding_briefcase = true;
		}
		StopTouching (held_object);
		if (held_object.GetComponent<Item> ().is_player) {
			held_object.GetComponent<Item> ().current_player.GetComponent<PlayerScript> ().GetPickedUp (this.gameObject);
		}
	}
	public void TransformIntoItem(){
		if (held_object.GetComponent<Item> ().is_player) {
			return;
		}
		body.SetActive (false);
		is_transformed = true;
		Hide ();
		held_object.GetComponent<Item> ().SetPlayer (this.gameObject, player_num);
		TransformDrop ();
	}
	public void RevertBack(){
		body.SetActive (true);
		is_transformed = false;
		Reveal ();
		if (is_being_held) {
			being_held_by.GetComponent<PlayerScript> ().Drop ();
		}
		held_object.GetComponent<Item> ().ResetPlayer ();
		transform.position = new Vector3 (transform.position.x, transform.position.y + 0.5f, 0f);
		PickUp ();
	}

	void ProcessMovement(){
		if (is_knocked_out) {
			rb.velocity = Vector3.down * 5f;
			print (player_num + "knockout idle");
			Idle ();
			return;
		}
		if (using_up_escalator) {
			rb.useGravity = false;
			float movement = Time.deltaTime * escalator_speed;
			transform.position = new Vector3 (transform.position.x + movement, transform.position.y, -1f);
			return;
		}
		if (is_using_stairs) {
			rb.velocity = Vector3.zero;
			return;
		}
		if (Input.GetKeyDown(key_mappings[player_num][0])) {
			left = true;
		}
		if (Input.GetKeyUp(key_mappings[player_num][0])) {
			left = false;
		}
		if (Input.GetKeyDown(key_mappings[player_num][1])) {
			right = true;
		} 		
		if (Input.GetKeyUp(key_mappings[player_num][1])) {
			right = false;
		}
		if (Input.GetKeyDown(key_mappings[player_num][2])) {
			actions [0] = true;
		}
		if (Input.GetKeyDown(key_mappings[player_num][3])) {
			actions [1] = true;
		}

		if (is_in_box) {
			rb.velocity = Vector3.zero;
			transform.position = inside_box.transform.position;
			return;
		}
		if (controller_set) {
			ProcessInputController ();
		}
		if (left) {

			rb.velocity = Vector3.left * vel;
			if (is_ability) {
				rb.velocity /= 3;
				Reveal ();
			}
			Animate ();
		}
		if (right) {
			rb.velocity = Vector3.right * vel;
			if (is_ability) {
				rb.velocity /= 3;
				Reveal ();
			}
			Animate ();
		}
		if (right && left) {
			rb.velocity = Vector3.zero;
			if (is_ability) {
				Hide ();
			}
			print (player_num + "right left idle");
			Idle ();
		}
		if (!right && !left) {
			rb.velocity = Vector3.zero;
			if (is_ability) {
				Hide ();
			}
			print (player_num + "!right !left idle");
			Idle ();
		}

		if (!IsGrounded ()) {
			rb.velocity += Vector3.down * vel;
		}
	}
	void ProcessRotation(){
		if (is_ability)
			return;
		if (rb.velocity.x < -0.05f) {
			this.transform.rotation = Quaternion.Euler (new Vector3 (0f, 180f, 0f));
			if (is_holding && held_object.GetComponent<Item>().flash_light) {
				held_object.transform.rotation = Quaternion.Euler (new Vector3 (0f, 180f, 270f));
			}
		} else if (rb.velocity.x > 0.05f) {
			this.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 0f));
			if (is_holding && held_object.GetComponent<Item>().flash_light) {
				held_object.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 270f));
			}
		}

	}
	void ProcessActions(){
		if (actions [0]) {
			ProcessAction1 ();
		}
		if (actions [1]) {
			ProcessAction2 ();
		}
		if (actions [2]) {
			ProcessAction3 ();
		}
	}
	void ProcessAction1(){
		actions [0] = false;

		if (is_transformed || is_ability) {
			return;
		}

		if (is_up_escalator) {
			touching_up_escalator.GetComponentInParent<UpEscalator> ().UseEscalator ();
			return;
		}
		if (is_touching_up_stairs) {
			UseUpStairs ();
			return;
		}
		if (is_touching_down_stairs) {
			UseDownStairs ();
			return;
		}
		if (is_in_box) {
			ExitBox ();
			return;
		}

		if (is_touching_box) {
			EnterBox ();
			return;
		}
		if (is_touching_door) {
			OpenDoor ();
			return;
		}

		if (is_holding) {
			Drop ();
		} else {
			PickUp ();
		}

	}
	void ProcessAction2(){
		actions [1] = false;
		if (is_holding && !is_transformed) {
			TransformIntoItem ();
		} else if (!is_transformed) {
			UseAbility ();
		} else if (is_transformed) {
			RevertBack ();
		}

	}
	void ProcessAction3(){
		actions [2] = false;
	}


	void ProcessHold(){
		if (!is_holding) {
			return;
		}

		held_object.transform.position = right_hand.transform.position;

	}
	void ProcessTransformed(){
		if (!is_transformed) {
			return;
		}
		transform.position = held_object.transform.position;
	}
		
	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.tag == "Item") {
			StartTouching (coll.gameObject);
		} else if (coll.gameObject.tag == "Up Escalator Entrance") {
			coll.gameObject.GetComponentInParent<UpEscalator> ().AddPlayer (this.gameObject);
			is_up_escalator = true;
			touching_up_escalator = coll.gameObject;
		} else if (coll.gameObject.tag == "Up Escalator Exit") {
			using_up_escalator = false;
			rb.useGravity = true;
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);
			coll.gameObject.GetComponentInParent<UpEscalator> ().num_using--;
		} else if (coll.gameObject.tag == "Magical Box") {
			print ("touching box");
			TouchBox (coll.gameObject);
		}   else if (coll.gameObject.tag == "Up Stairs") {
			is_touching_up_stairs = true;
			touching_stairs = coll.gameObject;
			touching_stairs.GetComponent<Stairs> ().activated = true;

		} else if (coll.gameObject.tag == "Down Stairs") {
			is_touching_down_stairs = true;
			touching_stairs = coll.gameObject;
			touching_stairs.GetComponent<Stairs> ().activated = true;
		} else if (coll.gameObject.tag == "Door") {
			is_touching_door = true;
			touching_door = coll.gameObject;
		} else if (coll.gameObject.tag == "Finish") {
			if (is_holding_briefcase) {
				print ("win!");
				Win ();
			}
		}
	}
	void OnTriggerExit(Collider coll){
		if (coll.gameObject.tag == "Item") {
			StopTouching (coll.gameObject);
		} else if (coll.gameObject.tag == "Up Escalator Entrance") {
			coll.gameObject.GetComponentInParent<UpEscalator> ().RemovePlayer (this.gameObject);
			is_up_escalator = false;
			touching_up_escalator = null;
		} else if (coll.gameObject.tag == "Magical Box") {
			StopTouchBox (coll.gameObject);
		}  else if (coll.gameObject.tag == "Up Stairs") {
			is_touching_up_stairs = false;
			touching_stairs.GetComponent<Stairs> ().activated = false;
			touching_stairs = null;

		} else if (coll.gameObject.tag == "Down Stairs") {
			is_touching_down_stairs = false;
			touching_stairs.GetComponent<Stairs> ().activated = false;
			touching_stairs = null;

		}  else if (coll.gameObject.tag == "Door") {
			is_touching_door = false;
			touching_door = null;
		}
	}
	void StartTouching(GameObject obj){
		if (obj == held_object)
			return;
		
		is_touching = true;
		touching_objects.Add (obj);
		num_touching++;
	}
	void StopTouching(GameObject obj){
		if (obj == held_object)
			return;
		
		num_touching--;
		touching_objects.Remove (obj);
		if (num_touching == 0) {
			is_touching = false;
		} 
	}

	public void UseAbility(){
		is_ability = !is_ability;

		if (is_ability) {
			body.SetActive (false);
			ability.SetActive (true);
			animator.SetBool ("key ability", true);
			animator.SetBool ("ability", true);
			Hide ();
		} else {
			body.SetActive (true);
			ability.SetActive (false);
			animator.SetBool ("key ability", false);
			animator.SetBool ("ability", false);
			Reveal ();
		}

	}

	public void ActionCooldown(){
		cooldown = false;
	}



}
