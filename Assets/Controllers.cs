using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllers : MonoBehaviour {
	public static Controllers instance;
	public bool[] choices;
	public int[] choice_nums;
	public Transform[] quadrants;
	public GameObject[] choice_overlaps;
	public GameObject prompt1, prompt2;
	public bool grow, grow1, grow2, shrink1, shrink2;
	// Use this for initialization
	void Start () {
		instance = this;
		choices = new bool[4];
		choices [0] = false; 
		choices [1] = false;
		choices [2] = false;
		choices [3] = false;
		choice_nums = new int[4];
		choice_nums [0] = -1; 
		choice_nums [1] = -1;
		choice_nums [2] = -1;
		choice_nums [3] = -1;
	}
	
	// Update is called once per frame
	void Update () {
		//BringInChoices ();
		if(grow) GrowThis();
		GrowChoices();
		ShrinkChoices();
		if (AllChoices ()) {
			//prompt1.SetActive (false);
			//prompt2.SetActive (true);
		} else {
			//prompt1.SetActive (true);
			//prompt2.SetActive (false);
		}

	}
	public void LockInChoices(){

		bool[] available = new bool[4];
		available [0] = true;
		available [1] = true;
		available [2] = true;
		available [3] = true;
		for (int i = 0; i < 4; i++) {
			if (choice_nums [i] != -1) {
				available [choice_nums [i]] = false;
			} 
		}

		for (int i = 0; i < 4; i++) {
			if (choice_nums [i] == -1) {
				for (int j = 0; j < 4; j++) {
					if (available [j]) {
						choice_nums [i] = j;
						available [j] = false;
						break;
					}
				}
			} 
		}
		Game.GameInstance.start_game = true;

	}
	bool AllChoices(){
		return choices [0] && choices [1] && choices [2] && choices [3];
	}

	void BringInChoices(){

		for (int i = 0; i < 4; i++) {
			if (choices [i]) {
				if (choice_overlaps [i].transform.localPosition.x - quadrants [i].localPosition.x > -0.1f && choice_overlaps [i].transform.localPosition.x - quadrants [i].localPosition.x < 0.1f && choice_overlaps [i].transform.localPosition.y - quadrants [i].localPosition.y > -0.1f && choice_overlaps [i].transform.localPosition.y - quadrants [i].localPosition.y < 0.1f) {
					choice_overlaps [i].transform.localPosition = quadrants [i].localPosition;
				}
				else choice_overlaps [i].transform.localPosition = Vector3.Lerp (choice_overlaps[i].transform.localPosition, quadrants[i].localPosition, 0.1f);
			}

		}

	}
	void GrowChoices(){
		for (int i = 0; i < 4; i++) {
			if (choices [i]) {
				GrowObj (choice_overlaps[i]);
			}

		}
	}
	void GrowObj(GameObject obj){
		if (!(obj.transform.localScale.x > 0.95f))
			obj.transform.localScale = Vector3.Lerp (obj.transform.localScale, Vector3.one, 0.1f);
		else
			obj.transform.localScale = Vector3.one;
	}
	void ShrinkChoices(){
		for (int i = 0; i < 4; i++) {
			if (!choices [i]) {
				ShrinkObj (choice_overlaps[i]);
			}

		}
	}
	void ShrinkObj(GameObject obj){
		if (!(obj.transform.localScale.x < 0.01f))
			obj.transform.localScale = Vector3.Lerp (obj.transform.localScale, Vector3.zero, 0.1f);
		else
			obj.transform.localScale = Vector3.zero;
	}
	public void Grow(){
		grow = true;
	}
	void GrowThis(){
		transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(2f, 2f, 1f), 0.1f);
		if (transform.localScale.x >= 1.95f) {
			grow = false;
		}
	}
}
