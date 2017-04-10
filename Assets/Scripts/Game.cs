using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public AudioSource[] grunts;
	public GameObject goldTextBox;
	string[] gold_text = {"We've ", "cracked ", "the vault, ", "time ", "to ", "get ", "the ", "gold !"};
	int gold_index = 0;
	float write_speed = 0.12f;
	bool growGoldText = false;
	bool shrinkGoldText = false;
	public GameObject goldbar1;
	public GameObject goldbar2;

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
	// Use this for initialization
	void Start () {

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
	void InitGame(){


        SoundsController.instance.StopPlaying ();

		SoundsController.instance.QueueClip (endGame);
		if (keyb_debug) {
			return;
		}
		playerChoices = new int[4];
		for (int k = 0; k < 4; k++) {
			playerChoices[k] = PlayerSelector.instance.choices[k];
		}
	}

    // Update is called once per frame
    void Update () {
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
		if (time_limit - run_time < 60f) {
			low_time = true;

		}

        if (run_time > 5 && !RTA1_Active) {
            RTA1_Active = true;
            // PLAY VOICE LINE HERE
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
            RTA1_Items[rand1].gameObject.GetComponent<Item>().points *= 3;
            RTA1_Items[rand2].gameObject.GetComponent<Item>().points *= 3;
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
            RTA2_Active = true;
            event2TextBox.SetActive(true);
            growEvent2Text = true;
            int rand3 = Random.Range(0, RTA2_Items.Length);
            RTA2_Items[rand3].gameObject.GetComponent<Item>().enabled = true;
            RTA2_Items[rand3].gameObject.GetComponent<Collider>().enabled = true;
            RTA2_Items[rand3].gameObject.GetComponent<Item>().points = 500;
            RTA_Arrows[2].GetComponent<Transform>().position = new Vector3(RTA2_Items[rand3].gameObject.GetComponent<Transform>().position.x, RTA2_Items[rand3].gameObject.GetComponent<Transform>().position.y + 1.5f);
            RTA_Arrows[2].GetComponent<Transform>().parent = RTA2_Items[rand3].GetComponent<Transform>();
			RTA_Arrows [2].GetComponent<Arrow> ().SetOriginal ();
        }

        if (run_time > 161) {
            shrinkEvent2Text = true;
        }

        if (run_time >= time_limit) {
			Win ();
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
		UpdateUI ();
		if (end)
			GrowWinMenuX ();

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
		end = true;
		win_menu.SetActive (true);
		ui.SetActive (false);
		bool red_winner = false;

		if (red_team_score > blue_team_score) {
			red_winner = true;
		} else if (red_team_score < blue_team_score) {
			red_winner = false;
		} else {
			print ("tie");
		}

		win_score.text = "";
		win_text.text = "";

		if (red_winner) {
			win_text.text += "RED TEAM WINS";
			win_score.text += "profit: $" + red_team_score.ToString ();
		} else {
			win_text.text += "BLUE TEAM WINS";
			win_score.text += "profit: $" + blue_team_score.ToString ();

		}

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
		goldTextBox.SetActive(true);
		growGoldText = true;
	}
	void GrowGoldTextBox(){
		if(goldTextBox.activeSelf) goldTextBox.transform.localScale = Vector3.Lerp (goldTextBox.transform.localScale, new Vector3(1f, 1f, 1f), 0.1f);
		if (goldTextBox.activeSelf && goldTextBox.transform.localScale.x >= 0.9f) {
			growGoldText = false;
			SoundsController.instance.PlaySound (Game.GameInstance.phone);
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
		if(event1TextBox.activeSelf) event1TextBox.transform.localScale = Vector3.Lerp (event1TextBox.transform.localScale, new Vector3(1f, 1f, 1f), 0.1f);
		if (event1TextBox.activeSelf && event1TextBox.transform.localScale.x >= 0.9f) {
			growEvent1Text = false;
			SoundsController.instance.PlaySound (Game.GameInstance.phone);
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
        if (event2TextBox.activeSelf) event2TextBox.transform.localScale = Vector3.Lerp(event2TextBox.transform.localScale, new Vector3(1f, 1f, 1f), 0.1f);
        if (event2TextBox.activeSelf && event2TextBox.transform.localScale.x >= 0.9f) {
            growEvent2Text = false;
            SoundsController.instance.PlaySound(Game.GameInstance.phone);
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
}
