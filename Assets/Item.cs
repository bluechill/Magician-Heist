using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	public bool briefcase = false;
	public bool flash_light = false;
	public bool magical_key = false;
	public bool is_player = false;
	public bool gold_bar = false;
	public bool tree = false;
	public bool key_card = false;
	public bool held = false;
    public bool thrown = false;
	public int player_num = -1;
	public bool enabled = false;
	public GameObject current_player;
	public bool highlight = false;
	private MaterialPropertyBlock propertyBlock;
	private SpriteRenderer srend;
	// Use this for initialization
	void Start () {
		propertyBlock = new MaterialPropertyBlock ();
		srend = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if ((tree && held) || flash_light) {
			transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 90f));
		} else {
			transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, 0f));
		}
		if (enabled) {
			GetComponent<Rigidbody> ().isKinematic = false;
			GetComponent<SpriteRenderer> ().sortingOrder = 30;
		} else {
			GetComponent<Rigidbody> ().isKinematic = true;
		}
        if(GetComponent<Rigidbody>().velocity.magnitude <= 0.1f)
        {

        
        }

		srend.GetPropertyBlock(propertyBlock);
		propertyBlock.SetFloat("_Outline", highlight ? 1f : 0);
		propertyBlock.SetColor("_OutlineColor", Color.red);
		propertyBlock.SetFloat("_OutlineSize", 24);
		srend.SetPropertyBlock(propertyBlock);
	}

	public void SetPlayer(GameObject obj, int n){
		current_player = obj;
		is_player = true;
		player_num = n;
	}
	public void ResetPlayer(){
		current_player = null;
		is_player = false;
		player_num = -1;
	}
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Guard" && !other.gameObject.GetComponent<StatePatternEnemy>().knockout && thrown)
        {
            other.gameObject.GetComponent<StatePatternEnemy>().Knockout();
            thrown = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;

        }
    }

	public void SetHighlight(bool shouldHighlight)
	{
		highlight = shouldHighlight;	
	}
}
