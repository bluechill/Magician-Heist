using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {
	public GameObject[] tutorials;
	public int index;
	// Use this for initialization
	void Start () {
		SoundsController.instance.DisableLooping ();
		SoundsController.instance.StopPlaying ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha2)) {

			index = 1;
			if (tutorials [index - 1].activeSelf) {
				tutorials [index - 1].SetActive (false);
				return;
			}
			Invoke ("StartTutorial", 1f);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			index = 2;
			if (tutorials [index - 1].activeSelf) {
				tutorials [index - 1].SetActive (false);
				return;
			}
			Invoke ("StartTutorial", 1f);
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			index = 3;
			if (tutorials [index - 1].activeSelf) {
				tutorials [index - 1].SetActive (false);
				return;
			}
			Invoke ("StartTutorial", 1f);
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {

			index = 4;
			if (tutorials [index - 1].activeSelf) {
				tutorials [index - 1].SetActive (false);
				return;
			}
			Invoke ("StartTutorial", 1f);
		}
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			index = 5;
			if (tutorials [index - 1].activeSelf) {
				tutorials [index - 1].SetActive (false);
				return;
			}
			Invoke ("StartTutorial", 1f);
		}
		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			index = 6;
			if (tutorials [index - 1].activeSelf) {
				tutorials [index - 1].SetActive (false);
				return;
			}
			Invoke ("StartTutorial", 1f);
		}
		if (Input.GetKeyDown (KeyCode.Alpha8)) {

			index = 7;
			if (tutorials [index - 1].activeSelf) {
				tutorials [index - 1].SetActive (false);
				return;
			}
			Invoke ("StartTutorial", 1f);
		}
		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			index = 8;
			if (tutorials [index - 1].activeSelf) {
				tutorials [index - 1].SetActive (false);
				return;
			}
			Invoke ("StartTutorial", 1f);
		}
		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			index = 9;
			if (tutorials [index - 1].activeSelf) {
				tutorials [index - 1].SetActive (false);
				return;
			}
			Invoke ("StartTutorial", 1f);
		}
	}
	void StartTutorial(){
		tutorials [index - 1].SetActive (true);
		tutorials [index - 1].GetComponentInChildren<TutorialWrite> ().Write ();
	}
}
