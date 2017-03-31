using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Item : MonoBehaviour {
	public bool couch = false;
	public bool briefcase = false;
	public bool flash_light = false;
	public bool magical_key = false;
	public bool is_player = false;
	public bool gold_bar = false;
	public bool tree = false;
	public bool key_card = false;
	public bool held = false;
    public bool thrown = false;
	public int player_num = -1;
	public bool enabled = false;
	public GameObject current_player;
	public GameObject plus50_prefab;
    public Game gameScript;
	private MaterialPropertyBlock propertyBlock;
	private SpriteRenderer srend;
	public int points;
    	public int size;
	public bool counted = false;
	private bool canGetMorePoints = true;
	float lifespan = 0f;
	public bool kill = false;
	float age = 0f; 
	float kill_time = 0f; 
	int original_sortingOrder;
	// Use this for initialization
	void Start () {
		srend = GetComponent<SpriteRenderer>();
		original_sortingOrder = GetComponent<SpriteRenderer> ().sortingOrder;
	}
	
	// Update is called once per frame
	void Update () {
		if (kill) {
			age = Time.time - kill_time;
			if (age > lifespan){
				//DestroyThis ();
			}
		}

		if ((tree && held) || flash_light) {
			transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, 90f));
		} else {
			transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, 0f));
		}

		if (enabled) {
			GetComponent<Rigidbody> ().isKinematic = false;
			GetComponent<SpriteRenderer> ().sortingOrder = 30;
		} else {
			GetComponent<Rigidbody> ().isKinematic = true;
			GetComponent<SpriteRenderer> ().sortingOrder = original_sortingOrder;
		}

        if (GetComponent<Rigidbody>().velocity.magnitude <= 0.1f)
        {

        }
		if (GetComponent<Rigidbody> ().velocity.magnitude <= 0.01f || (GetComponent<Rigidbody> ().velocity.y <= 0.0001f && GetComponent<Rigidbody> ().velocity.y >= -0.0001f)) {
			thrown = false;
			GetComponent<SpriteRenderer> ().sortingOrder = original_sortingOrder;
			gameObject.transform.GetChild (0).gameObject.layer = 10;
		}
	}

	public void SetPlayer(GameObject obj, int n){
		current_player = obj;
		is_player = true;
		player_num = n;
	}
	public void ResetPlayer(){
		current_player = null;
		is_player = false;
		player_num = -1;
	}
    public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject.transform.parent != null) {
			//print ("BUMP");
			//print (other);

			PlayerScript p = other.gameObject.transform.parent.GetComponent<PlayerScript> ();
			//print (!p.is_knocked_out);
			//print (thrown);
			//print (current_player != p.gameObject);
			//print (!p.forceField.activeSelf);
			//print (GetComponent<Rigidbody> ().velocity);

			if (p != null && !p.is_knocked_out && thrown && current_player != p.gameObject && !p.forceField.activeSelf && (p.red_team != current_player.GetComponent<PlayerScript>().red_team)) {
				p.KnockOut ();
				current_player.GetComponent<PlayerScript> ().points += 50;
                print(current_player);
                if (current_player.GetComponent<PlayerScript>().red_team)
					Game.GameInstance.red_team_score += 10;
                else
					Game.GameInstance.blue_team_score += 10;
                var plus50 = Instantiate (plus50_prefab);
				plus50.transform.position = p.transform.position + Vector3.up * 0.5f;
				//print ("In if statement ");
				thrown = false;
				gameObject.transform.GetChild (0).gameObject.layer = 10;
				GetComponent<Rigidbody> ().velocity = Vector3.zero;
			}
		} else if (other.gameObject.tag == "Gold Trigger" && current_player != null && canGetMorePoints) {
			current_player.GetComponent<PlayerScript> ().points += 50;
			var plus50 = Instantiate (plus50_prefab);
			plus50.transform.position = this.transform.position + Vector3.up * 0.1f;
			canGetMorePoints = false;
			lifespan = 1f;
			kill_time = Time.time;
			kill = true;
		}
    }
	void DestroyThis(){
		transform.localScale = Vector3.Lerp (transform.localScale, Vector3.zero, 0.1f);
		if (transform.localScale.magnitude <= 0.0001f) {
			Destroy (this.gameObject);
		}
	}
}
