using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
	public Animator animator;
	public AnimationClip walkClip;
	public Rigidbody rb;

	public float life_time;
	public float age;
	public float birthtime;

	public bool[] actions;
	public int num_actions = 3;

	public KeyCode[][] key_mappings;

	public bool right,left;
	public bool is_hiding = false;
	public bool is_in_box = false;
	public bool is_in_closet = false;
	public bool using_up_escalator = false;
	public bool is_using_stairs = false;
	public bool is_holding = false;
	public GameObject held_object;
	public bool is_being_held = false;
	public GameObject being_held_by;
	public bool is_touching_box = false;
	public bool is_touching_up_stairs = false;
	public bool is_touching_down_stairs = false;
	public bool is_touching_door = false;
	public bool is_holding_briefcase = false;
	public GameObject touching_box;
	public GameObject touching_door;
	public GameObject inside_box;
	public GameObject touching_stairs;
	public GameObject win_menu;
	public GameObject lose_menu;

	public GameObject ability;
	public GameObject body;
	public CapsuleCollider capsule;
	public bool is_ability = false;
	public bool is_transformed = false;
	public bool is_knocked_out = false;



	public void Start(){
		birthtime = Time.time;
		rb = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();
		capsule = GetComponent<CapsuleCollider> ();
		InitKeys ();
		actions = new bool[num_actions];
		for (int i = 0; i < num_actions; i++) {
			actions[i] = false;
		}

	}

	public void Drop(){
		if (!is_holding)
			return;
		if (held_object.GetComponent<Item> ().briefcase) {
			is_holding_briefcase = false;
		}
		held_object = null;
		is_holding = false;

	}


	public void GetPickedUp(GameObject obj){
		is_being_held = true;
		being_held_by = obj;
	}
	public void GetDropped(){
		is_being_held = false;
		being_held_by.GetComponent<PlayerScript> ().Drop ();
		being_held_by = null;
	}
	public void UseUpEscalator(){
		using_up_escalator = true;
	}
	public void TouchBox(GameObject box){
		is_touching_box = true;
		touching_box = box;
	}
	public void StopTouchBox(GameObject box){
		is_touching_box = false;
		touching_box = null;
	}

	public void TransformDrop(){
		is_holding = false;
	}
	public bool IsGrounded(){
		RaycastHit hit, hit2;

		Vector3 extents = GetComponent<Collider> ().bounds.extents;

		Vector3 posLeft = transform.position - new Vector3(extents.x, 0f,0f);
		Vector3 posRight = transform.position + new Vector3(extents.x, 0f,0f);

		int layer = 1 << LayerMask.NameToLayer ("Default");

		Physics.Raycast(posLeft, -Vector3.up, out hit, 1.1f, layer);
		Physics.Raycast(posRight, -Vector3.up, out hit2, 1.1f, layer);

		if (hit.collider != null || hit2.collider != null) {
			//ray hit an environment object

			//float distance = Mathf.Abs(hit.point.y - transform.position.y);
			return true;
		}
		return false;
	}

	void InitKeys(){
		key_mappings = new KeyCode[3][];
		key_mappings [0] = new KeyCode[4];
		key_mappings [1] = new KeyCode[4];
		key_mappings [2] = new KeyCode[4];

		key_mappings [0][0] = KeyCode.W;
		key_mappings [0][1] = KeyCode.E;
		key_mappings [0][2] = KeyCode.R;
		key_mappings [0][3] = KeyCode.T;

		key_mappings [1][0] = KeyCode.S;
		key_mappings [1][1] = KeyCode.D;
		key_mappings [1][2] = KeyCode.F;
		key_mappings [1][3] = KeyCode.G;
	}

	public void EnterBox(){
		is_hiding = true;
		is_in_box = true;
		body.SetActive (false);
		inside_box = touching_box;
		inside_box.GetComponentInParent<BoxPlayerScript> ().players_in_box.Add (this.gameObject);
		StopTouchBox (touching_box);
		Hide ();
	}
	public void ExitBox(){
		is_hiding = false;
		is_in_box = false;
		body.SetActive (true);
		inside_box.GetComponentInParent<BoxPlayerScript> ().players_in_box.Remove (this.gameObject);
		inside_box = null;
		Reveal ();
	}
	public void UseUpStairs(){
		is_touching_up_stairs = false;
		is_using_stairs = true;

		animator.Play ("Using Stairs");
		Invoke ("DoneUpStairs", 1.5f);
	}
	public void UseDownStairs(){
		is_touching_down_stairs = false;
		is_using_stairs = true;

		animator.Play ("Using Stairs");
		Invoke ("DoneDownStairs", 1.5f);
	}
	public void DoneUpStairs(){
		transform.position = new Vector3 (transform.position.x, transform.position.y + 3.89f, 0f);
		is_using_stairs = false;
		animator.SetBool ("using_stairs", false);
	}
	public void DoneDownStairs(){
		transform.position = new Vector3 (transform.position.x, transform.position.y - 3.89f, 0f);
		is_using_stairs = false;
		animator.SetBool ("using_stairs", false);
	}
	public void OpenDoor(){
		touching_door.GetComponentInChildren<Door> ().OpenDoor ();
	}
	public void KnockOut(){
		is_knocked_out = true;
		animator.SetBool ("knockout", true);
		animator.Play ("Knockout");
		right = false;
		left = false;
		Hide ();
		Invoke ("WakeupAnimation", 4f);
		Invoke ("Wakeup", 5f);
	}
	public void WakeupAnimation(){
		animator.SetBool ("knockout", false);
	}
	public void Wakeup(){
		is_knocked_out = false;
		Reveal ();
	}
	public void Hide(){
		is_hiding = true;
	}	
	public void Reveal(){
		is_hiding = false;
	}
	public void Win(){
		win_menu.SetActive (true);
		Invoke ("Restart", 5f);
	}
	public void Lose(){
		lose_menu.SetActive (true);
		Invoke ("Restart", 5f);
	}
	public void Restart(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
}