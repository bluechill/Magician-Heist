using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertCamera : MonoBehaviour {
	private Camera camera;

	void Start() {
		camera = GetComponent<Camera> ();
	}

	void OnPreCull() {
		camera.ResetWorldToCameraMatrix ();
		camera.ResetProjectionMatrix ();
		camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(new Vector3 (1, 1, 1));
	}

	void OnPreRender () {
//		GL.SetRevertBackfacing (true);
	}

	void OnPostRender () {
//		GL.SetRevertBackfacing (false);
	}
}
