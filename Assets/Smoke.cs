using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour {
	float age;
	float birth;
	float life = 2f;

	// Use this for initialization
	void Start () {
		birth = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		age = Time.time - birth;
		if (age > life)
			Destroy (this.gameObject);
	}
}
