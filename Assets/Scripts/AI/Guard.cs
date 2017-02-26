using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour {

	public enum GuardMode {
		Patrol
	}
	public GuardMode mode;

	public Transform[] patrolPoints;
	public Transform aiPointsOfInterst;

	private int destinationPoint;
	private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
		agent.autoBraking = false;

		GoToNextPoint ();
	}
	
	// Update is called once per frame
	void Update () {
		if (agent.remainingDistance < 0.5f)
			GoToNextPoint ();
	}

	void GoToNextPoint() 
	{
		if (patrolPoints.Length == 0)
			return;

		agent.destination = patrolPoints [destinationPoint].position;

		destinationPoint = (destinationPoint + 1) % patrolPoints.Length;
	}
}
