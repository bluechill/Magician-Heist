using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InControl;
public class Game : MonoBehaviour {
    
	public Material light_shader;
    public bool tutorial_scene = false;
    public bool tutorial_done = false;
    public GameObject timer_tick;
	public GameObject contr_selection;
	public bool start_game = false;
	public GameObject timer;
	public AudioSource[] grunts;
	public GameObject goldTextBox;
	string[] gold_text = {"We've ", "cracked ", "the vault, ", "time ", "to ", "get ", "the ", "gold !"};
	int gold_index = 0;
	float write_speed = 0.12f;
	bool growGoldText = false;
	bool shrinkGoldText = false;
	public GameObject goldbar1;
	public GameObject goldbar2;
	bool said_low_time = false;
	bool said_losing_team1 = false;
	bool said_losing_team2 = false;
	public GameObject event1TextBox;
	string[] event1_text = {"We've ", "located ", "some ", "valuable ", "items, ", "be ", "on ", "the ", "lookout ", "for ", "these ", "arrows !"};
	int event1_index = 0;
	bool growEvent1Text = false;
	bool shrinkEvent1Text = false;
	public GameObject event1_arrow1;
	public GameObject event1_arrow2;

    public GameObject event2TextBox;
	string[] event2_text = {"The ", "Boss ", "heard ", "they ", "got ", "a ", "rare ", "painting ", "in ", "there ", "he ", "wants,     ","GO ", "GET ", "IT !"};

	int event2_index = 0;
    bool growEvent2Text = false;
    bool shrinkEvent2Text = false;

	public GameObject sparklesPrefab;
    public AudioSource beep;
	public AudioSource cheering;
	public AudioSource itemRemoval;
	public AudioSource chaChing;
	public AudioSource poof;
	public AudioSource phone;
	public AudioSource shield_hit;
	public AudioSource gun_ricochet;
	public AudioSource gun_cock;
	public AudioSource gun_reload;
	public AudioSource gun_shot;

	// VOICE OVERS 

	public AudioSource[] first_event;
	public AudioSource[] second_event;
	public AudioSource[] third_event;
	public AudioSource[] time_running_out;
	public AudioSource[] lead_change;
	public AudioSource[] funny;
	public bool[] funny_said;
	bool funny_saying = false;
	public AudioSource blue_team_losing;
	public AudioSource red_team_losing;


	public AudioClip mainLoop;
	public AudioClip endGame;
	public bool keyb_debug;
	public bool end = false;
	public GameObject ui;
	bool low_time = false;
	public bool skip_select = false;
	public GameObject camera;
    public int num_players = 4;
    public bool do_tut = true;
	public static Game GameInstance;
    public GameObject[] tutorials;
    public GameObject tutorial;
    public int tut = 0;
	public Text timer_text;
	public Text win_text;
	public Text win_score;
	public GameObject win_menu;

	public Text red_team_capacity;
	public Text blue_team_capacity;
	public Text red_team_score_obj;
	public Text blue_team_score_obj;
	public GameObject redTruck;
	public GameObject blueTruck;

	public PlayerScript[] players;
	public PlayerScript player1;
	public PlayerScript player2;
	public PlayerScript player3;
	public PlayerScript player4;

	public GameObject[] playerPrefabs;
    public GameObject[] RTA1_Items;
    public GameObject[] RTA2_Items;
    public GameObject Gold_Door;
    public GameObject[] RTA_Arrows;
	public int[] playerChoices;

	public int gold_bars = 0;
    public int red_team_score = 0;
    public int blue_team_score = 0;
	public float time_limit;
	public float run_time = 0f;
	public float start_time = 0f;
    bool RTA1_Active = false;
    bool RTA2_Active = false;
    bool Gold_Door_Active = true;


