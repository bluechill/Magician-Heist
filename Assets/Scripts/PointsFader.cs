using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsFader : MonoBehaviour {

	public float time = 0.0f;
	public SpriteGlow sprend;

	// Use this for initialization
	void Start () {
		sprend = GetComponent<SpriteGlow> ();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		if (time < 0.5f)
			sprend.GlowColor = new Color (sprend.GlowColor.r, sprend.GlowColor.g, sprend.GlowColor.b, time / 0.5f);
		else if (time > 2f && time < 4f)
			sprend.GlowColor = new Color (sprend.GlowColor.r, sprend.GlowColor.g, sprend.GlowColor.b, (4 - time) / 2f);
		else if (time > 4f)
			Destroy (this.gameObject);
	}
}
