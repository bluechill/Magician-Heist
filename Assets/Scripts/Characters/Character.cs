using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (SpriteRenderer))]
public class Character : MonoBehaviour {

	//parent class for Character objects
	//	ex. Players, Guards, Police

	//required public components of a Character
	public Rigidbody rb;
	public SpriteRenderer sprend;

	//inspector settables separator
	public bool __________________;
	public float movement_velocity;
	// Use this for initialization
	public virtual void Start () {
		rb = GetComponent<Rigidbody> ();
		sprend = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