    public bool AcceptInput = false;
	// Use this for initialization
	void Start () {
		funny_said = new bool[funny.Length];
		for (int i = 0; i < funny.Length; i++) {
			funny_said [i] = false;
		}
        if (tutorial_scene)
        {

            SoundsController.instance.DisableLooping();
            SoundsController.instance.StopPlaying();
            SoundsController.instance.ResetEventTime();
            SoundsController.instance.QueueClip(endGame);


        }
        InitGame ();

		GameInstance = this;
		start_time = Time.time;

        if (do_tut)
        {
            tutorial.SetActive(true);
            Tutorial();
        }
        else tut = tutorials.Length;
    }
	void Awake(){
		
	}
	void InitGame(){

		if (keyb_debug) {
			return;
		}
		playerChoices = new int[4];
		for (int k = 0; k < 4; k++) {
			playerChoices [k] = k;
		}
	}
	void InitControllerSetup(){
		for (int k = 0; k < 4; k++) {
			players [k].player_num = Controllers.instance.choice_nums [k];
		}
	}
    // Update is called once per frame
    void Update () {
        
        if(tutorial_scene && tutorial_done)
        {
            SceneManager.LoadScene("Gamma Level");
        }

		if (!start_game) {
			timer_tick.SetActive (false);
			return;
		}
		else {
			timer_tick.SetActive (true);
			ShrinkControllers ();
			InitControllerSetup ();

		}
		if (TimerTick.instance.ticking)
			return;
		else if(!timer.activeSelf) {

			SoundsController.instance.DisableLooping ();
			SoundsController.instance.StopPlaying ();
			SoundsController.instance.ResetEventTime ();
			SoundsController.instance.QueueClip (endGame);

			start_time = Time.time;
			timer.SetActive (true);
			AcceptInput = true;
		}
        if (run_time >= time_limit && !tutorial_scene) {
            Win();
        }
        if (growGoldText) {
			GrowGoldTextBox ();
		}
		if (shrinkGoldText) {
			ShrinkGoldTextBox ();
		}
		if (growEvent1Text) {
			GrowEvent1TextBox ();
		}
		if (shrinkEvent1Text) {
			ShrinkEvent1TextBox ();
		}
        if (growEvent2Text) {
            GrowEvent2TextBox();
        }
        if (shrinkEvent2Text) {
            ShrinkEvent2TextBox();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Restart();
        }
		else if (Input.GetKeyDown(KeyCode.F1))
        {
            RestartNoTut();
        }
		else if (Input.GetKeyDown(KeyCode.F2))
		{
			Win ();
		}
		else if (Input.GetKeyDown (KeyCode.F3))
		{
			RestartGame ();
		}

        if (tut < tutorials.Length)
        {
            return;
        }

        run_time = Time.time - start_time;
		if (time_limit - run_time < 60f && !tutorial_scene) {
			low_time = true;

		}

        if (run_time > 5 && !RTA1_Active) {
            RTA1_Active = true;
            // PLAY VOICE LINE HERE

			first_event [Random.Range (0, first_event.Length)].Play ();

            int rand1 = Random.Range(0, RTA1_Items.Length);
            int rand2 = Random.Range(0, RTA1_Items.Length);
            print(rand1);
            print(rand2);
            if (rand1 == rand2) {
                if (rand2 < 5)
                    rand2 += 1;
                else {
                    rand2 = Random.Range(0, (RTA1_Items.Length - 1));
                }
            }
            print(rand1);
            print(rand2);
			event1TextBox.SetActive(true);
			growEvent1Text = true;
            RTA1_Items[rand1].gameObject.GetComponent<Item>().points *= 5;
            RTA1_Items[rand2].gameObject.GetComponent<Item>().points *= 5;
            RTA1_Items[rand1].gameObject.GetComponent<Item>().money_grade = 4;
            RTA1_Items[rand2].gameObject.GetComponent<Item>().money_grade = 4;
            RTA_Arrows[0].GetComponent<Transform>().position = new Vector3 (RTA1_Items[rand1].gameObject.GetComponent<Transform>().position.x, RTA1_Items[rand1].gameObject.GetComponent<Transform>().position.y + 1.5f);
            RTA_Arrows[1].GetComponent<Transform>().position = new Vector3(RTA1_Items[rand2].gameObject.GetComponent<Transform>().position.x, RTA1_Items[rand2].gameObject.GetComponent<Transform>().position.y + 1.5f);
            RTA_Arrows[0].GetComponent<Transform>().parent = RTA1_Items[rand1].GetComponent<Transform>();
            RTA_Arrows[1].GetComponent<Transform>().parent = RTA1_Items[rand2].GetComponent<Transform>();
			RTA_Arrows [0].GetComponent<Arrow> ().SetOriginal ();
			RTA_Arrows [1].GetComponent<Arrow> ().SetOriginal ();
			GameObject sparkles1 = MonoBehaviour.Instantiate (sparklesPrefab);
			sparkles1.transform.parent = RTA1_Items[rand1].GetComponent<Transform>();
			sparkles1.transform.localPosition = new Vector3 (0f, 0.5f, 0f);
			GameObject sparkles2 = MonoBehaviour.Instantiate (sparklesPrefab);
			sparkles2.transform.parent = RTA1_Items[rand2].GetComponent<Transform>();
			sparkles2.transform.localPosition = new Vector3 (0f, 0.5f, 0f);


        }
		if (run_time > 16) {
			shrinkEvent1Text = true;
		}
        if (run_time > 90 && Gold_Door_Active) {
			OpenGoldDoor();
        }
		if (run_time > 101) {
			shrinkGoldText = true;
		}
        if (run_time > 150 && !RTA2_Active) {
			third_event [Random.Range (0, third_event.Length)].Play ();
            RTA2_Active = true;
            event2TextBox.SetActive(true);
            growEvent2Text = true;
            int rand3 = Random.Range(0, RTA2_Items.Length);
            RTA2_Items[rand3].gameObject.GetComponent<Item>().enabled = true;
            RTA2_Items[rand3].gameObject.GetComponent<Collider>().enabled = true;
            RTA2_Items[rand3].gameObject.GetComponent<Item>().points = 300;
			GameObject sparkles3 = MonoBehaviour.Instantiate (sparklesPrefab);
			sparkles3.transform.parent = RTA2_Items[rand3].GetComponent<Transform>();
			sparkles3.transform.localPosition = new Vector3 (0f, 0.5f, 0f);
			RTA2_Items [rand3].gameObject.GetComponent<SpriteRenderer> ().material = light_shader;
            RTA_Arrows[2].GetComponent<Transform>().position = new Vector3(RTA2_Items[rand3].gameObject.GetComponent<Transform>().position.x, RTA2_Items[rand3].gameObject.GetComponent<Transform>().position.y + 1.5f);
            RTA_Arrows[2].GetComponent<Transform>().parent = RTA2_Items[rand3].GetComponent<Transform>();
			RTA_Arrows [2].GetComponent<Arrow> ().SetOriginal ();
        }

        if (run_time > 161) {
            shrinkEvent2Text = true;
        }

        
        int minutes = (int)((time_limit - run_time )/ 60f);
        int second = (int)((time_limit - run_time) % 60f);
        if (second < 10) {
            timer_text.text = minutes.ToString() + ": 0" + second.ToString();
        }
        else {
            timer_text.text = minutes.ToString() + ": " + second.ToString();
        }
        if (minutes == 0) {
            timer_text.color = Color.Lerp(timer_text.color, Color.red, 0.01f);
        }

        if (low_time) {
			FlashTimer ();
		}
		if (time_limit - run_time < 25f && !tutorial_scene && !said_low_time) {
			said_low_time = true;
			time_running_out [Random.Range (0, time_running_out.Length)].Play ();
		}
		if (time_limit - run_time < 130f && !tutorial_scene && !said_losing_team2) {
			said_losing_team2 = true;
			lead_change [Random.Range (0, lead_change.Length)].Play ();

		}
		if (time_limit - run_time < 90f && !tutorial_scene && !said_losing_team1) {

			said_losing_team1 = true;
			if (blue_team_score > red_team_score) {
				red_team_losing.Play ();
			} else if(blue_team_score < red_team_score){
				blue_team_losing.Play ();
			} else {
				said_losing_team1 = false;
			}
		}
		UpdateUI ();
		if (end)
			GrowWinMenuX ();
		if (end) {
			for (int i = 0; i < 4; i++) {
				if (InputManager.Devices.Count > i && InputManager.Devices[i].MenuWasPressed) {
					SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
				}
			}
		}

	}
	void UpdateUI(){
		
		red_team_capacity.text = "";
		red_team_capacity.text += redTruck.GetComponent<TruckScript>().weight_used.ToString();

		blue_team_capacity.text = "";
		blue_team_capacity.text += blueTruck.GetComponent<TruckScript>().weight_used.ToString();

		red_team_score_obj.text = "$";
		red_team_score_obj.text +=  red_team_score.ToString();

		blue_team_score_obj.text = "$";
		blue_team_score_obj.text += blue_team_score.ToString();


	}
    void Tutorial()
    {
        if (tut >= tutorials.Length)
        {
            return;
        }
        if (tut > 0)
        {
            tutorials[tut - 1].SetActive(false);
        }
        tutorials[tut].SetActive(true);
        tut++;
        if (tut >= tutorials.Length)
        {
            Invoke("EndTut", 4f);
            return;
        }
        Invoke("Tutorial", 4f);
    }
    void EndTut()
    {
        tutorial.SetActive(false);
    }
    public void Win(){
        AcceptInput = false;
        end = true;
		win_menu.SetActive (true);
		ui.SetActive (false);
		bool red_winner = false;

        win_score.text = "";
        win_text.text = "";

        if (red_team_score > blue_team_score) {
            win_text.text += "RED TEAM WINS";
            win_score.text += "profit: $" + red_team_score.ToString();
        } else if (red_team_score < blue_team_score) {
            win_text.text += "BLUE TEAM WINS";
            win_score.text += "profit: $" + blue_team_score.ToString();
        } else {
            win_text.text += "TIE!";
            win_score.text += "profit: $" + blue_team_score.ToString();

        }

		/*win_score.text = "";
		win_text.text = "";

		if (red_winner) {
			win_text.text += "RED TEAM WINS";
			win_score.text += "profit: $" + red_team_score.ToString ();
		} else {
			win_text.text += "BLUE TEAM WINS";
			win_score.text += "profit: $" + blue_team_score.ToString ();

		}*/

	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene("ControllerSetup");
	}

