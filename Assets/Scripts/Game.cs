﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {
	private double nextMusicEventTime;
	private int nextClip = 0;
	private int flip = 0;
	public AudioClip[] musicClips;

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

		SoundsController.instance.sound_objects [0].GetComponent<AudioSource> ().loop = false;
		SoundsController.instance.sound_objects [1].GetComponent<AudioSource> ().loop = false;

		if (musicClips.Length > nextClip) {
			SoundsController.instance.sound_objects [flip].GetComponent<AudioSource> ().clip = musicClips [nextClip];
			SoundsController.instance.sound_objects [flip].GetComponent<AudioSource> ().Play ();
			nextMusicEventTime = AudioSettings.dspTime + musicClips [nextClip].length;
			flip = 1 - flip;
			++nextClip;
		}
	}
    // Update is called once per frame
    void Update () {
		var time = AudioSettings.dspTime;
		if (time + 1.0 > nextMusicEventTime && musicClips.Length > nextClip) {
			SoundsController.instance.sound_objects [flip].GetComponent<AudioSource> ().clip = musicClips [nextClip];
			SoundsController.instance.sound_objects [flip].GetComponent<AudioSource> ().PlayScheduled (nextMusicEventTime);
			nextMusicEventTime = AudioSettings.dspTime + musicClips [nextClip].length;
			++nextClip;
			flip = flip - 1;
		}

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
	public void FlashTimer(){
		GetComponent<TimerFlash> ().Use ();
	}
}
