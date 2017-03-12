using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour {
    GameObject Magician;
    GameObject Timer;
	// Use this for initialization
	void Start () {
        Magician = GameObject.Find("Magician");
        Timer = GameObject.Find("Timer");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene("Demo Level");
        if (Magician.GetComponent<Magician>().Game_Won)
            SceneManager.LoadScene("Win Scene");
        if (Timer.GetComponent<Timer>().gameOver)
            SceneManager.LoadScene("Game Over Scene");
    }
}
