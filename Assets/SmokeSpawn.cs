using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeSpawn : MonoBehaviour {
	public float delay;
	float age = 0f;
	float birth = 0f;
	float lifespan = 2f;
	float spawn = 0.25f;
	public GameObject player;
	// Use this for initialization
	void Start () {
		birth = Time.time;

	}
	
	// Update is called once per frame
	void Update () {
		age = Time.time - birth;
		if (age > (delay + spawn)) {
			player.SetActive (true);
			//Destroy (this.gameObject);
		} if (age > lifespan) {
			Destroy (this.gameObject);
		}
	}

}
