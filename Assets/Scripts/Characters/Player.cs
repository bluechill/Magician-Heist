using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Director;


public class Player : MonoBehaviour {
	float movement = 5f;
	public float animation_speed;
	bool left = false;
	bool right = false;
	Rigidbody rb;
	public AnimationClip walkClip;
	Animator animator;
	public bool animating = true;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
		ProcessKeyboard ();
		ProcessVelocity ();
	}

	void ProcessKeyboard(){
		if (Input.GetKeyDown (KeyCode.W)) {
			left = true;
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			right = true;
		}
		if (Input.GetKeyUp (KeyCode.W)) {
			left = false;
		}
		if (Input.GetKeyUp (KeyCode.E)) {
			right = false;
		}
	}
	void ProcessVelocity(){
		if (left && !right) {
			rb.velocity = Vector3.left * movement;
		} else if (right && !left) {
			rb.velocity = Vector3.right * movement;
		} else {
			rb.velocity = Vector3.zero;
		}

		if (rb.velocity.magnitude > 0f && !animating) {
//			var playableClip = AnimationClipPlayable.Create (walkClip);
//			playableClip.speed = rb.velocity.magnitude;
//			animator.Play (playableClip);
//			animating = true;
		}

	}

	void SetAnimationStopped() {
		animating = false;
	}
}
