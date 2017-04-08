using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	float birth;
	float age;
	float life = 3f;
	public bool red_team;
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
	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.tag == "Wall") {
			Destroy (this.gameObject);

		}
		Destroy (this.gameObject);

	}
}
