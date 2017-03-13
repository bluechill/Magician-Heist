using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGuard : MonoBehaviour {
	bool right = false;
	Rigidbody rb;
	Animator animator;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (right) {
			rb.velocity = Vector3.right * 3f;
			Animate ();
		} else {
			rb.velocity = Vector3.left * 3f;
			Animate ();
		}
		ProcessRotation ();
	}
	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.tag == "Guard Constraint") {
			right = !right;
		} else if (coll.gameObject.tag == "Player") {
			if (!coll.gameObject.GetComponentInParent<PlayerScript> ().is_hiding) {
				KnockOut (coll.gameObject);
			}
		}
	}

	void Animate(){
		animator.speed = 2f * rb.velocity.magnitude;
		animator.SetBool ("idle", false);

	}
	void Idle(){
		animator.speed = 0.4f;
		animator.SetBool ("idle", true);

	}
	void ProcessRotation(){

		if (rb.velocity.x < -0.05f)
			this.transform.rotation = Quaternion.Euler(new Vector3(0f,180f,0f));
		else if (rb.velocity.x > 0.05f)
			this.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));

	}
	void KnockOut(GameObject player){
		player.GetComponentInParent<PlayerScript> ().KnockOut ();
	}
}
