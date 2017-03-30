using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckScript : MonoBehaviour {
    public int team_score;
	public int weight_used = 0;
	int capacity = 100;
	public GameObject room_text;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		room_text.GetComponent<TextMesh> ().text = "";
		room_text.GetComponent<TextMesh> ().text += weight_used.ToString ();
	}

    private void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.CompareTag("Item")) {
			if (coll.gameObject.GetComponent<Item>().size <= capacity - weight_used) {
				coll.gameObject.GetComponent<Item> ().counted = true;
				weight_used += coll.gameObject.GetComponent<Item>().size; 
                team_score += coll.gameObject.GetComponent<Item>().points;
                print(team_score);
				print(capacity - weight_used);
            }
        }
    }

    private void OnTriggerExit(Collider coll) {

		if (coll.gameObject.CompareTag("Item") && coll.gameObject.GetComponent<Item> ().counted) {
			coll.gameObject.GetComponent<Item> ().counted = false;
			weight_used -= coll.gameObject.GetComponent<Item>().size;
            team_score -= coll.gameObject.GetComponent<Item>().points;
            print(team_score);
			print(capacity - weight_used);
        }
    }
}
