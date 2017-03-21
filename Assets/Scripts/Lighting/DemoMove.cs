using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMove : MonoBehaviour {

	public float moveTime = 2.0f;
	public float moveAmount = 2.0f;

	private Vector3 start;
	private float time;

	// Use this for initialization
	void Start () {
		start = this.transform.position;
		time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		if (time > moveTime * 2.0f)
			time = 0.0f;

		float amount = moveAmount * (time / moveTime);

		if (time > moveTime)
			amount = moveAmount * (1 - (time - moveTime) / moveTime);

		this.transform.position = new Vector3 (start.x, start.y + amount, 0.0f);
	}
}