    public void RestartNoTut()
    {
        SceneManager.LoadScene("play_test_no_tut");
    }
	public void FlashTimer(){
		GetComponent<TimerFlash> ().Use ();
	}
	public void GrowWinMenuX(){
		win_menu.transform.localScale = Vector3.Lerp (win_menu.transform.localScale, new Vector3(1f, win_menu.transform.localScale.y, 1f), 0.05f);
	}
	public void OpenGoldDoor(){
		Gold_Door_Active = false;
		Destroy(Gold_Door);
		//PLAY AUDIO OF DOOR BEING DESTROYED
		second_event [Random.Range (0, second_event.Length)].Play ();

		goldTextBox.SetActive(true);
		growGoldText = true;
	}
	void GrowGoldTextBox(){
		if(goldTextBox.activeSelf) goldTextBox.transform.localScale = Vector3.Lerp (goldTextBox.transform.localScale, new Vector3(1.25f, 1.25f, 1.25f), 0.1f);
		if (goldTextBox.activeSelf && goldTextBox.transform.localScale.x >= 1.2f) {
			growGoldText = false;
			Invoke ("WriteGoldText", write_speed);
		}
	}

	void ShrinkGoldTextBox(){
		if(goldTextBox.activeSelf) goldTextBox.transform.localScale = Vector3.Lerp (goldTextBox.transform.localScale, new Vector3(0f, 0f, 0f), 0.1f);
		if (goldTextBox.activeSelf && goldTextBox.transform.localScale.magnitude <= 0.1f) {
			shrinkGoldText = false;
			goldTextBox.SetActive (false);
		}

	}
	void WriteGoldText(){
		if (gold_index == gold_text.Length) {
			goldbar1.SetActive (true);
			goldbar2.SetActive (true);
			phone.Stop ();
			return;
		}
		goldTextBox.GetComponentInChildren<Text> ().text += gold_text [gold_index++];
		Invoke ("WriteGoldText", write_speed);
	}


