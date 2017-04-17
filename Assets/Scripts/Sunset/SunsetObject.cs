using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunsetObject : MonoBehaviour {

	public SpriteRenderer sr;
	public AnimationCurve alphaCurve;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		float alpha = (Game.GameInstance.run_time / (Game.GameInstance.time_limit * 0.9f));

		if (alpha < 0.0f)
			alpha = 0.0f;
		else if (alpha > 1.0f)
			alpha = 1.0f;

		alpha = 1.0f - alpha;

		alpha = alphaCurve.Evaluate (alpha);

		sr.color = new Color (1.0f, 1.0f, 1.0f, alpha);
	}
}
