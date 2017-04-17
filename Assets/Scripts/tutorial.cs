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
        if(age > 105f)
        {
            if(msg) msg.SetActive(true);
        }
	}
}
