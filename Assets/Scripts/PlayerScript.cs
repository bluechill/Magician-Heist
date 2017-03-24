using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class PlayerScript : MonoBehaviour {

	public float action_cooldown = 0.5f;
	public bool cooldown = false;

	public float vel;
	public int player_num;

	public bool is_touching = false;
	public int num_touching = 0;
	public GameObject nearestActionObject = null;
	public GameObject transformed_object;
	public GameObject right_hand;

	public bool keyboard_user;
	public InputDevice controller;
	public bool controller_set = false;


	public Animator animator;
	public Rigidbody rb;

	public float life_time;
	public float age;
	public float birthtime;

	public bool[] actions;
	public int num_actions = 3;

	public KeyCode[][] key_mappings;

	public bool right,left, up, down;
	public bool is_hiding = false;
	public bool is_in_box = false;
	public bool is_holding = false;
	public GameObject held_object;
	public bool is_being_held = false;
	public GameObject being_held_by;
	public bool is_touching_box = false;
	public bool is_touching_door = false;
	public bool is_touching_elevator = false;
	public bool is_touching_stairs = false;
	public bool is_in_elevator = false;
	public bool elevator_ready = false;
	public bool is_holding_briefcase = false;
	public GameObject inside_box;

	public GameObject ability;
	public GameObject body;
	public CapsuleCollider capsule;
	public bool is_ability = false;
	public bool is_transformed = false;
	public bool is_knocked_out = false;
    public bool is_holding_key = false;
    public bool is_holding_key_card = false;
    public bool is_touching_closet = false;
	public bool is_in_closet = false;

    public bool grounded = false;
	public bool is_touching_desk_overlap = false;
	public GameObject touching_desk_overlap;

	public void Start(){
		birthtime = Time.time;
		rb = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
		capsule = GetComponent<CapsuleCollider> ();
		InitKeys ();
		actions = new bool[num_actions];
		for (int i = 0; i < num_actions; i++) {
			actions[i] = false;
		}

	}

	public void Drop(){
		if (!is_holding)
			return;
		if (held_object.GetComponent<Item> ().briefcase) {
			is_holding_briefcase = false;
		}
        if (held_object.GetComponent<Item>().magical_key)
        {
            is_holding_key = false;
            held_object.GetComponentInChildren<KeyPlayerScript>().DropMe();
        }
        if (held_object.GetComponent<Item>().key_card)
        {
            is_holding_key_card = false;
        }
        if (is_touching_desk_overlap) {
			held_object.GetComponent<Item> ().enabled = false;
			held_object.transform.position = new Vector3(touching_desk_overlap.transform.position.x, touching_desk_overlap.transform.position.y + 0.5f, 0f);
		}
		held_object.GetComponent<Item> ().held = false;
        if (rb.velocity.x > 0f)
        {
            print("throw");
            held_object.GetComponent<Item>().thrown = true;
            held_object.GetComponent<Rigidbody>().isKinematic = false;
            held_object.GetComponent<Rigidbody>().velocity = ((Vector3.right + (Vector3.up*0.5f)) * 10f);

        }
        else if (rb.velocity.x < 0f)
        {
            held_object.GetComponent<Item>().thrown = true;
            held_object.GetComponent<Rigidbody>().isKinematic = false;
            held_object.GetComponent<Rigidbody>().velocity = ((Vector3.left + (Vector3.up * 0.5f)) * 10f);
        }
		held_object.transform.position = new Vector3(held_object.transform.position.x, held_object.transform.position.y, 0f );
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
	public void TouchBox(GameObject box){
		is_touching_box = true;
	}
	public void StopTouchBox(GameObject box){
		is_touching_box = false;
	}

	public void TransformDrop(){
		is_holding = false;
	}
	public bool IsGrounded(){
		RaycastHit hit, hit2;

		Vector3 extents = GetComponent<Collider> ().bounds.extents;

		Vector3 posLeft = transform.position - new Vector3(extents.x, 0f,0f);
		Vector3 posRight = transform.position + new Vector3(extents.x, 0f,0f);

		int layer = 1 << LayerMask.NameToLayer ("Default");

		Physics.Raycast(posLeft, -Vector3.up, out hit, 1.1f, layer);
		Physics.Raycast(posRight, -Vector3.up, out hit2, 1.1f, layer);

		if (hit.collider != null || hit2.collider != null) {
			//ray hit an environment object

			//float distance = Mathf.Abs(hit.point.y - transform.position.y);
			return true;
		}
		return false;
	}
	public bool IsBoxGrounded(){
		RaycastHit hit, hit2;

		Vector3 extents = ability.GetComponentInChildren<Collider> ().bounds.extents;

		Vector3 posLeft = transform.position - new Vector3(extents.x, 0f,0f);
		Vector3 posRight = transform.position + new Vector3(extents.x, 0f,0f);

		int layer = 1 << LayerMask.NameToLayer ("Default");

		Physics.Raycast(posLeft, -Vector3.up, out hit, 0.5f, layer);
		Physics.Raycast(posRight, -Vector3.up, out hit2, 0.5f, layer);

		if (hit.collider != null || hit2.collider != null) {
			//ray hit an environment object

			//float distance = Mathf.Abs(hit.point.y - transform.position.y);
			return true;
		}
		return false;
	}

	void InitKeys(){
		key_mappings = new KeyCode[3][];
		key_mappings [0] = new KeyCode[6];
		key_mappings [1] = new KeyCode[6];
		key_mappings [2] = new KeyCode[6];

		key_mappings [0][0] = KeyCode.A;
		key_mappings [0][1] = KeyCode.D;
		key_mappings [0][2] = KeyCode.W;
		key_mappings [0][3] = KeyCode.S;
		key_mappings [0][4] = KeyCode.E;
		key_mappings [0][5] = KeyCode.R;

		key_mappings [1][0] = KeyCode.H;
		key_mappings [1][1] = KeyCode.K;
		key_mappings [1][2] = KeyCode.U;
		key_mappings [1][3] = KeyCode.J;
		key_mappings [1][4] = KeyCode.I;
		key_mappings [1][5] = KeyCode.O;

	}

	public void EnterBox(){
		is_hiding = true;
		is_in_box = true;
		body.SetActive (false);
		inside_box = nearestActionObject;
		inside_box.GetComponentInParent<BoxPlayerScript> ().players_in_box.Add (this.gameObject);
		StopTouchBox (nearestActionObject);
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

	public void OpenDoor()
	{
		nearestActionObject.GetComponentInChildren<Door> ().SwitchState (is_holding_key);
	}
	public void SwitchElevator(){
		nearestActionObject.GetComponentInChildren<Elevator> ().SwitchState (this.gameObject);
		elevator_ready = is_in_elevator && !nearestActionObject.GetComponentInChildren<Elevator> ().open;

	}
	public void UseElevator(){
		nearestActionObject.GetComponentInChildren<Elevator> ().Use ();
	}
	public void KnockOut(){
		is_knocked_out = true;
		animator.SetBool ("knockout", true);
		animator.Play ("Knockout");
		right = false;
		left = false;
		Hide ();
		Invoke ("WakeupAnimation", 4f);
		Invoke ("Wakeup", 5f);
	}
	public void WakeupAnimation(){
		animator.SetBool ("knockout", false);
	}
	public void Wakeup(){
		is_knocked_out = false;
		Reveal ();
	}
	public void Hide(){
		is_hiding = true;
	}	
	public void Reveal(){
		is_hiding = false;
	}


	public void ActionCooldown(){
		cooldown = false;
	}

	public virtual void UseAbility(){

	}


	//initialize a controller to this magician
	public void TryInitializeController(){
		if (InputManager.Devices.Count > player_num) {
			controller = InputManager.Devices [player_num];
			controller_set = true;
		}
	}
	//uses InControl as InputManager
	public void ProcessInputController(){
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
		if (controller.DPadDown) {
			down = true;
		} else if (controller.DPadUp) {
			up = true;
		} else {
			down = false;
			up = false;
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
	public void Animate(){
		if (is_in_elevator || is_in_closet) {
			return;
		}
		//print ("animate");
		animator.speed = 1f;
		animator.SetBool ("idle", false);

	}
	public void Idle(){
		//print ("Idle");
		animator.speed = 1f;
		animator.SetBool ("idle", true);

	}
	public void AbilityAnimate(){

		ability.GetComponentInChildren<Animator>().SetBool ("idle", false);

	}
	public void AbilityIdle(){

		ability.GetComponentInChildren<Animator>().SetBool ("idle", true);

	}

	public void PickUp()
	{
		if (nearestActionObject == null)
			return;
		
		held_object = nearestActionObject;
		is_holding = true;
		if (held_object.GetComponent<Item> ().briefcase) {
			is_holding_briefcase = true;
		}

		if (held_object.GetComponent<Item> ().is_player) {
			held_object.GetComponent<Item> ().current_player.GetComponent<PlayerScript> ().GetPickedUp (this.gameObject);
		}
		if (held_object.GetComponent<Item> ().magical_key) {
			is_holding_key = true;
			held_object.GetComponentInChildren<KeyPlayerScript> ().PickMeUp (this.gameObject);
		}
        if (held_object.GetComponent<Item>().key_card)
        {
            is_holding_key_card = true;
        }
        held_object.GetComponent<Item>().thrown = false;
        held_object.GetComponent<Item> ().held = true;
		held_object.GetComponent<Item> ().enabled = true;

		nearestActionObject.GetComponent<SpriteGlow> ().enabled = false;
		nearestActionObject = null;
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

	public void ProcessMovement(){
		if (is_knocked_out) {
			rb.velocity = Vector3.down * 5f;
			//print (player_num + "knockout idle");
			Idle ();
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
			up = true;
		}
		if (Input.GetKeyUp(key_mappings[player_num][2])) {
			up = false;
		}
		if (Input.GetKeyDown(key_mappings[player_num][3])) {
			down = true;
		} 		
		if (Input.GetKeyUp(key_mappings[player_num][3])) {
			down = false;
		}


		if (Input.GetKeyDown(key_mappings[player_num][4])) {
			actions [0] = true;
		}
		if (Input.GetKeyDown(key_mappings[player_num][5])) {
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
		if (is_ability && player_num == 1) {
			return;
		}

		if ((up || down) && is_touching_stairs && !cooldown) {
			is_touching_stairs = false;
			nearestActionObject.GetComponent<Stairs> ().Use (this.gameObject);
			cooldown = true;
			Invoke ("ActionCooldown", action_cooldown);
		}

		if (left) {

			rb.velocity = Vector3.left * vel;

			Animate ();
		}
		if (right) {
			rb.velocity = Vector3.right * vel;

			Animate ();
		}
		if (right && left) {
			rb.velocity = Vector3.zero;
			Idle ();
		}
		if (!right && !left) {
			rb.velocity = Vector3.zero;
			Idle ();
		}
		if (is_in_elevator || is_in_closet) {
			rb.velocity = Vector3.zero;
			return;
		}

		if (is_ability && player_num == 0)
			grounded = IsBoxGrounded ();
		else grounded = IsGrounded();

		if (!grounded) {
			rb.velocity = Vector3.down * vel;
		}
		else if (is_ability && player_num == 0) {	
			if (rb.velocity.magnitude > 0f) {
				//box is moving
				AbilityAnimate();
			} else {
				AbilityIdle ();
			}
			rb.velocity /= 3;
		}

	}
	public void ProcessRotation(){

		if (rb.velocity.x < -0.05f) {
			this.transform.rotation = Quaternion.Euler (new Vector3 (0f, 180f, 0f));
			if (is_holding && held_object.GetComponent<Item>().flash_light) {
				held_object.transform.rotation = Quaternion.Euler (new Vector3 (0f, 180f, 270f));
			}else if(is_holding && held_object){

			}
		} else if (rb.velocity.x > 0.05f) {
			this.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 0f));
			if (is_holding && held_object.GetComponent<Item>().flash_light) {
				held_object.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 270f));
			} else if(is_holding && held_object){

			}
		}

	}
	public void ProcessActions(){
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
	public void ProcessAction1(){
		actions [0] = false;

		if (is_transformed || is_ability) {
			return;
		}

		if (is_in_box) {
			ExitBox ();
			return;
		}

		if (nearestActionObject != null) {
			if (nearestActionObject.tag == "Magical Box")
				EnterBox ();
			else if (nearestActionObject.tag == "Door")
				OpenDoor ();
			else if (nearestActionObject.tag == "Elevator")
				SwitchElevator ();
			else if (nearestActionObject.tag == "Closet")
				UseCloset ();
			else
				PickUp ();
		} else if (is_holding) {
			Drop ();
		}
//
//
//		if (is_holding)
//			Drop ();
//		else
//
//		if (is_touching_box) {
//			EnterBox ();
//			return;
//		}
//		if (is_touching_door) {
//			OpenDoor ();
//			return;
//		}
//		if (is_touching_elevator) {
//			SwitchElevator ();
//			return;
//		}
//		if (is_touching_closet) {
//			UseCloset ();
//			return;
//		}
//		if (is_holding) {
//			Drop ();
//		} else {
//			PickUp ();
//		}

	}
	public void ProcessAction2(){
		actions [1] = false;
		if (is_holding && !is_transformed) {
			TransformIntoItem ();
		} else if(is_touching_closet && nearestActionObject.GetComponent<Closet>().open){
			DisappearBody ();
			EnterCloset ();
		} else if(nearestActionObject && nearestActionObject.tag == "Elevator" && !is_in_elevator && nearestActionObject.GetComponent<Elevator>().open){
			DisappearBody ();
			EnterElevator ();
		}  else if(is_in_elevator && nearestActionObject.GetComponent<Elevator>().open){
			ReappearBody ();
			ExitElevator ();
		}   else if(is_in_elevator && !nearestActionObject.GetComponent<Elevator>().open && elevator_ready){
			UseElevator ();
		} else if(is_in_closet){
			return;
		} else if (!is_transformed) {
			UseAbility ();
		} else if (is_transformed) {
			RevertBack ();
		}
	}
	public void ProcessAction3(){
		actions [2] = false;
	}


	public void ProcessHold(){
		if (!is_holding) {
			return;
		}

		held_object.transform.position = right_hand.transform.position;

	}
	public void ProcessTransformed(){
		if (!is_transformed) {
			return;
		}
		transform.position = held_object.transform.position;
	}

	public void OnTriggerEnter(Collider coll){
		if (coll.gameObject.tag == "Item") {

		} else if (coll.gameObject.tag == "Exit") {
			if (is_holding_briefcase) {
				print ("win!");
				Game.GameInstance.Win ();
			} else if(held_object && held_object.GetComponent<Item>().gold_bar){
				Game.GameInstance.gold_bars += 1;
				held_object.GetComponent<Item> ().gold_bar = false;
			}
		}
        else if (coll.gameObject.tag == "Guard")
        {
            if(!is_knocked_out && !is_hiding && !coll.gameObject.GetComponent<StatePatternEnemy>().knockout && !is_ability)  KnockOut();
        }
    }

//	public void OnTriggerExit(Collider coll){
//		if (coll.gameObject.tag == "Item") {
//			if (coll.gameObject.GetComponent<Item> ().magical_key && player_num != 1) {
//				coll.gameObject.GetComponentInParent<KeyPlayerScript> ().players_touching_key.Remove (this.gameObject);
//			}
//		} else if (coll.gameObject.tag == "Magical Box") {
//			StopTouchBox (coll.gameObject);
//		} else if (coll.gameObject.tag == "Door") {
//			is_touching_door = false;
//			touching_door = null;
//		}  else if (coll.gameObject.tag == "Elevator") {
//			is_touching_elevator = false;
//			touching_elevator = null;
//			coll.gameObject.GetComponent<Elevator> ().GetOut (this.gameObject);
//		}  else if (coll.gameObject.tag == "Stairs") {
//			is_touching_stairs = false;
//			touching_stairs = null;
//		}  else if (coll.gameObject.tag == "Closet") {
//			is_touching_closet = false;
//			touching_closet = null;
//		}  else if (coll.gameObject.tag == "Desk Overlap") {
//			is_touching_desk_overlap = false;
//			touching_desk_overlap = null;
//		} 
//	}
		
	public void DisappearBody(){
		body.SetActive (false);
		if (is_holding) {
			held_object.SetActive (false);
		}
	}
	public void ReappearBody(){
		body.SetActive (true);
		if (is_holding) {
			held_object.SetActive (true);
		}
	}

	public void EnterElevator(){
		is_in_elevator = true;
		nearestActionObject.GetComponent<Elevator> ().GetIn (this.gameObject);
	}

	public void ExitElevator(){
		is_in_elevator = false;
		nearestActionObject.GetComponent<Elevator> ().GetOut (this.gameObject);
	}

	public void UseCloset(){
		nearestActionObject.GetComponent<Closet> ().SwitchStates ();
	}
	public void EnterCloset(){
		is_in_closet = true;
		nearestActionObject.GetComponent<Closet> ().EnterCloset (this.gameObject);
	}
	public void ExitCloset(){
		is_in_closet = false;
		is_touching_closet = false;
		nearestActionObject = null;
		ReappearBody ();
	}

	private void FindNearestItem() {
		if (nearestActionObject != null)
			nearestActionObject.GetComponent<SpriteGlow> ().enabled = false;
		nearestActionObject = null;

		float boxWidth = 0.5f;
		float sign = 1.0f;

		if (this.transform.localEulerAngles.y >= 180f)
			sign *= -1f;

		Collider[] colliders = Physics.OverlapBox (this.transform.position + new Vector3(boxWidth / 2f * sign, 0f, 0f), new Vector3 (boxWidth, 1.0f, 1.0f));

		if (colliders.Length == 0)
			return;

		List<Collider> taggedColliders = new List<Collider> ();

		for (int i = 0; i < colliders.Length; ++i) {
			if (colliders[i].GetComponent<SpriteGlow>() != null &&
				!colliders[i].transform.IsChildOf(this.transform) &&
				!(colliders[i].tag == "Item" && is_holding))
				taggedColliders.Add (colliders [i]);
		}

		if (taggedColliders.Count == 0)
			return;

		Collider closest = taggedColliders [0];
		float zeroDistance = Vector3.Distance (taggedColliders [0].ClosestPointOnBounds (this.transform.position), this.transform.position);

		for (int i = 1; i < taggedColliders.Count; ++i) {
			float distance = Vector3.Distance (taggedColliders [1].ClosestPointOnBounds (this.transform.position), this.transform.position);
			if (distance < zeroDistance) {
				closest = colliders [i];
				zeroDistance = distance;
			}
		}

		if (closest.gameObject.GetComponent<SpriteGlow> () != null) {
			nearestActionObject = closest.gameObject;
			nearestActionObject.GetComponent<SpriteGlow> ().enabled = true;
		}
	}

	void FixedUpdate() {
		FindNearestItem ();
	}
}