	void GrowEvent1TextBox(){
		if(event1TextBox.activeSelf) event1TextBox.transform.localScale = Vector3.Lerp (event1TextBox.transform.localScale, new Vector3(1.25f, 1.25f, 1.25f), 0.1f);
		if (event1TextBox.activeSelf && event1TextBox.transform.localScale.x >= 1.2f) {
			growEvent1Text = false;
			Invoke ("WriteEvent1Text", write_speed);
		}
	}

	void ShrinkEvent1TextBox(){
		if(event1TextBox.activeSelf) event1TextBox.transform.localScale = Vector3.Lerp (event1TextBox.transform.localScale, new Vector3(0f, 0f, 0f), 0.1f);
		if (event1TextBox.activeSelf && event1TextBox.transform.localScale.magnitude <= 0.1f) {
			shrinkEvent1Text = false;
			event1TextBox.SetActive (false);
		}

	}
	void WriteEvent1Text(){
		if (event1_index == event1_text.Length) {
			event1_arrow1.SetActive (true);
			event1_arrow2.SetActive (true);
			phone.Stop ();
			return;
		}
		event1TextBox.GetComponentInChildren<Text> ().text += event1_text [event1_index++];
		Invoke ("WriteEvent1Text", write_speed);
	}
    void GrowEvent2TextBox() {
        if (event2TextBox.activeSelf) event2TextBox.transform.localScale = Vector3.Lerp(event2TextBox.transform.localScale, new Vector3(1.25f, 1.25f, 1.25f), 0.1f);
        if (event2TextBox.activeSelf && event2TextBox.transform.localScale.x >= 1.2f) {
            growEvent2Text = false;
            Invoke("WriteEvent2Text", write_speed);
        }
    }

