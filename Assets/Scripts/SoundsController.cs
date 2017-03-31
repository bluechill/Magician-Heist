using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsController : MonoBehaviour {
	public static SoundsController instance;

	private Dictionary<AudioSource, double> sound_objects = new Dictionary<AudioSource, double>();
	private double nextEventTime = 0.0f;

	// Use this for initialization
	void Start () {
		nextEventTime = AudioSettings.dspTime;
	}

	void Awake ()   
	{
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		List<AudioSource> toDestroy = new List<AudioSource>();

		foreach (KeyValuePair<AudioSource, double> kv in sound_objects) {
			if (AudioSettings.dspTime > (kv.Value + kv.Key.clip.length) && !kv.Key.isPlaying) {
				toDestroy.Add (kv.Key);
			}
		}

		foreach (AudioSource source in toDestroy) {
			Destroy(source.gameObject);
			sound_objects.Remove (source);
		}
	}

	public bool IsPlaying(){
		return	sound_objects.Count > 0;
	}

	public void DisableLooping() {
		foreach (KeyValuePair<AudioSource, double> kv in sound_objects) {
			kv.Key.loop = false;
			break;
		}
	}

	public void EnableLooping() {
		foreach (KeyValuePair<AudioSource, double> kv in sound_objects) {
			if (kv.Key.isPlaying || AudioSettings.dspTime < (kv.Value + kv.Key.clip.length))
				kv.Key.loop = true;

			break;
		}
	}

	public void QueueClip(AudioClip clip) {
		GameObject source = new GameObject ();
		source.name = "Audio Source: " + clip.name;
		source.AddComponent<AudioSource> ();
		source.transform.parent = this.transform;
		source.GetComponent<AudioSource>().playOnAwake = false;
		source.GetComponent<AudioSource>().clip = clip;

		if (nextEventTime < AudioSettings.dspTime)
			nextEventTime = AudioSettings.dspTime;

		source.GetComponent<AudioSource>().PlayScheduled (nextEventTime);
		sound_objects [source.GetComponent<AudioSource>()] = nextEventTime;
		nextEventTime = nextEventTime + clip.length;
	}

	public void StopPlaying() {
		foreach (KeyValuePair<AudioSource, double> kv in sound_objects) {
			kv.Key.Stop ();
			Destroy (kv.Key.gameObject);
		}

		sound_objects.Clear ();
	}
}
