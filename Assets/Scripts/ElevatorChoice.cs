using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorChoice : MonoBehaviour {
	SpriteRenderer sprend;
	// Use this for initialization
	void Start () {
		sprend = GetComponent<SpriteRenderer> ();
		InvokeRepeating ("Blink", 0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponentInParent<Elevator>().choice) {
			transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, 180f));
		} else {
			transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, 0f));
		}
	}
	void Blink(){
		sprend.enabled = !sprend.enabled;
	}
}
