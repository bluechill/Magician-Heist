using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour {
	SpriteRenderer rend;
	public Sprite[] sprites;
	int idx = 0;
	// Use this for initialization
	void Start () {
		rend = GetComponent<SpriteRenderer> ();
		Invoke ("BlinkSprite",0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void BlinkSprite(){
		rend.sprite = sprites [idx % sprites.Length];
		idx++;
		Invoke ("BlinkSprite",0.5f);
	}
}
