using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsPrompt : MonoBehaviour {

	public GameObject parentObject;
	public GameObject parentPlayer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!parentPlayer.GetComponent<PlayerScript> ().nearestActionObject) {
			Destroy (this.gameObject);
		}
		if (parentPlayer.GetComponent<PlayerScript> ().nearestActionObject != parentObject) {
			Destroy (this.gameObject);
		}
	}
}
