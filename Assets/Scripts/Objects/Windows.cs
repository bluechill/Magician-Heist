using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windows : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Renderer>().material.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Break() {
       foreach (Transform t in transform) {
            if (t.name == "Horizontal Cross Beam")
                Destroy(t);
            else if (t.name == "Vertical Cross Beam")
                Destroy(t);
        }
    }
}
