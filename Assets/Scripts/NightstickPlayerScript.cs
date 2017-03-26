using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
public class NightstickPlayerScript : PlayerScript {

	// Update is called once per frame
	void Update () {
		age = Time.time - birthtime;
		if (!started) {
			started = true;
			pointsText = GameObject.FindGameObjectWithTag ("Points Text 3").GetComponent<Text> ();
		}
		ProcessMovement ();
		ProcessRotation ();
		ProcessActions ();
		ProcessHold ();
		ProcessTransformed ();

		this.GetComponent<PlayerScript> ().Update ();

		if (is_ability && is_being_held)
		{
			if(!ability.GetComponent<Item>().thrown) transform.position =  Vector3.Lerp(transform.position, ability.transform.position, 0.1f);
		}
	}

}
