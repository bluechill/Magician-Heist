using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoRotate : MonoBehaviour {

	public float moveTime = 2.0f;
	public float moveAmount = 2.0f;

	private Vector3 start;
	private float time;

	// Use this for initialization
	void Start () {
		start = this.transform.eulerAngles;
		time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		float amount = moveAmount * (time / moveTime);

		if (time > moveTime * 2.0f || amount >= 360)
			time = 0.0f;

		amount = moveAmount * (time / moveTime);

		if (time > moveTime)
			amount = moveAmount * (1 - (time - moveTime) / moveTime);

		this.transform.eulerAngles = new Vector3 (start.x, start.y, start.z + amount);
	}
}
