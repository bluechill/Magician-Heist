using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
public class Magician : Character {

	//members used for controller input/movement
	int num_buttons = 4;
	bool action_button1;
	bool action_button2;
	bool[] directions;

	//members used for closet manipulation
	bool touching_closet = false;
	GameObject closet;
	bool in_closet = false;
	bool hiding = false;

	//members for using magical ability
	public GameObject ability_object;
	public bool using_ability = false;

	//members for interacting with magical box
	public bool touching_box = false;
	GameObject box;
	public bool in_box = false;

	//inspector variables
	public int player_num;
	public bool keyboard_user;
	InputDevice controller;
	bool controller_set = false;
	KeyCode[][] KeyboardControls;
	int num_players = 4;

	// Use this for initialization
	public override void Start () {
		//run base class initializer
		base.Start ();
		directions = new bool[2];
		directions [0] = false;
		directions [1] = false;
		action_button1 = false;
		action_button2 = false;
		//current button mapping:
		// 0 :  move left 
		// 1 :  move right 
		// 2 :  action button 
		InitKeyboardControls();
	}
	
	// Update is called once per frame
	void Update () {

		//always check for a plugged in controller to assign to this player
		// TODO implement a character select screen and change this
		TryInitializeController ();

		if(controller_set) ProcessInputController ();
		
		if(keyboard_user) ProcessInputKeyboard ();
		ProcessMovement ();
		ProcessVelocityAnimation ();
		ProcessAction1 ();
		ProcessAction2 ();
	}
	//initialize keyboard control 2d array
	void InitKeyboardControls(){
		KeyboardControls = new KeyCode[num_players][];

		for (int i = 0; i < num_players; i++) {
			KeyboardControls [i] = new KeyCode[num_buttons];
			for (int j = 0; j < num_buttons; j++) {
				KeyboardControls [i] [j] = KeyCode.X;
			}
		}
		// players x buttons keyboard control map initialized to X
		//KeyboardControls[player_num][button_index] 
		//ONLY SET UP FOR TWO PLAYERS ON ONE KEYBOARD, SORRY, TOO MANY BUTTONS!
		KeyboardControls[0][0] = KeyCode.A;
		KeyboardControls[0][1] = KeyCode.S;
		KeyboardControls[0][2] = KeyCode.D;
		KeyboardControls[0][3] = KeyCode.F;

		KeyboardControls[1][0] = KeyCode.J;
		KeyboardControls[1][1] = KeyCode.K;
		KeyboardControls[1][2] = KeyCode.L;
		KeyboardControls[1][3] = KeyCode.Semicolon;

	}

	//process player input from the keyboard
	//DOES NOT SUPPORT MULTIPLE MAGICIANS
	void ProcessInputKeyboard(){
		//uses computer keys to allow easier beginning development for other members of the group. 
		if (Input.GetKeyDown (KeyboardControls[player_num][0])) {
			directions [0] = true;
		} 
		if (Input.GetKeyUp (KeyboardControls[player_num][0])) {
			directions [0] = false;
		}
		if (Input.GetKeyDown (KeyboardControls[player_num][1])) {
			directions [1] = true;
		} 
		if (Input.GetKeyUp (KeyboardControls[player_num][1])) {
			directions [1] = false;
		}
		if (Input.GetKeyDown (KeyboardControls[player_num][2])) {
			action_button1 = true;
		}
		if (Input.GetKeyDown (KeyboardControls[player_num][3])) {
			action_button2 = true;
		}
	}
	//initialize a controller to this magician
	void TryInitializeController(){
		if (InputManager.Devices.Count > player_num) {
			controller = InputManager.Devices [player_num];
			controller_set = true;
		}
	}
	//processes input from the Magician's controller 
	//uses InControl as InputManager
	void ProcessInputController(){
		rb.AddForce (Vector3.right * controller.LeftStickX * movement_velocity);
		if (controller.LeftStickX < 0) {
			directions [0] = true;
			directions [1] = false;
		} else if (controller.LeftStickX > 0) {
			directions [1] = true;
			directions [0] = false;
		} else {
			directions [0] = false;
			directions [1] = false;
		}
	}
	void ProcessMovement(){
		//move left
		if (directions [0]) {
			rb.velocity = Vector3.left * movement_velocity;
		} 
		//move right
		else if (directions [1]) {
			rb.velocity = Vector3.right * movement_velocity;
		} 
		//dont move
		else {
			rb.velocity = Vector3.zero;
		}

		if (in_closet || in_box)
			rb.velocity = Vector3.zero;
	}
	//function to process pressing the first action button 
	void ProcessAction1(){

		//if X button has been pressed 
		if (action_button1) {

			print ("action1");

			if (touching_closet) {
				closet.GetComponent<Closet> ().Action1 ();
			}
		}
		//set back the button to false by default to 
		// only process action once per button press 
		action_button1 = false;
	}
	//function to process pressing the second action button
	void ProcessAction2(){

		//if X button has been pressed 
		if (action_button2) {

			print ("action2");

			if (touching_closet) {
				closet.GetComponent<Closet> ().Action2 (this.gameObject);
				action_button2 = false;
				return;
			}
			if (touching_box) {
				//if playing the magician controlling the box
				// no need to do regular action
				if (player_num == 0) {
					UseAbility ();
					action_button2 = false;
					return;
				}
				box.GetComponent<MagicalBox> ().Action (this.gameObject);
				action_button2 = false;
				return;
			}
			UseAbility ();

		}
		//set back the button to false by default to 
		// only process action once per button press 
		action_button2 = false;
	}


	/// Ability Functions 
	/// 
	///  each magician is expected to have their 
	///  ability object attached to their GameObject and Magician script 


	public void UseAbility(){
		if (ability_object == null)
			return;
		print ("ability");
		ability_object.SetActive (true);
		ability_object.GetComponent<Ability> ().UseAbility ();
	}

	public void EnterCloset(){
		in_closet = true;
		hiding = true;
		Disappear ();
	}
	//
	public void ExitCloset(){
		in_closet = false;
		hiding = false;
		Reappear ();
	}

	public void EnterBox(){
		in_box = true;
		hiding = true;
		Disappear ();
	}
	//
	public void ExitBox(){
		in_box = false;
		hiding = false;
		Reappear ();
	}
	//sets the magician sprite to invisible
	// TO DO: add juice, particles, maybe puff of smoke?
	public void Disappear(){
		sprend.color = new Color (1, 1, 1, 0);
	}
	//sets the magician sprite to visible
	// TO DO: add juice, particles, maybe puff of smoke?
	public void Reappear(){
		sprend.color = new Color (1, 1, 1, 1);
	}
	//function to return true when magician is hiding anywhere
	public bool Hiding(){
		return hiding;
	}
	//Processes animation changes that are based on velocity
	void ProcessVelocityAnimation(){
		if (rb.velocity.x > 0) {
			sprend.flipX = true;
		} else if (rb.velocity.x < 0) {
			sprend.flipX = false;
		}
	}
	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.tag == "Closet") {
			touching_closet = true;
			closet = coll.gameObject;
		}
		if (coll.gameObject.tag == "Magical Box") {
			touching_box = true;
			box = coll.gameObject;
		}

	}
	void OnTriggerExit(Collider coll){
		if (coll.gameObject.tag == "Closet") {
			touching_closet = false;
			closet = null;
		}
		if (coll.gameObject.tag == "Magical Box") {
			touching_box = false;
			box = null;
		}
	}
}



