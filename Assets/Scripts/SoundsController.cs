using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsController : MonoBehaviour {
	public static SoundsController instance;

	public GameObject[] sound_objects;

	// Use this for initialization
	void Start () {
		
	}
	void Awake ()   
	{
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
	public bool IsPlaying(string name){
		return sound_objects [GetIndex (name)].GetComponent<AudioSource> ().isPlaying;
	}
	public void PlaySound(string name){
		sound_objects [GetIndex (name)].GetComponent<AudioSource> ().Play ();
	}
	int GetIndex(string name){
		switch (name) {
		case "main":
			return 0;
			break;
		default:
			break;

		}
		print ("error finding this sound: " + name);
		return -1;
	}

}
