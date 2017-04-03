using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapacityIndicator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localPosition = new Vector3(transform.localPosition.x, 1f, 0f);
	}
}
