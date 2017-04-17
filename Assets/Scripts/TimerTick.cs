using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerTick : MonoBehaviour {
	public static TimerTick instance;

	public AudioClip one;
	public AudioClip two;
	public AudioClip three;
	public AudioClip four;
	public AudioClip five;
	public AudioClip go;

	public AudioSource source;
	private bool played = false;

	public float time = 0.0f;
	public GameObject number;
	public bool ticking = true;
	// Use this for initialization
	void Start () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		int intNum = Mathf.FloorToInt (time);
		AudioClip clip = null;

		switch (intNum) {
		case 0:
			number.GetComponent<Text> ().text = "5";
			clip = five;
			break;
		case 1:
			number.GetComponent<Text> ().text = "4";
			clip = four;

			break;
		case 2:
			number.GetComponent<Text> ().text = "3";
			clip = three;

			break;
		case 3:
			number.GetComponent<Text> ().text = "2";
			clip = two;

			break;
		case 4:
			number.GetComponent<Text> ().text = "1";
			clip = one;

			break;
		case 5:
			number.GetComponent<Text> ().text = "GO";
			clip = go;

			break;
		default:
			ticking = false;
			number.GetComponent<Text> ().text = "";
			return;
		}

		float dec = time - Mathf.Floor (time);

		if (dec < 0.3f)
			number.transform.localScale = new Vector3 (dec / 0.3f, dec / 0.3f, dec / 0.3f);
		else if (dec < 0.7f) {
			number.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

			if (!played && clip != null) {
				source.PlayOneShot (clip);
				played = true;
			}
		} else if (dec < 1.0f) {
			number.transform.localScale = new Vector3 ((1.0f - dec) / 0.3f, (1.0f - dec) / 0.3f, (1.0f - dec) / 0.3f);
			played = false;
		}
	}
}
