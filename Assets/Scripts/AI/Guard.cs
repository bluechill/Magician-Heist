using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour {

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

	private int destinationPoint;
	private NavMeshAgent agent;
	private Animator animator;

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

		animator.speed = agent.speed;

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
	}

	void GoToNextPoint() 
	{
		if (patrolPoints.Length == 0)
			return;

		agent.destination = patrolPoints [destinationPoint].position;

		destinationPoint = (destinationPoint + 1) % patrolPoints.Length;
	}
}
