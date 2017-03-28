using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

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

	PlayerScript[] players;
	public PlayerScript player1;
	public PlayerScript player2;
	public PlayerScript player3;
	public PlayerScript player4;

	public GameObject[] playerPrefabs;

	public int gold_bars = 0;
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

			players = new PlayerScript[num_players];

			for (int i = 0; i < num_players; i++) {
				GameObject player;
				player = MonoBehaviour.Instantiate (playerPrefabs [PlayerSelector.instance.choices [i]]);
				players [i] = player.GetComponent<PlayerScript> ();
				player.GetComponent<PlayerScript> ().player_num = i;
				if (i <= 1) {
					camera.GetComponent<Camera2DFollowMultiple> ().targets [i] = player.transform;
				}
			}
		}

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
		if (run_time >= time_limit) {
			Win ();
		}

		timer_text.text = "Time Remaining: " + ((time_limit - run_time)).ToString("0.00");
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
		win_menu.SetActive (true);

		PlayerScript winner = player1;
		if (player2.points > winner.points)
			winner = player2;
		if (player3 != null && player3.points > winner.points)
			winner = player3;
		if (player4 != null && player4.points > winner.points)
			winner = player4;

		win_score.text = winner.pointsText.text;

		if (winner == player1)
			win_text.text = "Player 1 Wins!";
		else if (winner == player2)
			win_text.text = "Player 2 Wins!";
		else if (player3 != null && winner == player3)
			win_text.text = "Player 3 Wins!";
		else if (player4 != null && winner == player4)
			win_text.text = "Player 4 Wins!";

		Invoke ("RestartNoTut", 5f);
	}

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RestartNoTut()
    {
        SceneManager.LoadScene("play_test_no_tut");
    }
}
