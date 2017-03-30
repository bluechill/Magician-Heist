using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckScript : MonoBehaviour {
    public int team_score;
    public int room_remaining;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.CompareTag("Item")) {
            if (coll.gameObject.GetComponent<Item>().size <= room_remaining) {
                room_remaining -= coll.gameObject.GetComponent<Item>().size; 
                team_score += coll.gameObject.GetComponent<Item>().points;
                print(team_score);
                print(room_remaining);
            }
        }
    }

    private void OnTriggerExit(Collider coll) {
        if (coll.gameObject.CompareTag("Item")) {
            room_remaining += coll.gameObject.GetComponent<Item>().size;
            team_score -= coll.gameObject.GetComponent<Item>().points;
            print(team_score);
            print(room_remaining);
        }
    }
}
