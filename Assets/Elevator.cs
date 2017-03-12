using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

	public GameObject[] stair_sets;
	public float scroll_speed;
	int num_set;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("ResetSet", 1.75f, 1.75f);
		num_set = stair_sets.Length - 1;
	}
	
	// Update is called once per frame
	void Update () {

		MoveStairs ();



	}

	void MoveStairs(){
		float movement = Time.deltaTime * scroll_speed;
		for (int i = 0; i < stair_sets.Length; i++) {
			stair_sets [i].transform.localPosition = new Vector3 (stair_sets[i].transform.localPosition.x + movement, stair_sets[i].transform.localPosition.y, 0);
		}
	}
	void ResetSet(){
		stair_sets [num_set--].transform.localPosition = Vector3.zero;
		if (num_set == -1) {
			num_set = stair_sets.Length - 1;
		}
	}
}
