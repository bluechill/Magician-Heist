using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrobeLight : MonoBehaviour {
	Light light;
	ParticleSystem ps;
	float intensity = 1.25f;
	private float time = 0.0f;
	public float switchTime = 0.6f;
	public bool loose;
	private bool emitted = false;
	// Use this for initialization
	void Start () {
		light = GetComponent<Light> ();
		ps = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (loose)
			intensity = 2.5f;
		
		time += Time.deltaTime;

		if (time > switchTime * 2.0f) {
			time = 0.0f;
			emitted = false;
		}

		float amount = (time / switchTime);

		if (amount > 1.0f) {
			amount = 2.0f - amount;

			if (!emitted && ps != null) {
				ps.Emit (3);
				emitted = true;
			}
		}

		amount *= intensity;
		light.intensity = amount;
	}
}
