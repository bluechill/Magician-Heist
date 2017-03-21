using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour {

	public Sprite flashLightOn;
	public Sprite flashLightOff;

	public GameObject fieldOfViewLight;

	private SpriteRenderer sprend;


	[SerializeField]
	private bool _isOn = true;
	public bool isOn {
		set {
			_isOn = value;

			SwapSprites ();
		}
		get {
			return _isOn;
		}
	}

	void Start() {
		sprend = GetComponent<SpriteRenderer> ();

		SwapSprites();
	}

	void SwapSprites() {
		if (isOn)
			sprend.sprite = flashLightOn;
		else
			sprend.sprite = flashLightOff;

		fieldOfViewLight.SetActive (isOn);
	}

	void Update() {
		if ((isOn && sprend.sprite != flashLightOn) ||
		    (!isOn && sprend.sprite != flashLightOff) ||
		    (isOn && !fieldOfViewLight.activeSelf) ||
		    (!isOn && fieldOfViewLight.activeSelf))
		{
			SwapSprites ();
		}
	}
}
