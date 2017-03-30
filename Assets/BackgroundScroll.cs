using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour {
	public float scroll_speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector3 (transform.localPosition.x - scroll_speed, transform.localPosition.y, 0f);
		if (transform.localPosition.x <= -850f) {
			transform.localPosition = new Vector3 (1394f, transform.localPosition.y, 0f);
		}
	}
}
