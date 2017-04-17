using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedFade : MonoBehaviour {
    public float lifespan;
    float age;
    float birth;
    bool kill = false;
    public bool end;
	// Use this for initialization
	void Start () {
        birth = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        age = Time.time - birth;
        if (age > lifespan) kill = true;
        if (kill) Kill();
	}
    void Kill()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.1f);
        if (transform.localScale.x < 0.01f)
        {
            if (end) Game.GameInstance.tutorial_done = true;
            Destroy(this.gameObject);
        }
    }
}
