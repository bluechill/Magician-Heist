using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour {
    public bool use;
    float age;
    float birth;
    public GameObject msg;
	// Use this for initialization
	void Start () {
        birth = Time.time;	
	}
	
	// Update is called once per frame
	void Update () {
        age = Time.time - birth;
		if(CheckReady() && msg) msg.SetActive(true);
        if(age > 105f)
        {
            if(msg) msg.SetActive(true);
        }
	}
	bool CheckReady(){
		return Game.GameInstance.player1.ready && Game.GameInstance.player2.ready && Game.GameInstance.player3.ready && Game.GameInstance.player4.ready;
	}
}
