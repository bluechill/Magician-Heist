using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public Text Clock;
    public bool gameOver = false;
    int hours; //Display Times
    int minutes;
    int passedMinutes; //Used for Calculating Endgame Score
    double seconds;
    GameObject Magician;

    // Use this for initialization
    void Start() {
        //Game Starts at 10PM
        seconds = 0;
        hours = 22;
        minutes = 00;
        passedMinutes = 0;
        Magician = GameObject.Find("Magician");
    }

    // Update is called once per frame
    void Update() {
        if (Magician.GetComponent<Transform>().position.y < 4.8)
            gameOver = true;
        if (!gameOver) {
            seconds += 1;
            if (seconds == 60) {
                minutes++;
                passedMinutes++;
                seconds = 0;
            }
            if (minutes == 60) {
                hours += 1;
                if (hours == 24) {
                    gameOver = true;
                    hours = 00;
                }
                minutes = 00;
            }
            if (hours < 10 && minutes < 10)
                Clock.text = "0" + hours.ToString() + ":0" + minutes.ToString();
            else if (hours < 10)
                Clock.text = "0" + hours.ToString() + ":" + minutes.ToString();
            else if (minutes < 10)
                Clock.text = hours.ToString() + ":0" + minutes.ToString();
            else
                Clock.text = hours.ToString() + ":" + minutes.ToString();
        }
    }

    //void FinishTime() {
    //    // 6AM is when the game ends (Hard Cutoff)
    //    int FinalTimeScore = 10000 - 20 * passedMinutes;
    //    EndTime.text = "10,000 - 20 x " + passedMinutes.ToString() + " = " + FinalTimeScore.ToString();
    //    EndTime.gameObject.SetActive(true);
    //}
}