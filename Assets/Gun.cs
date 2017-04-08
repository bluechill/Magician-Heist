using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
	public Transform exit_point;
	public Transform flash_point;
	public float bullet_speed;
	public GameObject bulletPrefab;
	public GameObject flamePrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Shoot(){
		GameObject bullet = MonoBehaviour.Instantiate (bulletPrefab);
		bullet.transform.position = exit_point.transform.position;
		float sign = 1;
		if (GetComponentInParent<PlayerScript> ().transform.localRotation.y != 0) {
			sign = -1;
		}
		bullet.GetComponent<Rigidbody> ().velocity = Vector3.right * sign * bullet_speed;
		bullet.GetComponent<Bullet> ().red_team = GetComponentInParent<PlayerScript> ().red_team;

		GameObject gunFlame = MonoBehaviour.Instantiate (flamePrefab);
		gunFlame.transform.position = flash_point.transform.position;
		Game.GameInstance.gun_shot.Play ();
	}
}
