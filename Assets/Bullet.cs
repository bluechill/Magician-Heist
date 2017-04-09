using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	float birth;
	float age;
	float life = 3f;
	public bool red_team;
	public GameObject smokePrefab;
	public GameObject gunSpark;
	// Use this for initialization
	void Start () {
		birth = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<TrailRenderer>().sortingOrder = 5;
		age = Time.time - birth;
		if (age > life)
			Destroy (this.gameObject);
		if (age > 1f) {
			GetComponent<TrailRenderer> ().startColor = Color.Lerp (GetComponent<TrailRenderer> ().startColor, new Color(GetComponent<TrailRenderer> ().startColor.r,GetComponent<TrailRenderer> ().startColor.g,GetComponent<TrailRenderer> ().startColor.b,0f), 0.1f);
			GetComponent<TrailRenderer> ().endColor = Color.Lerp (GetComponent<TrailRenderer> ().endColor, new Color(GetComponent<TrailRenderer> ().endColor.r,GetComponent<TrailRenderer> ().endColor.g,GetComponent<TrailRenderer> ().endColor.b,0f), 0.1f);
		}
	}
	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.tag == "Wall") {
			Destroy (this.gameObject);

		}
		if (GetComponent<Rigidbody> ().isKinematic)
			return;
		Game.GameInstance.gun_ricochet.Play ();
		GameObject spark = MonoBehaviour.Instantiate (gunSpark);
		spark.transform.position = transform.position;
		GetComponent<SpriteRenderer> ().enabled = false;
		GetComponent<Rigidbody> ().isKinematic = true;
		//Destroy (this.gameObject);

	}
}
