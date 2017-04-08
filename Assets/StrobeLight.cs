using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrobeLight : MonoBehaviour {
	Light light;
	public bool dim = true;
	public bool brighten = false;
	public bool dimmed = true;
	public bool brightened = false;
	float intensity = 1.25f;
	public bool loose;
	// Use this for initialization
	void Start () {
		light = GetComponent<Light> ();
		InvokeRepeating ("Transition", 1.2f, 1.2f);
	}
	
	// Update is called once per frame
	void Update () {
		if (loose)
			intensity = 2.5f;
		if (dim) {
			light.intensity = Mathf.Lerp (light.intensity, 0f, 0.1f);
			if (light.intensity <= 0.1f) {
				dim = false;
				brighten = true;
			}
		} 
		if (brighten) {
			light.intensity = Mathf.Lerp (light.intensity, intensity, 0.1f);
			if (light.intensity >= intensity - 0.1f) {
				brighten = false;
				dim = true;
			}
		}
	}
	void Transition(){
	}
}
