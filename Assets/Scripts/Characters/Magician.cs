using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
public class Magician : Character {

	//members used for controller input/movement
	int num_buttons = 3;
	bool[] buttons;
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
		buttons = new bool[num_buttons];
		for (int i = 0; i < buttons.Length; i++) {
			buttons[i] = false;
		}
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
		KeyboardControls[0][2] = KeyCode.F;

		KeyboardControls[1][0] = KeyCode.J;
		KeyboardControls[1][1] = KeyCode.K;
		KeyboardControls[1][2] = KeyCode.Semicolon;

	}
	//process player input from the keyboard
	//DOES NOT SUPPORT MULTIPLE MAGICIANS
	void ProcessInputKeyboard(){
		//uses computer keys to allow easier beginning development for other members of the group. 
		if (Input.GetKeyDown (KeyboardControls[player_num][0])) {
			buttons [0] = true;
		} 
		if (Input.GetKeyUp (KeyboardControls[player_num][0])) {
			buttons [0] = false;
		}
		if (Input.GetKeyDown (KeyboardControls[player_num][1])) {
			buttons [1] = true;
		} 
		if (Input.GetKeyUp (KeyboardControls[player_num][1])) {
			buttons [1] = false;
		}
		if (Input.GetKeyDown (KeyboardControls[player_num][2])) {
			buttons [2] = true;
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
	}
	void ProcessMovement(){
		//move left 
		if (buttons [0]) {
			rb.AddForce (Vector3.left * movement_velocity);
		}

		//move right
		if (buttons [1]) {
			rb.AddForce (Vector3.right * movement_velocity);
		}
	}
	//function to process pressing the action button
	void ProcessAction(){

		//if X button has been pressed 
		if (buttons [2]) {



		}
		//set back the button to false by default to 
		// only process action once per button press 
		buttons [2] = false;
	}
	//Processes animation changes that are based on velocity
	void ProcessVelocityAnimation(){
		if (rb.velocity.x > 0) {
			sprend.flipX = true;
		} else if (rb.velocity.x < 0) {
			sprend.flipX = false;
		}
	}
}



