using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeLayer : MonoBehaviour {
	float age;
	float lifespan = 5;
	float birth;
	// Use this for initialization
	void Start () {
		GetComponent<ParticleSystem> ().GetComponent<Renderer> ().sortingOrder = 50;
		birth = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		age = Time.time - birth;
		if (age > lifespan)
			Destroy (this.gameObject);
	}
}
