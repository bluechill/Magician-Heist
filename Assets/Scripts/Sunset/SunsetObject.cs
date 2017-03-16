using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunsetObject : MonoBehaviour {

	public SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		sr.color = new Color (1.0f, 1.0f, 1.0f, SunsetTimer.instance.alphaAmount);
	}
}
