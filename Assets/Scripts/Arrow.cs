using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	bool up = false;
	bool down = true;
	float original_y;
	// Use this for initialization
	void Start () {
		//Invoke ("Bounce", 1f);
		original_y = transform.localPosition.y;
	}
	public void SetOriginal(){
		original_y = transform.localPosition.y;
	}
	// Update is called once per frame
	void Update () {
		if (up) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3(transform.localPosition.x, original_y, 0f), 0.1f);
			if (transform.localPosition.y - original_y <= 0.1f && transform.localPosition.y - original_y >= -0.1f) {
				up = false;
				down = true;
			}
		} 
		if (down) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3(transform.localPosition.x, original_y - 1f, 0f), 0.2f);
			if (transform.localPosition.y - (original_y - 1f) <= 0.1f && transform.localPosition.y - (original_y - 1f) >= -0.1f) {
				down = false;
				up = true;
			}
		}
	}

}
