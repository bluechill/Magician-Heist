using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPlayerScript : PlayerScript {


	Animator animator;
	public AnimationClip walkClip;
	Rigidbody rb;
	public float vel;
	public float escalator_speed;
	bool right,left;
	public bool[] actions;
	int num_actions = 3;

	public int player_num;

	public KeyCode[][] key_mappings;

	public bool is_touching = false;

	public bool is_transformed = false;

	public int num_touching = 0;
	public List<GameObject> touching_objects;
	public GameObject transformed_object;
	public GameObject right_hand;

	public bool is_up_escalator = false;
	public GameObject touching_up_escalator;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
		capsule = GetComponent<CapsuleCollider> ();
		actions = new bool[num_actions];
		for (int i = 0; i < num_actions; i++) {
			actions[i] = false;
		}
		InitKeys ();
	}
	void InitKeys(){
		key_mappings = new KeyCode[3][];
		key_mappings [0] = new KeyCode[4];
		key_mappings [1] = new KeyCode[4];
		key_mappings [2] = new KeyCode[4];

		key_mappings [0][0] = KeyCode.W;
		key_mappings [0][1] = KeyCode.E;
		key_mappings [0][2] = KeyCode.R;
		key_mappings [0][3] = KeyCode.T;

		key_mappings [1][0] = KeyCode.S;
		key_mappings [1][1] = KeyCode.D;
		key_mappings [1][2] = KeyCode.F;
		key_mappings [1][3] = KeyCode.G;
	}
	// Update is called once per frame
	void Update () {
		ProcessMovement ();
		ProcessRotation ();
		ProcessActions ();
		ProcessHold ();
		ProcessTransformed ();
	}

	void ProcessMovement(){

		if (using_up_escalator) {
			rb.useGravity = false;
			float movement = Time.deltaTime * escalator_speed;
			transform.position = new Vector3 (transform.position.x + movement, transform.position.y, 2f);
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
		if (!IsGrounded ()) {
			rb.velocity += Vector3.down * vel;
		}
	}
	void ProcessRotation(){

		if (rb.velocity.x < -0.05f)
			this.transform.rotation = Quaternion.Euler(new Vector3(0f,180f,0f));
		else if (rb.velocity.x > 0.05f)
			this.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));

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

		if (is_in_box) {
			ExitBox ();
			return;
		}

		if (is_touching_box) {
			EnterBox ();
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


	void Animate(){
		animator.speed = 2f * rb.velocity.magnitude;
		animator.SetBool ("idle", false);

	}
	void Idle(){
		animator.speed = 0.4f;
		animator.SetBool ("idle", true);

	}

	void PickUp(){
		if (num_touching == 0 || is_holding)
			return;
		held_object = touching_objects [0];
		is_holding = true;
		StopTouching (held_object);
		if (held_object.GetComponent<Item> ().is_player) {
			held_object.GetComponent<Item> ().current_player.GetComponent<PlayerScript> ().GetPickedUp (this.gameObject);
		}
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
			animator.SetBool ("box ability", true);
			animator.SetBool ("ability", true);
			capsule.center = new Vector3 (0,0,0);
		} else {
			body.SetActive (true);
			ability.SetActive (false);
			animator.SetBool ("box ability", false);
			animator.SetBool ("ability", false);
			capsule.center = new Vector3(0f,-0.175f, 0f);
			if (is_being_held) {
				print ("error??");
			}
			is_touching_box = false;
		}

	}
	void TransformIntoItem(){
		if (held_object.GetComponent<Item> ().is_player) {
			return;
		}
		body.SetActive (false);
		is_transformed = true;
		held_object.GetComponent<Item> ().SetPlayer (this.gameObject, player_num);
		TransformDrop ();
	}
	void RevertBack(){
		body.SetActive (true);
		is_transformed = false;
		if (is_being_held) {
			being_held_by.GetComponent<PlayerScript> ().Drop ();
		}
		held_object.GetComponent<Item> ().ResetPlayer ();
		transform.position = new Vector3 (transform.position.x, transform.position.y + 0.5f, 0f);
		PickUp ();
	}
	void TransformDrop(){
		is_holding = false;
	}
	bool IsGrounded(){
		RaycastHit hit, hit2;

		Vector3 extents = GetComponent<Collider> ().bounds.extents;

		Vector3 posLeft = transform.position - new Vector3(extents.x, 0f,0f);
		Vector3 posRight = transform.position + new Vector3(extents.x, 0f,0f);

		int layer = 1 << LayerMask.NameToLayer ("Default");

		Physics.Raycast(posLeft, -Vector3.up, out hit, 1.1f, layer);
		Physics.Raycast(posRight, -Vector3.up, out hit2, 1.1f, layer);

		if (hit.collider != null || hit2.collider != null) {
			//ray hit an environment object

			float distance = Mathf.Abs(hit.point.y - transform.position.y);
			return true;
		}
		return false;
	}

}
