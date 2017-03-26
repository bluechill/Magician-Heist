using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;
public class PlayerSelector : MonoBehaviour {

	public static PlayerSelector instance;

	int max_players = 4;
	public InputDevice[] controllers;
	public GameObject[] controller_icons;
	public int[] choices;
	public bool[] cooldowns;
	public int num_players;
	public bool[] init;
	// Use this for initialization
	void Start () {
		init = new bool[4];
		cooldowns = new bool[4];
		choices = new int[4];
		controllers = new InputDevice[4];
		for (int i = 0; i < max_players; i++) {
			init [i] = false;
			cooldowns [i] = false;
			choices [i] = -1;
		}
	}
	void Awake ()   
	{
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}
	}
	// Update is called once per frame
	void Update () {
		num_players = InputManager.Devices.Count;
		InitControllers ();
		TakeInput ();
		UpdateChoices ();
	}
	void InitControllers(){
		for (int i = 0; i < num_players; i++) {
			if (!init[i]) {
				init[i] = true;
				controllers [i] = InputManager.Devices [i];
			}
		}

	}
	void TakeInput(){
		for (int i = 0; i < num_players; i++) {
			if (init[i]) {
				bool cd = false;

				if (!cooldowns [i] && controllers [i].MenuWasPressed && ValidChoices ()) {
					print ("selected characters");
					SceneManager.LoadScene ("AfterControllerSetup");
				}

				if (!cooldowns[i] && controllers [i].DPadLeft) {
					cd = true;

					if (choices [i] == -1) {
						choices [i] = 1;
					} else {
						choices [i]--;
						if (choices [i] < 0) {
							choices [i] = 0;
						}
					}
				}

				if (!cooldowns[i] && controllers [i].DPadRight) {
					cd = true;
					if (choices [i] == -1) {
						choices [i] = 2;
					} else {
						choices [i]++;
						if (choices [i] > 3) {
							choices [i] = 3;
						}
					}
				}

				if(cd){
					cooldowns [i] = true;
					if (i == 0) {
						Invoke ("Cooldown1", 0.25f);
					}
					if (i == 1) {
						Invoke ("Cooldown2", 0.25f);
					}
					if (i == 2) {
						Invoke ("Cooldown3", 0.25f);
					}
					if (i == 3) {
						Invoke ("Cooldown4", 0.25f);
					}
				}

			}
		}
	}
	void UpdateChoices(){
		int i = 0;
		for (; i < num_players; i++) {
			controller_icons [i].SetActive (true);

			if (choices [i] == -1) {
				controller_icons [i].transform.localPosition = new Vector3 (0f, controller_icons [i].transform.localPosition.y, 0f);
			}
			if (choices [i] == 0) {
				controller_icons [i].transform.localPosition = new Vector3 (-300f, controller_icons [i].transform.localPosition.y, 0f);
			}
			if (choices [i] == 1) {
				controller_icons [i].transform.localPosition = new Vector3 (-100f, controller_icons [i].transform.localPosition.y, 0f);
			}
			if (choices [i] == 2) {
				controller_icons [i].transform.localPosition = new Vector3 (100f, controller_icons [i].transform.localPosition.y, 0f);
			}
			if (choices [i] == 3) {
				controller_icons [i].transform.localPosition = new Vector3 (300f, controller_icons [i].transform.localPosition.y, 0f);
			}
		}
		for (; i < max_players; i++) {
			controller_icons [i].SetActive (false);
		}
	}
	void Cooldown1(){
		cooldowns [0] = false;
	}
	void Cooldown2(){
		cooldowns [1] = false;
	}
	void Cooldown3(){
		cooldowns [2] = false;
	}
	void Cooldown4(){
		cooldowns [3] = false;
	}

	bool ValidChoices(){


		if (num_players <= 1)
			return false;
		
		if (num_players == 2 && (choices[0] == -1 || choices[1] == -1))
			return false;
		
		for (int i = 0; i < num_players; i++) {
			for (int j = 0; j < num_players; j++) {
				if (i != j) {
					if (( choices [i]!= -1) && (choices [j] != -1) && (choices[i] == choices[j])) {
						return false;
					}
				}
			}
		}
		return true;
	}

}
