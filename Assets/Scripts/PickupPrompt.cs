using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPrompt : MonoBehaviour {

	public GameObject parentObject;
	public GameObject parentPlayer;
	public GameObject itemPickup;
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
		if (parentPlayer && parentPlayer.GetComponent<PlayerScript> ().is_transformed) {
			Destroy (this.gameObject);
		}
	}
}
