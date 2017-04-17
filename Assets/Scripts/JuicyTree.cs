using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuicyTree : MonoBehaviour {

	private float time = 0.0f;
	public float switchTime = 0.6f;
	public float juiceAmount = 0.4f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		if (time > switchTime * 2.0f)
			time = 0.0f;

		float amount = (time / switchTime);

		if (amount > 1.0f)
			amount = 2.0f - amount;

		amount *= juiceAmount;
		
		this.transform.localScale = new Vector3 (1.0f + amount, 
			1.0f - amount / 5.0f, 
			1.0f);
	}
}
