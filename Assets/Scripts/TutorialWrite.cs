using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWrite : MonoBehaviour {
	float write_speed = 0.15f;
	public bool write;
	public string input;
	string[] input_tokens;
	int index = 0;
	Text txt;
	// Use this for initialization
	void Start () {
		
	}
	public void Write(){
		write = true;
	}
	// Update is called once per frame
	void Update () {
		if (!txt)
			txt = GetComponent<Text> ();

		if (write) {
			write = false;
			txt.text = "";
			TokenizeInput ();
			WriteWord ();
		}
		
	}
	void WriteWord(){
		if (index == input_tokens.Length) {
			return;
		}
		txt.text += input_tokens [index++] + " "; 
		Invoke ("WriteWord", write_speed);
	}
	void TokenizeInput(){
		char[] delimit = { ' ' };
		input_tokens = input.Split(delimit);
	}
}
