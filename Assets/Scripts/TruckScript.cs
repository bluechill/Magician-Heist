using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TruckScript : MonoBehaviour {
    public GameObject dropOff;
	public float capacity_percent = 0f;
	public Text score_text;
    public int team_score;
	public int weight_used = 0;
	public int capacity = 100;
	public GameObject room_text;
	public List<Collider> overStockItems;
    public List<GameObject> countedItems;
    public List<GameObject> extraItems;
    public bool red_team;
	public GameObject bed_center_obj;
	public GameObject itemWall;
	public GameObject capacity_indicator;
	public GameObject indicators;
	public GameObject itemWallSprite;
	public GameObject moneyPopupPrefab;

	public GameObject[] players;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		capacity_percent = ((weight_used * 1f) / (capacity * 1f));
		if (capacity_percent >= 1f) {
			itemWallSprite.SetActive (true);
		} else {
			itemWallSprite.SetActive (false);
		}
		room_text.GetComponent<TextMesh> ().text = "";
		room_text.GetComponent<TextMesh> ().text += weight_used.ToString ();
		//FindContained ();
		//FillSpace ();
		
		UpdateScore ();
		UpdateCapacity ();
        SpreadItems2();
    }

    private void OnTriggerEnter(Collider coll) {

		/*
        if (coll.gameObject.CompareTag("Item")) {

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
        */
    }

    private void OnTriggerExit(Collider coll) {
		/*
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
        */
    }
	public void RemoveItem(GameObject item){
		if (countedItems.Contains (item)) {

			if (red_team) {
				Game.GameInstance.red_team_score -= item.GetComponent<Item>().points;
			} else {
				Game.GameInstance.blue_team_score -= item.GetComponent<Item>().points;
			}
			GameObject moneyPopup1 = MonoBehaviour.Instantiate (moneyPopupPrefab);
			moneyPopup1.GetComponent<MoneyPopup> ().SetValue (item.GetComponent<Item> ().points, false);
			moneyPopup1.transform.position = new Vector3(players [0].transform.position.x, players [0].transform.position.y + 2f, -10f);
			GameObject moneyPopup2 = MonoBehaviour.Instantiate (moneyPopupPrefab);
			moneyPopup2.GetComponent<MoneyPopup> ().SetValue (item.GetComponent<Item> ().points, false);
			moneyPopup2.transform.position = new Vector3(players [1].transform.position.x, players [1].transform.position.y + 2f, -10f);
			Game.GameInstance.itemRemoval.Play ();
			countedItems.Remove (item);

		}
	}
	void Cheer(){
		Game.GameInstance.cheering.Play ();
		Game.GameInstance.FunnyQuote ();
	}

    void KickoutItem()
    {
        if (capacity_percent <= 1f)
        {
            return;
        }
        int weight = 0;
        List<GameObject> removals = new List<GameObject>();
        foreach(GameObject item in countedItems)
        {
            weight += item.GetComponent<Item>().size;
            if(weight > capacity)
            {
                item.transform.position = dropOff.transform.position;
                RemoveItem(item);
                return;
            }
        }
    }
	void UpdateCapacity(){
		float boxWidth = 7.5f;
		float sign = 1.0f;

		if (this.transform.localEulerAngles.y >= 180f)
			sign *= -1f;

		Collider[] colliders = Physics.OverlapBox (bed_center_obj.transform.position, new Vector3 (boxWidth, 1.0f, 1.0f));

		if (colliders.Length == 0)
			return;

		List<Collider> taggedColliders = new List<Collider> ();
		weight_used = 0;
		for (int i = 0; i < colliders.Length; ++i) {
			if (colliders [i].gameObject.tag != "Bullet") {
				
				if (colliders [i].GetComponent<SpriteGlow> () != null && !colliders [i].GetComponent<Item> ().held && !colliders [i].GetComponent<Item> ().thrown &&
				   !colliders [i].transform.IsChildOf (this.transform)) {

					weight_used += colliders [i].gameObject.GetComponent<Item> ().size;

					if (colliders [i].gameObject.GetComponent<Item> ().counted == false) {

                        if (red_team) {
							Game.GameInstance.red_team_score += colliders [i].gameObject.GetComponent<Item> ().points;
						} else {
							Game.GameInstance.blue_team_score += colliders [i].gameObject.GetComponent<Item> ().points;
						}
						GameObject moneyPopup1 = MonoBehaviour.Instantiate (moneyPopupPrefab);
						moneyPopup1.GetComponent<MoneyPopup> ().SetValue (colliders [i].gameObject.GetComponent<Item> ().points, true);
						moneyPopup1.transform.position = new Vector3 (players [0].transform.position.x, players [0].transform.position.y + 2f, -10f);
						GameObject moneyPopup2 = MonoBehaviour.Instantiate (moneyPopupPrefab);
						moneyPopup2.GetComponent<MoneyPopup> ().SetValue (colliders [i].gameObject.GetComponent<Item> ().points, true);
						moneyPopup2.transform.position = new Vector3 (players [1].transform.position.x, players [1].transform.position.y + 2f, -10f);
						Game.GameInstance.chaChing.Play ();
						if (colliders [i].gameObject.GetComponent<Item> ().points >= 80 || colliders [i].gameObject.GetComponent<Item> ().money_grade == 4) {
							Invoke ("Cheer", 0.25f);
						}
					}

					colliders [i].gameObject.GetComponent<Item> ().counted = true;
					colliders [i].gameObject.GetComponent<Item> ().curr_Truck = this.gameObject;
					if (!countedItems.Contains (colliders [i].gameObject))
						countedItems.Add (colliders [i].gameObject);
				}
			}
		}

        KickoutItem();


		if (taggedColliders.Count == 0) {
			return;
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
			if (colliders [i].gameObject.tag != "Bullet") {
				if (colliders [i].GetComponent<SpriteGlow> () != null && !colliders [i].GetComponent<Item> ().counted &&
				   !colliders [i].transform.IsChildOf (this.transform))
					taggedColliders.Add (colliders [i]);
			}
		}

		if (taggedColliders.Count == 0) {
			overStockItems.Clear ();
			return;
		}

		for (int i = 0; i < taggedColliders.Count; i++) {
			//taggedColliders [i].GetComponent<SpriteGlow> ().enabled = true;
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
	public void UpdateScore(){
		score_text.text = "$";
		if (red_team) {
			score_text.text += Game.GameInstance.red_team_score.ToString();
		} else {
			score_text.text += Game.GameInstance.blue_team_score.ToString();
		}

	}
	public void SpreadItems(){



		for (int i = 0; i < countedItems.Count; i++) {
			if (!countedItems [i].GetComponent<Item> ().held && !countedItems [i].GetComponent<Item> ().thrown) { 

				float x_;
				int sign = 1;
				if (red_team)
					sign = -1;
				x_ = (bed_center_obj.transform.position.x + (sign * 5.5f)) - (sign * (i * 12f/countedItems.Count));


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
	public void SpreadItems2(){

		float size_used = 0f;
		float start_x = 0f;
		int sign = 1;
		if (red_team)
			sign = 1;
		else
			sign = -1;
		
		start_x = (bed_center_obj.transform.position.x - (sign *  6.5f));


			
		for (int i = 0; i < countedItems.Count; i++) {
			if (!countedItems [i].GetComponent<Item> ().held && !countedItems [i].GetComponent<Item> ().thrown) { 

				float x_ = start_x + (sign * size_used) + (sign * countedItems [i].GetComponent<Item> ().width / 2f);
				size_used += countedItems [i].GetComponent<Item> ().width;



				if (!V3EqualsFine (countedItems [i].transform.position, new Vector3 (x_, bed_center_obj.transform.position.y, 0f))) {
					countedItems [i].transform.position = Vector3.Lerp(countedItems [i].transform.position, new Vector3 (x_, bed_center_obj.transform.position.y, 0f), 0.05f) ;
					countedItems [i].GetComponent<Rigidbody> ().velocity = Vector3.zero;
				}


				if (countedItems [i].GetComponent<Item> ().tree) {
					if (!V3EqualsFine (countedItems [i].transform.position, new Vector3 (countedItems [i].transform.position.x, bed_center_obj.transform.position.y + (0.18f), 0f))) {
						countedItems [i].transform.position = Vector3.Lerp(countedItems [i].transform.position, new Vector3 (countedItems [i].transform.position.x, bed_center_obj.transform.position.y + (0.15f), 0f), 0.05f) ;
						countedItems [i].GetComponent<Rigidbody> ().velocity = Vector3.zero;
					}

				} else if (!countedItems [i].GetComponent<Item> ().couch) {
					if (!V3EqualsFine (countedItems [i].transform.position, new Vector3 (countedItems [i].transform.position.x, bed_center_obj.transform.position.y - (0.25f), 0f))) {
						countedItems [i].transform.position = Vector3.Lerp(countedItems [i].transform.position, new Vector3 (countedItems [i].transform.position.x, bed_center_obj.transform.position.y - (0.25f), 0f), 0.05f) ;
						countedItems [i].GetComponent<Rigidbody> ().velocity = Vector3.zero;
					}

				}


			}
		}

		int p = 0;
		if (red_team) {
			foreach (Transform indicator in indicators.transform) {

				indicator.gameObject.SetActive (false);

			}

			foreach (Transform indicator in indicators.transform) {
				p++;
				if ((p * 1f) / 100f > capacity_percent) {
					return;
				}
				indicator.gameObject.SetActive (true);

			}
		} else {

			List<Transform> indicator_transforms = new List<Transform>();
			foreach (Transform indicator in indicators.transform) {

				indicator_transforms.Add (indicator);

			}
			indicator_transforms.Reverse ();
			foreach (Transform indicator in indicator_transforms) {

				indicator.gameObject.SetActive (false);

			}
			foreach (Transform indicator in indicator_transforms) {
				p++;
				if ((p * 1f) / 100f > capacity_percent) {
					return;
				}
				indicator.gameObject.SetActive (true);

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
	bool V3EqualsFine(Vector3 vec_a, Vector3 vec_b){
		float x_dif = vec_a.x - vec_b.x;
		float y_dif = vec_a.y - vec_b.y;
		if (x_dif < 0)
			x_dif = x_dif * -1;

		if (y_dif < 0)
			y_dif = y_dif * -1;

		if (x_dif <= 0.01) {
			if (y_dif <= 0.01) {
				return true;
			}
		}
		return false;
	}

}
