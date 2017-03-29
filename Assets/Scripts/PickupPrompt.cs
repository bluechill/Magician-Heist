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
			DestroyThis();
		}
		if (parentPlayer.GetComponent<PlayerScript> ().nearestActionObject != parentObject) {
			DestroyThis();
		}
		if (parentPlayer && parentPlayer.GetComponent<PlayerScript> ().is_transformed) {
			DestroyThis();
		}
	}
	void DestroyThis(){
		transform.localScale = Vector3.Lerp (transform.localScale, Vector3.zero, 0.1f);
		if (transform.localScale.magnitude <= 0.0001f) {
			Destroy (this.gameObject);
		}
	}
}
