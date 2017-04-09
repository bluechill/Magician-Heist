using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<ParticleRenderer> ().sortingOrder = 30;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
