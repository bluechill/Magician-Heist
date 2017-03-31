using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public bool end = false;
	public GameObject ui;
	bool low_time = false;
	public bool skip_select = false;
	public GameObject camera;
	public int num_players = 0;
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

	PlayerScript[] players;
	public PlayerScript player1;
	public PlayerScript player2;
	public PlayerScript player3;
	public PlayerScript player4;

	public GameObject[] playerPrefabs;
	public int[] playerChoices;

	public int gold_bars = 0;
    public int red_team_score = 0;
    public int blue_team_score = 0;
	public float time_limit;
	public float run_time = 0f;
	public float start_time = 0f;
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
		if (!skip_select) {
			num_players = PlayerSelector.instance.num_players;

			for (int i = 0; i < num_players; i++) {
				players[i].GetComponent<PlayerScript> ().player_num = playerChoices[i];
			}

		}
		SoundsController.instance.StopSound ("Title");
		SoundsController.instance.StopSound ("Main");

		if(!SoundsController.instance.IsPlaying("Main")) SoundsController.instance.PlaySound ("Main");
	}
    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            RestartNoTut();
        }
		if (Input.GetKeyDown(KeyCode.F2))
		{
			Win ();
		}
        if (tut < tutorials.Length)
        {
            return;
        }

        run_time = Time.time - start_time;
		if (time_limit - run_time < 60f) {
			low_time = true;

		}
		if (run_time >= time_limit) {
			Win ();
		}

		timer_text.text = "Time Remaining: " + ((time_limit - run_time)).ToString("0.00");
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

		red_team_score_obj.text = "";
		red_team_score_obj.text += red_team_score.ToString();

		blue_team_score_obj.text = "";
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
}
