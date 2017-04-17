using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialImage : MonoBehaviour {

	public Sprite[] images;
	int i = 0;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("SwitchImage", 0.0f, 0.7f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void SwitchImage(){
		GetComponent<Image>().sprite = images [i];
		i = (i + 1) % images.Length;
	}
}
