using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_LineOfSight : LayerMonoBehavior {
	public LayerMask CullingLayers;
	public Mesh mesh;

	void Start() {
		mesh = new Mesh ();

		var go = GameObject.FindGameObjectWithTag ("DebugVisibleArea");
		go.GetComponent<MeshFilter> ().mesh = mesh;
	}

	private List<Vector3> hits = new List<Vector3>();
	// Update is called once per frame
	void Update () {
		if (CullingLayers.value == 0)
			return;

		Vector2 pos2D = new Vector2 (this.transform.position.x, this.transform.position.y);

		hits.Clear ();

		var lineSegmentEnds = new List<Vector2> ();

		foreach (GameObject go in LayerMonoBehavior.FindGameObjectsWithLayer(CullingLayers.value >> 1)) {
			lineSegmentEnds.Clear ();

			var coll = go.GetComponent<Collider2D> ();
			if (coll is BoxCollider2D) {
				lineSegmentEnds.Add (new Vector2 (coll.bounds.center.x - coll.bounds.extents.x, coll.bounds.center.y - coll.bounds.extents.y));
				lineSegmentEnds.Add (new Vector2 (coll.bounds.center.x + coll.bounds.extents.x, coll.bounds.center.y - coll.bounds.extents.y));
				lineSegmentEnds.Add (new Vector2 (coll.bounds.center.x - coll.bounds.extents.x, coll.bounds.center.y + coll.bounds.extents.y));
				lineSegmentEnds.Add (new Vector2 (coll.bounds.center.x + coll.bounds.extents.x, coll.bounds.center.y + coll.bounds.extents.y));
			} else if (coll is CircleCollider2D) {
				var circle = coll as CircleCollider2D;
				float radius = circle.radius;
				Vector3 center = coll.bounds.center;

				int precision = 20;

				for (int i = 1; i <= precision; ++i) {
					float angle = ((float)i) / ((float)precision) * (2f * Mathf.PI);
					lineSegmentEnds.Add (new Vector2 (Mathf.Cos (angle) * radius + center.x, Mathf.Sin (angle) * radius + center.y));
				}
			}

			foreach (Vector2 vec in lineSegmentEnds) {
				Vector3 direction = (vec - pos2D).normalized;

				float angle = Mathf.Atan2 (direction.y, direction.x);
				float amount = 0.0001f;

				angle -= amount;
				Vector3 direction2 = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0.0f);

				angle += amount * 2f;
				Vector3 direction3 = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0.0f);

				RaycastHit2D hit = Physics2D.Raycast (pos2D, direction, 20.0f, CullingLayers.value);
				if (hit.collider != null) {
					hits.Add (new Vector3 (hit.point.x, hit.point.y));
				}

				hit = Physics2D.Raycast (pos2D, direction2, 20.0f, CullingLayers.value);
				if (hit.collider != null) {
					hits.Add (new Vector3 (hit.point.x, hit.point.y));
				}

				hit = Physics2D.Raycast (pos2D, direction3, 20.0f, CullingLayers.value);
				if (hit.collider != null) {
					hits.Add (new Vector3 (hit.point.x, hit.point.y));
				}
			}
		}

		hits.Sort (delegate(Vector3 x, Vector3 y) {
			var nX = (x - this.transform.position).normalized;
			var nY = (y - this.transform.position).normalized;

			float xAngle = Mathf.Atan2(nX.y, nX.x);
			float yAngle = Mathf.Atan2(nY.y, nY.x);

			if (xAngle < yAngle)
				return -1;
			else if (xAngle > yAngle)
				return 1;
			else
				return 0;
		});

		UpdateMesh ();
	}

	private void UpdateMesh()
	{
		var vertex = new Vector3[hits.Count+1];

		for (int x = 0;x < hits.Count;++x)
			vertex[x+1] = hits[x];

		vertex [0] = this.transform.position;

		var uvs = new Vector2[vertex.Length];
		for(int x = 0; x < vertex.Length; ++x)
		{
			if((x%2) == 0)
			{
				uvs[x] = new Vector2(0,0);
			}
			else
			{
				uvs[x] = new Vector2(1,1);
			}
		}

		//Triangles
		var tris = new int[3 * (vertex.Length - 1)];    //3 verts per triangle * num triangles
		int C1 = 1;
		int C2 = 2;

		for (int x = 0; x < (tris.Length-3); x += 3) {
			tris [x] = C2;
			tris [x + 1] = C1;
			tris [x + 2] = 0;

			C1++;
			C2++;
		}

		tris [tris.Length - 3] = 1;
		tris [tris.Length - 2] = vertex.Length-1;
		tris [tris.Length - 1] = 0;

		mesh.vertices = vertex;
		mesh.uv = uvs;
		mesh.triangles = tris;

		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();

		mesh.name = "Debug Visible Area";
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.magenta;

		foreach (Vector3 hit in hits)
			Gizmos.DrawLine (this.transform.position, hit);
	}
}
