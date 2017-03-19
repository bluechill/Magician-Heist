using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.Director;

public class Guard : LayerMonoBehavior {

	public enum GuardMode {
		Patrol,
		Search,
		Attack
	}
	public GuardMode mode;

	public float patrolSpeed = 2.0f;
	public float searchSpeed = 3.5f;

	public FieldOfView fov;

	public Transform[] patrolPoints;
	public Transform aiPointsOfInterst;

	public AnimationClip guardWalk;

	private int destinationPoint = 0;
	private NavMeshAgent agent;
	private Animator animator;
	public bool animating = true;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		agent.autoBraking = false;
		agent.updateRotation = false;

		GoToNextPoint ();
	}
	
	// Update is called once per frame
	void Update () {
		if (mode == GuardMode.Patrol)
			agent.speed = patrolSpeed;
		else if (mode == GuardMode.Search || mode == GuardMode.Attack)
			agent.speed = searchSpeed;

		if (agent.velocity.magnitude > 0f && !animating) {
			var playableClip = AnimationClipPlayable.Create (guardWalk);
			playableClip.speed = agent.speed;
			animator.Play (playableClip);
			animating = true;
		}

		if (agent.velocity.x <= 0.0f)
			this.transform.rotation = Quaternion.Euler(new Vector3(0f,180f,0f));
		else if (agent.velocity.x > 0.0f)
			this.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));

		if (agent.remainingDistance < 0.5f && (mode == GuardMode.Patrol || mode == GuardMode.Search))
			GoToNextPoint ();

		if (fov.visibleTargets.Count > 0) {
			agent.destination = fov.visibleTargets [0].transform.position;
			mode = GuardMode.Attack;
		}

		if (agent.isOnOffMeshLink && agent.currentOffMeshLinkData.linkType != OffMeshLinkType.LinkTypeManual)
			agent.CompleteOffMeshLink ();
		else if (agent.isOnOffMeshLink) {
			var obj = agent.currentOffMeshLinkData.offMeshLink;
			print (obj.tag);
			if (obj.GetComponent<Door> () != null) {
				bool open = obj.GetComponent<Door> ().open;

				if (!open) {
				}
					//obj.GetComponent<Door> ().OpenDoor ();
				else
					agent.ResetPath ();
			}
		}
	}

	void SetAnimationStopped() {
		animating = false;
	}

	void GoToNextPoint() 
	{
		if (patrolPoints.Length == 0)
			return;

		agent.destination = patrolPoints [destinationPoint].position;

		destinationPoint = (destinationPoint + 1) % patrolPoints.Length;
	}
}
