using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpEscalator : MonoBehaviour {

	public GameObject entrance;
	public Color activated_color;
	Color original_color;
	public bool activated = false;
	public bool in_use = false;
	public int num_using = 0;
	public int num_touching = 0;
	public List<GameObject> touching_players;
	public GameObject exit_collider;
	public GameObject entrance_collider;
	// Use this for initialization
	void Start () {
		original_color = entrance.GetComponent<SpriteRenderer> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		SetColor ();
	}
	void SetColor(){
		if (activated)
			entrance.GetComponent<SpriteRenderer>().color = activated_color;
		else 
			entrance.GetComponent<SpriteRenderer>().color = original_color;
	}
	public void AddPlayer(GameObject pla){
		touching_players.Add (pla);
		num_touching++;
		activated = true;
	}
	public void RemovePlayer(GameObject pla){
		touching_players.Remove (pla);
		num_touching--;
		if (num_touching == 0) {
			activated = false;
		}
	}
	public void UseEscalator(){
		num_using = touching_players.Count;
		foreach (GameObject player in touching_players) {
			player.GetComponent<PlayerScript> ().UseUpEscalator ();
		}
	}
}
