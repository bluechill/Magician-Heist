using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;
public class PlayerSelector : MonoBehaviour {
    //public InputDevice[] controller_refs;
    public Dictionary<int, InputDevice> controller_refs;
	public static PlayerSelector instance;
	public GameObject title_screen;
	public GameObject selection_screen;
	public GameObject kill_object;
	int max_players = 4;
	public InputDevice[] controllers;
	public GameObject[] controller_icons;
	public int[] choices;
	public bool[] cooldowns;
	public int num_players;
	public bool[] init;
	public bool setting_up = false;
	public bool grow = false;
	public bool kill = false;
    public bool end_setup = false;
	// Use this for initialization
	void Start () {
        //controller_refs = new InputDevice[4];
        controller_refs = new Dictionary<int, InputDevice>();
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
		//num_players = InputManager.Devices.Count;
		if (setting_up) {
			if (title_screen) {
				title_screen.GetComponent<TitleScreen> ().Kill ();
			}
			InitControllers ();
			TakeInput ();
			UpdateChoices ();
		} else {
			for (int i = 0; i < InputManager.Devices.Count; i++) {
				if (InputManager.Devices [i].Action1) {
					setting_up = true;

				}
			}
		}


		if (grow) {
			GrowSelectionScreen ();
		}
		if (kill) {
			DestroySelectionScreen ();
		}
        if (end_setup) Kill();
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
        if (end_setup) return;
		for (int i = 0; i < num_players; i++) {
			if (init[i]) {
				bool cd = false;

				if (controllers [i].MenuWasPressed && ValidChoices () && !end_setup) {
					print ("selected characters");

                    end_setup = true;
                    for(int k = 0; k < 4; k++) {
                        controller_refs.Add(choices[k], controllers[k]);

                    }


                }

				if (!cooldowns[i] && controllers [i].LeftStickX < 0f) {
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

				if (!cooldowns[i] && controllers [i].LeftStickX > 0f) {
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
			if (controller_icons [i])
				controller_icons [i].SetActive (true);
			else
				continue;

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
			if(controller_icons[i]) controller_icons [i].SetActive (false);
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
		if (num_players < 4)
			return false;

		for (int i = 0; i < num_players; i++) {
			for (int j = 0; j < num_players; j++) {
				if (i != j) {

					if (choices [i] == -1 || choices [j] == -1) {
						return false;
					} if (i != j) {
						if (choices [i] == choices [j]) {
							return false;
						}
					}


				}
			}
		}
		return true;
	}
	public void Grow(){
		grow = true;
	}
	public void Kill(){
		kill = true;
	}
	void DestroySelectionScreen(){
		if (selection_screen) {
			selection_screen.gameObject.transform.localScale = Vector3.Lerp (selection_screen.gameObject.transform.localScale, Vector3.zero, 0.1f);
			if (selection_screen.gameObject.transform.localScale.magnitude <= 0.0001f) {
				Destroy (selection_screen.gameObject);
			}

		}

		if(!selection_screen){
			kill_object.gameObject.transform.localScale = Vector3.Lerp (kill_object.gameObject.transform.localScale, new Vector3(0f, kill_object.gameObject.transform.localScale.y, 0f), 0.15f);
			if (kill_object.gameObject.transform.localScale.x <= 0.0001f) {
				SceneManager.LoadScene ("Beta Level");
				Destroy (kill_object.gameObject);
			}
		}

	}
	void GrowSelectionScreen(){
		selection_screen.gameObject.transform.localScale = Vector3.Lerp (selection_screen.gameObject.transform.localScale, new Vector3(0.6f, 0.6f, 1f), 0.1f);
		if (selection_screen.gameObject.transform.localScale.x >= 0.59f) {
			grow = false;
		}
	}
}
