using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaRender : MonoBehaviour {

	public float alpha = 1.0f;
	public Material mat;

	// Update is called once per frame
	void OnPostRender()
	{
		GL.PushMatrix();
		GL.LoadOrtho();
		mat.SetFloat( "_Alpha", alpha );
		mat.SetPass(0);
		GL.Begin( GL.QUADS );
		GL.Vertex3( 0f, 0f, 0.1f );
		GL.Vertex3( 1f, 0f, 0.1f );
		GL.Vertex3( 1f, 1f, 0.1f );
		GL.Vertex3( 0f, 1f, 0.1f );
		GL.End();
		GL.PopMatrix();
	}
}
