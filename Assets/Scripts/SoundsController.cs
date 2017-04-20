using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundsController : MonoBehaviour {
	public static SoundsController instance;
	public bool isPlaying = false;

	private Dictionary<AudioSource, double> sound_objects = new Dictionary<AudioSource, double>();
	private double nextEventTime = 0.0f;

	// Use this for initialization
	void Start () {
	}

	void OnSceneUnloaded(Scene aScene)
	{
		sound_objects.Clear ();
	}

	void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
	{
		nextEventTime = AudioSettings.dspTime;
	}

	public void ResetEventTime()
	{
		nextEventTime = AudioSettings.dspTime;
	}

	void Awake ()   
	{
		if (instance == null)
		{
			DontDestroyOnLoad(this.gameObject);
			instance = this;

			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		List<AudioSource> toDestroy = new List<AudioSource>();

		isPlaying = false;
		foreach (KeyValuePair<AudioSource, double> kv in sound_objects) {
			if (AudioSettings.dspTime > (kv.Value + kv.Key.clip.length) && !kv.Key.isPlaying) {
				toDestroy.Add (kv.Key);
			}

			isPlaying = isPlaying || kv.Key.isPlaying;
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
//		source.transform.parent = this.transform;
		source.GetComponent<AudioSource>().playOnAwake = false;
		source.GetComponent<AudioSource>().clip = clip;

		if (nextEventTime < AudioSettings.dspTime)
			nextEventTime = AudioSettings.dspTime;

		source.GetComponent<AudioSource>().PlayScheduled (nextEventTime);
		source.GetComponent<AudioSource> ().volume = 0.4f;
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
	public void PlaySound(AudioSource clip){
		clip.Play ();
	}
}
