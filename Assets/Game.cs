using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public static Game GameInstance;
    public GameObject[] tutorials;
    public GameObject tutorial;
    public int tut = 0;
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
        tutorial.SetActive(true);
        Tutorial();
    }

    // Update is called once per frame
    void Update () {
        if (tut < tutorials.Length)
        {
            return;
        }

        run_time = Time.time - start_time;
		if (run_time >= time_limit) {
			Lose ();
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
		win_score.text = gold_bars.ToString();
		Invoke ("Restart", 5f);
	}
	public void Lose(){
		lose_menu.SetActive (true);
		Invoke ("Restart", 5f);
	}
	public void Restart(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
}
