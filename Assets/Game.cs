using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public static Game GameInstance;

	public Text timer_text;
	public Text win_score;
	public GameObject win_menu;
	public GameObject lose_menu;

	public int gold_bars = 0;
	public float time_limit;
	public float run_time = 0f;
	public float start_time = 0f;
	// Use this for initialization
	void Start () {
		GameInstance = this;
		start_time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		run_time = Time.time - start_time;
		if (run_time >= time_limit) {
			Lose ();
		}

		timer_text.text = "Time Remaining: " + ((time_limit - run_time)).ToString("0.00");
	}

	public void Win(){
		win_menu.SetActive (true);
		win_score.text = gold_bars.ToString();
		Invoke ("Restart", 4f);
	}
	public void Lose(){
		lose_menu.SetActive (true);
		Invoke ("Restart", 4f);
	}
	public void Restart(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
}
