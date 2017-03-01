using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_UI : MonoBehaviour {

	public GameObject player;
	public GameObject player_icon;
	public GameObject item_icon;
	Image player_img;
	Image item_img;
	Magician magician;
	public Sprite red_x;
	public Sprite[] item_sprites;
	// Use this for initialization
	void Start () {
		player_img = player_icon.GetComponent<Image> ();
		item_img = item_icon.GetComponent<Image> ();
		magician = player.GetComponent<Magician> ();
	}
	
	// Update is called once per frame
	void Update () {
		item_img.sprite = GetItemSprite ();
	}
	Sprite GetItemSprite (){
		if (magician.held_item == null) {
			return red_x;
		}
		return GetSprite (magician.held_item.tag);
	}
	Sprite GetSprite(string str){
		switch (str) {
		case "Lamp":
			return item_sprites [0];
		case "Item":
			return magician.held_item.GetComponent<SpriteRenderer> ().sprite;
		default:
			return red_x;
		}
	}
}
