using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Director;

public class JumpyLamp : LayerMonoBehavior {
	
	public float animationSpeed = 2.0f;
	public float movementAmount = 1.0f;
	public float movementTime = 1.0f;

	public AnimationClip lampJump;

	private Animator animator;
	private Rigidbody rb;

	private bool animating = true;
	private bool increasing = true;
	private Vector3 startPos;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		animator.speed = animationSpeed;

		startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float amount = 0f;

		if ((this.transform.position - startPos).x >= movementAmount)
			increasing = false;
		else if ((this.transform.position - startPos).x <= 0f)
			increasing = true;

		if (increasing) {
			this.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 0f));
			amount = movementAmount / movementTime;
		} else {
			this.transform.rotation = Quaternion.Euler (new Vector3 (0f, 180f, 0f));
			amount = - movementAmount / movementTime;
		}

		rb.velocity = new Vector3(amount, 0f,0f);

		if (rb.velocity.magnitude > 0f && !animating) {
			var playableClip = AnimationClipPlayable.Create (lampJump);
			playableClip.speed = animationSpeed;
			animator.Play (playableClip);
			animating = true;
		}
	}

	void SetAnimationStopped() {
		animating = false;
	}
}
