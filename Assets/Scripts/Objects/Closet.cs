using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour {

	SpriteRenderer closet_rend;
	public GameObject eyes;
	SpriteRenderer eyes_rend;
	public int num_inside = 0;
	public Sprite[] sprites;
	bool open = false;
	// Use this for initialization
	void Start () {
		closet_rend = GetComponent<SpriteRenderer> ();
		eyes_rend = eyes.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//occurs when a magician presses action 1 
	public void Action1(){
		if (open) {
			eyes.SetActive (false);
			closet_rend.sprite = sprites [6];

		} else {
			closet_rend.sprite = sprites [5];
			eyes.SetActive (true);
			eyes_rend.sprite = sprites [num_inside];
		}
		open = !open;
	}
	//occurs when a magician presses action 2
	public void Action2(GameObject magician){

	}

}
