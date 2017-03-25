﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Item : MonoBehaviour {
	public bool briefcase = false;
	public bool flash_light = false;
	public bool magical_key = false;
	public bool is_player = false;
	public bool gold_bar = false;
	public bool tree = false;
	public bool key_card = false;
	public bool held = false;
    public bool thrown = false;
	public int player_num = -1;
	public bool enabled = false;
	public GameObject current_player;
	private MaterialPropertyBlock propertyBlock;
	private SpriteRenderer srend;
	// Use this for initialization
	void Start () {
		srend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((tree && held) || flash_light) {
			transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, 90f));
		} else {
			transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, 0f));
		}

		if (enabled) {
			GetComponent<Rigidbody> ().isKinematic = false;
			GetComponent<SpriteRenderer> ().sortingOrder = 30;
		} else {
			GetComponent<Rigidbody> ().isKinematic = true;
		}

        if (GetComponent<Rigidbody>().velocity.magnitude <= 0.1f)
        {
			thrown = false;
        }
	}

	public void SetPlayer(GameObject obj, int n){
		current_player = obj;
		is_player = true;
		player_num = n;
	}
	public void ResetPlayer(){
		current_player = null;
		is_player = false;
		player_num = -1;
	}
    public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player" && other.gameObject.transform.parent != null)
        {
			PlayerScript p = other.gameObject.transform.parent.GetComponent<PlayerScript>();

			if (p != null && !p.is_knocked_out && thrown && current_player != p.gameObject) {
				p.KnockOut ();
				thrown = false;
				GetComponent<Rigidbody> ().velocity = Vector3.zero;
			}
        }
    }
}
