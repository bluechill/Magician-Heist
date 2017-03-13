using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunsetTimer : MonoBehaviour {

	public static SunsetTimer instance;

	public float alphaAmount = 1.0f;
	public float transitionTime = 30.0f;

	// Use this for initialization
	void Start () {
		if (!instance)
			instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		alphaAmount -= Time.deltaTime * 1.0f / transitionTime;

		if (alphaAmount < 0)
			alphaAmount = 0f;
	}
}
