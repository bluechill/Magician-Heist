using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupPrompt : MonoBehaviour {

	public Sprite[] grades;
	public GameObject parentObject;
	public GameObject parentPlayer;
	public GameObject itemPickup;
	public GameObject moneyGrade;
	TextMesh money_text;
	// Use this for initialization
	void Start () {
		money_text = moneyGrade.GetComponent<TextMesh> ();
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
	public void SetGrade(int grade){
		moneyGrade.GetComponent<MeshRenderer> ().sortingOrder = 31;
		moneyGrade.GetComponent<TextMesh> ().text = "";
		for (int i = 0; i < grade; i++) {
			moneyGrade.GetComponent<TextMesh> ().text += "$";
		}
		moneyGrade.GetComponent<TextMesh> ().color = Color.green;

		if (grade <= 1) {
			moneyGrade.GetComponent<TextMesh> ().color = Color.green;
		} else if (grade == 4) {
            moneyGrade.GetComponent<TextMesh> ().color = new Color (0.831f, 0.686f, 0.216f);
        }
	}
}
