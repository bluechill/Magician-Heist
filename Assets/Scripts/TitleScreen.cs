using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;
public class TitleScreen : MonoBehaviour {
	bool starting = false;
	public GameObject black;
    bool cooldown = false;
	public bool kill = false;
	public bool grow = true;
    bool tutorial = false;
	public GameObject selector;
	public AudioClip mainLoop;
	public GameObject[] kill_objs;
	// Use this for initialization
	void Start () {
		if (mainLoop != null) {
			SoundsController.instance.StopPlaying ();
			SoundsController.instance.QueueClip (mainLoop);
			SoundsController.instance.EnableLooping ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 4; i++) {
			if (InputManager.Devices.Count > i) {
                if (InputManager.Devices[i].Action1 && !starting)
                {
                    if (!cooldown)
                    {
						starting = true;
                        Game.GameInstance.beep.Play();
                        cooldown = true;
                        Invoke("BeepCD", 0.5f);
						Kill();
                    }
                }
                if (InputManager.Devices[i].Action2 && !starting)
                {
                    if (!cooldown)
                    {
						starting = true;
						tutorial = true;
                        Game.GameInstance.beep.Play();
                        cooldown = true;
                        Invoke("BeepCD", 0.5f);
						Kill();
                    }
                }
            }
		}
		if (grow) {
			GrowThis ();
		}
		if (kill) {
			DestroyThis ();
		}
		
	}
	public void Kill(){
		grow = false;
		kill = true;
	}
	void DestroyThis(){
		black.SetActive (true);



		for (int i = 0; i < kill_objs.Length; i++) {
			kill_objs[i].transform.localScale = Vector3.Lerp (kill_objs[i].transform.localScale, new Vector3(0f, 1f, 1f), 0.13f);
			if (kill_objs[i].transform.localScale.x <= 0.01f) {
				if (tutorial)
					SceneManager.LoadScene ("Tutorial Level");
				else {
					selector.SetActive (true);
					Destroy (this.transform.parent.parent.gameObject);
				}
			}
		}

	}
	void GrowThis(){
		transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(1f, 1f, 1f), 0.1f);
		if (transform.localScale.x >= 0.95f) {
			grow = false;
		}
	}
    void BeepCD()
    {
        cooldown = false;
    }
}
