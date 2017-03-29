using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerFlash : MonoBehaviour {

	public Color original_color;
	public Image screen_flash;
	public bool turning_red = false;
	public bool red = false;
	public bool turning_clear = false;
	public bool in_use = false;
	// Use this for initialization
	void Start () {
		original_color = screen_flash.color;
	}

	// Update is called once per frame
	void Update () {
		if (turning_red) {
			if (!red) {
				screen_flash.color = Color.red;
				screen_flash.color = new Color (screen_flash.color.r,screen_flash.color.g, screen_flash.color.b,0f);
				red = true;
			}

			screen_flash.color = Color.Lerp (screen_flash.color, new Color(screen_flash.color.r, screen_flash.color.g, screen_flash.color.b, 0.3f), 0.075f);
			if (screen_flash.color.a > 0.28f) {
				turning_red = false;
				turning_clear = true;
			}
		} else if (turning_clear) {

			screen_flash.color = Color.Lerp (screen_flash.color, new Color(screen_flash.color.r, screen_flash.color.g, screen_flash.color.b, 0f), 0.075f);
			if (screen_flash.color.a <= 0.01f) {
				turning_clear = false;
				in_use = false;
			}
		}
	}

	public void Use(){
		if (in_use)
			return;
		in_use = true;
		turning_red = true;
	}
}
