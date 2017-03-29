using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecurityCam : MonoBehaviour {


	public bool[] activated;
	public Image screen_flash;
	public bool turning_red = false;
	public bool red = false;
	public bool turning_clear = false;
	public bool in_use = false;
	// Use this for initialization
	void Start () {
		activated = new bool[4];
		for (int i = 0; i < 4; i++) {
			activated [i] = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (turning_red && !red) {
			screen_flash.color = Color.Lerp (screen_flash.color, new Color(screen_flash.color.r, screen_flash.color.g, screen_flash.color.b, 0.3f), 0.1f);
			if (screen_flash.color.a > 0.28f) {
				red = true;
				turning_red = false;
				turning_clear = true;
			}
		} else if (turning_clear && red) {
			screen_flash.color = Color.Lerp (screen_flash.color, new Color(screen_flash.color.r, screen_flash.color.g, screen_flash.color.b, 0f), 0.1f);
			if (screen_flash.color.a <= 0.00001f) {
				red = false;
				turning_clear = false;
				in_use = false;
			}
		}
	}

	public void Use(int p_num){
		if (activated[p_num] || in_use)
			return;
		in_use = true;
		activated[p_num] = true;
		turning_red = true;
		Game.GameInstance.time_limit -= 20;
	}
}
