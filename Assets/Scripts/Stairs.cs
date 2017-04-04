using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {
	public GameObject destination;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Use(GameObject player){
		player.transform.position = new Vector3 (destination.transform.position.x, destination.transform.position.y, destination.transform.position.z) ;
	}

}
