using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPopup : MonoBehaviour {
	TextMesh txt;
	Rigidbody rb;
	bool fade = false;
	// Use this for initialization
	void Start () {
		txt = GetComponent<TextMesh> ();
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		MoveUp ();
		if(fade) Fade ();
		GetComponent<Renderer> ().sortingOrder = 30;

	}
	public void SetValue(int value, bool add){
		if(!txt) txt = GetComponent<TextMesh> ();
		if(!rb) rb = GetComponent<Rigidbody> ();
		txt.text = "";
		if (add) {
			txt.text += "+";
			txt.color = Color.yellow;
		} else {
			txt.text += "-";
			txt.color = Color.red;
		}
		txt.text += value.ToString();
		Invoke ("SetFade", 0.25f);
	}
	void MoveUp(){
		rb.AddForce (Vector3.up * 3f);
		rb.AddForce (Vector3.right * 1.5f);
	}
	void Fade(){
		txt.color = Color.Lerp (txt.color, new Color(txt.color.r, txt.color.g, txt.color.b, 0f), 0.1f);
		if (txt.color.a <= 0.01f) {
			Destroy (this.gameObject);
		}
	}
	void SetFade(){
		fade = true;
	}
}
