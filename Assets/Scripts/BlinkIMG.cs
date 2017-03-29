using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkIMG : MonoBehaviour {
	Image rend;
	public Sprite[] sprites;
	int idx = 0;
	public float speed;
	// Use this for initialization
	void Start () {
		rend = GetComponent<Image> ();
		Invoke ("BlinkSprite",speed);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void BlinkSprite(){
		rend.sprite = sprites [idx % sprites.Length];
		idx++;
		Invoke ("BlinkSprite",speed);
	}
}
