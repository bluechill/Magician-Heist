using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckScript : MonoBehaviour {
    public int team_score;
	public int weight_used = 0;
	int capacity = 100;
	public GameObject room_text;
	public List<Collider> overStockItems;
	public List<GameObject> countedItems;
	public bool red_team;
	public GameObject bed_center_obj;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		room_text.GetComponent<TextMesh> ().text = "";
		room_text.GetComponent<TextMesh> ().text += weight_used.ToString ();
		FindContained ();
		FillSpace ();
		SpreadItems ();
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
				countedItems.Add (coll.gameObject);
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

			countedItems.Remove (coll.gameObject);

            print(team_score);
			print(capacity - weight_used);
        }
    }


	private void FindContained() {

		float boxWidth = 6f;
		float sign = 1.0f;

		if (this.transform.localEulerAngles.y >= 180f)
			sign *= -1f;

		Collider[] colliders = Physics.OverlapBox (bed_center_obj.transform.position, new Vector3 (boxWidth, 1.0f, 1.0f));

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

				countedItems.Add (overStockItems[i].gameObject);


				overStockItems.RemoveAt (i);


			} else {
				i++;
			}

		}
	}
	public void SpreadItems(){
		for (int i = 0; i < countedItems.Count; i++) {
			if (!countedItems [i].GetComponent<Item> ().held && !countedItems [i].GetComponent<Item> ().thrown) { 

				float x_;
				if (red_team) {
					x_ = (bed_center_obj.transform.position.x - 5f) + i;
					if (i > 10) {
						x_ = (bed_center_obj.transform.position.x - 5f) + (i - 10.5f);
					}

				} else {
					x_ = (bed_center_obj.transform.position.x + 5f) - i;
					if (i > 10) {
						x_ = (bed_center_obj.transform.position.x + 5f) - (i - 10.5f);
					}

				}
				if (!V3Equals (countedItems [i].transform.position, new Vector3 (x_, bed_center_obj.transform.position.y, 0f))) {
					countedItems [i].transform.position = Vector3.Lerp(countedItems [i].transform.position, new Vector3 (x_, bed_center_obj.transform.position.y, 0f), 0.05f) ;

				}


				if (countedItems [i].GetComponent<Item> ().tree) {
					if (!V3Equals (countedItems [i].transform.position, new Vector3 (countedItems [i].transform.position.x, bed_center_obj.transform.position.y + (0.18f), 0f))) {
						countedItems [i].transform.position = Vector3.Lerp(countedItems [i].transform.position, new Vector3 (countedItems [i].transform.position.x, bed_center_obj.transform.position.y + (0.15f), 0f), 0.05f) ;
					}

				} else if (!countedItems [i].GetComponent<Item> ().couch) {
					if (!V3Equals (countedItems [i].transform.position, new Vector3 (countedItems [i].transform.position.x, bed_center_obj.transform.position.y - (0.25f), 0f))) {
						countedItems [i].transform.position = Vector3.Lerp(countedItems [i].transform.position, new Vector3 (countedItems [i].transform.position.x, bed_center_obj.transform.position.y - (0.25f), 0f), 0.05f) ;
					}

				}

				
			}
		}
	}
	bool V3Equals(Vector3 vec_a, Vector3 vec_b){
		float x_dif = vec_a.x - vec_b.x;
		float y_dif = vec_a.y - vec_b.y;
		if (x_dif < 0)
			x_dif = x_dif * -1;

		if (y_dif < 0)
			y_dif = y_dif * -1;

		if (x_dif <= 0.3) {
			if (y_dif <= 0.3) {
				return true;
			}
		}
		return false;
	}

}
