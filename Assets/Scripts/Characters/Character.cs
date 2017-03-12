using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpriteRenderer))]
public class Character : MonoBehaviour {
    public bool blockedInput = false;

    //parent class for Character objects
    //	ex. Players, Guards, Police

    //required public components of a Character
    public Rigidbody rb;
    public SpriteRenderer sprend;

    //inspector settables separator
    public bool __________________;
    public float movement_velocity;
    // Use this for initialization
    public virtual void Start() {
        rb = GetComponent<Rigidbody>();
        sprend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnCollisionEnter(Collision coll) {
        print(coll.gameObject.tag);
        if (coll.gameObject.tag == "Guard") {
            StartCoroutine(KnockOut(10));
        }
        if (coll.gameObject.tag == "NightStick") {
            StartCoroutine(KnockOut(10));
        }
        if (coll.gameObject.tag == "Taser") {
            StartCoroutine(KnockOut(15));
        }
        if (coll.gameObject.tag == "UnconsciousMagician") {
            print("KO Mage");
            if (this.GetComponent<Magician>().action_button3) {
                print("Revive");
                coll.gameObject.GetComponent<Character>().blockedInput = false;
            }
        }
    }

    public IEnumerator KnockOut(int duration) { // When called have the item say how long the character is knocked out for
        blockedInput = true;
        yield return new WaitForSeconds(duration);
        blockedInput = false;
    }
}
