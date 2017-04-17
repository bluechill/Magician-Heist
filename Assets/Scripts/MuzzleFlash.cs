using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {
	float age;
	float birth;
	float life = 0.05f;
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
