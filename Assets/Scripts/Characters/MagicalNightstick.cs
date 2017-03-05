using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalNightstick : Ability {
    bool active = false;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    //reset position so it falls to the ground, just juice 
    public override void UseAbility() {
        Magician magician = GetComponentInParent<Magician>();
        if (active) {
            magician.using_ability = false;
            this.gameObject.SetActive(false);
        }
        else {
            magician.using_ability = true;
        }
        active = !active;
    }
//action function called when a player uses the magical nightstick
    public void Action(GameObject mag) {
        Magician magician = mag.GetComponent<Magician>();
    }   

}
