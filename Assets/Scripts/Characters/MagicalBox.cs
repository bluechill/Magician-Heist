using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalBox : Ability {

	bool active = false;
	public int num_inside = 0;
	public List<GameObject> players_inside;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void UseAbility(){
		//reset position so it falls to the ground, just juice 
		Magician magician = GetComponentInParent<Magician> ();
		if (active) {
			magician.ExitBox (); 
			magician.Reappear (); 
			magician.using_ability = false;
			this.gameObject.SetActive (false);
			if(num_inside != 0) RemoveAllMagicians ();

		} else {
			//transform.position = Vector3.zero;
			magician.EnterBox (); 
			magician.Disappear (); 
			magician.using_ability = true;
		}
		active = !active;
	}
	//action function called when a player uses the magical box
	public void Action(GameObject mag){
		Magician magician = mag.GetComponent<Magician> ();
		if (magician.in_box) {
			magician.ExitBox ();
			RemoveMagician (mag);
			num_inside--;

		} else {
			magician.EnterBox ();
			num_inside++;
			players_inside.Add (mag);
		}
	
	}
	//removes a magician from the box on command
	public void RemoveMagician(GameObject mag){
		players_inside.Remove (mag);
	}
	//removes all magicians from the box on command
	public void RemoveAllMagicians(){
		for (int i = 0; i < num_inside; i++) {
			Magician magician = players_inside [i].GetComponent<Magician> ();
			magician.touching_box = false;
			magician.ExitBox ();
		}
		num_inside = 0;
		players_inside.Clear ();
	}
}
