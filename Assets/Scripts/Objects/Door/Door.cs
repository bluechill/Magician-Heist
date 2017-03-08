using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Director;

public class Door : LayerMonoBehavior {

	public bool open = false;
	public bool animating = false;
	public AnimationClip doorOpen;

	private Animator animator;
	private OffMeshLink link;

	void Start() {
		animator = GetComponent<Animator> ();
		link = GetComponent<OffMeshLink> ();
		animator.Stop ();
	}

	// Use this for initialization
	public void OpenDoor()
	{
		if (!open && !animating) {
			animating = true;

			var playableClip = AnimationClipPlayable.Create (doorOpen);
			animator.speed = 1.0f;
			animator.Play (playableClip);
		}
	}

	void SetOpenDoor()
	{
		open = true;
	}

	public void CloseDoor()
	{
		if (open && !animating) {
			animating = true;

			var playableClip = AnimationClipPlayable.Create (doorOpen);
			animator.speed = -1.0f;
			animator.Play (playableClip);
		}
	}

	void SetCloseDoor()
	{
		open = false;
	}

	void FinishAnimating()
	{
		animating = false;
	}
}