    void ShrinkEvent2TextBox() {
        if (event2TextBox.activeSelf) event2TextBox.transform.localScale = Vector3.Lerp(event2TextBox.transform.localScale, new Vector3(0f, 0f, 0f), 0.1f);
        if (event2TextBox.activeSelf && event2TextBox.transform.localScale.magnitude <= 0.1f) {
            shrinkEvent2Text = false;
            event2TextBox.SetActive(false);
        }

    }
    void WriteEvent2Text() {
        if (event2_index == event2_text.Length) {
            phone.Stop();
            return;
        }
        event2TextBox.GetComponentInChildren<Text>().text += event2_text[event2_index++];
        Invoke("WriteEvent2Text", write_speed);
    }
	void ShrinkControllers() {
		if (contr_selection.activeSelf) contr_selection.transform.localScale = Vector3.Lerp(contr_selection.transform.localScale, new Vector3(0f, contr_selection.transform.localScale.y, 0f), 0.1f);
		if (contr_selection.activeSelf && contr_selection.transform.localScale.x <= 0.1f) {
			contr_selection.SetActive(false);
		}

	}
	bool FunnyLeft(){
		for (int i = 0; i < funny.Length; i++) {
			if(!funny_said[i]){
				return true;
			}
		}
		return false;
	}
	public void FunnyQuote(){
		if(funny_saying || !FunnyLeft()){
			return;
		}
		Invoke ("SayFunnyQuote", 4.4f);
	}
	public void SayFunnyQuote(){

		int r = Random.Range (0, Game.GameInstance.funny.Length);

		while(Game.GameInstance.funny_said[r]){
			r = Random.Range (0, Game.GameInstance.funny.Length);
			if (!FunnyLeft ())
				return;
		}

		Game.GameInstance.funny [r].Play();
		Game.GameInstance.funny_said[r] = true;
	}
}
