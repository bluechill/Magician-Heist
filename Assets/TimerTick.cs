using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerTick : MonoBehaviour {
	public static TimerTick instance;
	int tick = 5;
	public GameObject number;
	bool grow, fade;
	public bool ticking = true;
	// Use this for initialization
	void Start () {
		instance = this;
		Invoke ("StartCountdown", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (grow)
			GrowNumber ();
		if (fade)
			FadeNumber ();
	}
	void StartCountdown(){
		CountDown ();
	}
	void CountDown(){
		number.GetComponent<Text> ().text = "";
		if(tick > 0) number.GetComponent<Text> ().text += tick.ToString();
		else number.GetComponent<Text> ().text += "GO!";
		tick--;
		if (tick == -2) {
			Invoke ("StopTicking", 1f);
			grow = false;
			fade = false;
			return;
		}
		grow = true;

	}
	void StopTicking(){
		ticking = false;
	}
	void GrowNumber(){
		if (number.transform.localScale.x < 0.9f) {
			number.transform.localScale = Vector3.Lerp (number.transform.localScale, new Vector3 (1f, 1f, 1f), 0.1f);
		} else {
			grow = false;
			fade = true;
		}
	}
	void FadeNumber(){
		if (number.GetComponent<Text> ().color.a > 0.1f)
			number.GetComponent<Text> ().color = Color.Lerp (number.GetComponent<Text> ().color, new Color (number.GetComponent<Text> ().color.r, number.GetComponent<Text> ().color.g, number.GetComponent<Text> ().color.b, 0f), 0.1f);
		else {
			fade = false;
			number.transform.localScale = Vector3.zero;
			number.GetComponent<Text> ().color = new Color (number.GetComponent<Text> ().color.r,number.GetComponent<Text> ().color.g,number.GetComponent<Text> ().color.b,1f);
			CountDown ();
		}
	}
}
