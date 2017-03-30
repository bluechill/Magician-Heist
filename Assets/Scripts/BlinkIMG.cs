using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkIMG : MonoBehaviour {
	Image rend;
	public Sprite[] sprites;
	int idx = 0;
	public float spriteChangeRate = 0.666f;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Image> ();

		InvokeRepeating ("BlinkSprite", spriteChangeRate, spriteChangeRate);
	}

	void BlinkSprite(){
		rend.sprite = sprites [idx];

		idx = (idx + 1) % sprites.Length;
	}
}
