using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckScript : MonoBehaviour {
    public int team_score;
	public int weight_used = 0;
	int capacity = 100;
	public GameObject room_text;
	public List<Collider> overStockItems;
	public bool red_team;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		room_text.GetComponent<TextMesh> ().text = "";
		room_text.GetComponent<TextMesh> ().text += weight_used.ToString ();
		FindContained ();
		FillSpace ();
	}

    private void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.CompareTag("Item")) {
			if (coll.gameObject.GetComponent<Item>().size <= capacity - weight_used) {
				coll.gameObject.GetComponent<Item> ().counted = true;
				weight_used += coll.gameObject.GetComponent<Item>().size; 
				if (red_team) {
					Game.GameInstance.red_team_score += coll.gameObject.GetComponent<Item> ().points;
				} else {
					Game.GameInstance.blue_team_score += coll.gameObject.GetComponent<Item> ().points;
				}

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

			if (red_team) {
				Game.GameInstance.red_team_score -= coll.gameObject.GetComponent<Item> ().points;
			} else {
				Game.GameInstance.blue_team_score -= coll.gameObject.GetComponent<Item> ().points;
			}


            print(team_score);
			print(capacity - weight_used);
        }
    }


	private void FindContained() {

		float boxWidth = 5f;
		float sign = 1.0f;

		if (this.transform.localEulerAngles.y >= 180f)
			sign *= -1f;

		Collider[] colliders = Physics.OverlapBox (this.transform.position + new Vector3(boxWidth / 2f * sign, 0f, 0f), new Vector3 (boxWidth, 1.0f, 1.0f));

		if (colliders.Length == 0)
			return;

		List<Collider> taggedColliders = new List<Collider> ();

		for (int i = 0; i < colliders.Length; ++i) {
			if (colliders[i].GetComponent<SpriteGlow>() != null && !colliders[i].GetComponent<Item>().counted && 
				!colliders[i].transform.IsChildOf(this.transform) )
				taggedColliders.Add (colliders [i]);
		}

		if (taggedColliders.Count == 0) {
			overStockItems.Clear ();
			return;
		}

		for (int i = 0; i < taggedColliders.Count; i++) {
			taggedColliders [i].GetComponent<SpriteGlow> ().enabled = true;
		}
		overStockItems = taggedColliders;

	}
	public void FillSpace(){
		if (weight_used >= 100)
			return;
		if (overStockItems.Count == 0)
			return;

		int i = 0;
		while (weight_used < 100 && i < overStockItems.Count) {
			if (overStockItems [i].gameObject.GetComponent<Item> ().size + weight_used <= 100) {
				overStockItems [i].gameObject.GetComponent<Item> ().counted = true;
				weight_used += overStockItems [i].gameObject.GetComponent<Item> ().size; 
				team_score += overStockItems [i].gameObject.GetComponent<Item> ().points;
				overStockItems [i].gameObject.GetComponent<SpriteGlow> ().enabled = false;

				if (red_team) {
					Game.GameInstance.red_team_score += overStockItems [i].gameObject.GetComponent<Item> ().points;
				} else {
					Game.GameInstance.blue_team_score += overStockItems [i].gameObject.GetComponent<Item> ().points;
				}



				overStockItems.RemoveAt (i);


			} else {
				i++;
			}

		}
	}
}
