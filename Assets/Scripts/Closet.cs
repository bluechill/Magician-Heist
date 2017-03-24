using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet : MonoBehaviour {

	public bool open = false;
	public bool fridge = false;
	public SpriteRenderer sprend;
	public List<GameObject> players_in;
	public Sprite[] sprites;
	float original_x;
	// Use this for initialization
	void Start () {
		sprend = GetComponent<SpriteRenderer> ();
		original_x = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (open) {
			sprend.sprite = sprites [1];
			if (fridge) {
				transform.position = new Vector3 (original_x, transform.position.y, 0f);
			} else {
				transform.position = new Vector3 (original_x - 0.5f, transform.position.y, 0f);
			}
		} else {
			sprend.sprite = sprites [0];
			if (fridge) {
				transform.position = new Vector3 (original_x, transform.position.y, 0f);
			} else {
				transform.position = new Vector3 (original_x, transform.position.y, 0f);
			}
		}
		ProcessIn ();
	}
	public void SwitchStates(){
		open = !open;
	}
	public void GetIn(GameObject player){
		players_in.Add (player);
	}
	public void GetOut(GameObject player){
		players_in.Remove (player);
	}
	void ProcessIn(){
		int i = 0;
		foreach (GameObject player in players_in) {
			if(open) player.GetComponent<PlayerScript> ().ReappearBody();
			else player.GetComponent<PlayerScript> ().DisappearBody();
			player.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			float offset = 0.2f;
			if (fridge)
				offset = -0.3f;
			player.transform.position = new Vector3 (transform.position.x + offset + (0.3f * i), transform.position.y - 0.1f, 0f);
			i++;
		}
	}
}
