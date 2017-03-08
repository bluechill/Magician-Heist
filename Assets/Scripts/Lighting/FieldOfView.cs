using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour {
	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

	public float meshResolution;
	public int edgeResolveIterations;
	public float edgeDistanceThreshold;

	public MeshFilter viewMeshFilter;
	Mesh viewMesh;

	void Start() {
		viewMesh = new Mesh ();
		viewMesh.name = "View Visualization Mesh";
		viewMeshFilter.mesh = viewMesh;

		StartCoroutine ("FindTargetsWithDelay", 0.1); 
	}

	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}

	void FindVisibleTargets() {
		visibleTargets.Clear ();

		Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; ++i) {
			Transform target = targetsInViewRadius [i].transform;

			Vector3 directionToTarget = (target.position - transform.position).normalized;

			if (DirectionToAngle(directionToTarget, false) < viewAngle / 2f) {
				float distanceToTarget = Vector3.Distance (transform.position, target.position);

				if (!Physics.Raycast (transform.position, directionToTarget, distanceToTarget, obstacleMask)) {
					visibleTargets.Add (target);
				}
			}
		}
	}

	void LateUpdate() {
		DrawFieldOfView();
	}

	void DrawFieldOfView() {
		int rayCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float rayAngleSize = viewAngle / rayCount;
		List<Vector3> viewPoints = new List<Vector3> ();
		ViewCastInfo oldViewCast = new ViewCastInfo ();

		for (int i = 0; i <= rayCount; ++i) {
			float angle = 0f;

			if (this.transform.rotation.eulerAngles.y == 180f)
				angle = transform.eulerAngles.z - viewAngle / 2f + rayAngleSize * i;
			else
				angle = -transform.eulerAngles.z - viewAngle / 2f + rayAngleSize * i;

			ViewCastInfo newViewCast = ViewCast (angle);

			if (i > 0) {
				bool edgeDistanceThresholdExceeded = Mathf.Abs (oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;

				if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded)) {
					EdgeInfo edge = FindEdge (oldViewCast, newViewCast);

					if (edge.pointA != Vector3.zero)
						viewPoints.Add (edge.pointA);

					if (edge.pointB != Vector3.zero)
						viewPoints.Add (edge.pointB);
				}
			}

			viewPoints.Add (ViewCastToOtherSideOfObject(newViewCast).point);
			oldViewCast = newViewCast;
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices [0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; ++i) {
			vertices [i + 1] = transform.InverseTransformPoint(viewPoints [i]);

			if (i < vertexCount - 2) {
				triangles [i * 3] = 0;
				triangles [i * 3 + 1] = i + 1;
				triangles [i * 3 + 2] = i + 2;
			}
		}

		viewMesh.Clear ();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals ();
	}

	EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
		float minAngle = minViewCast.angle;
		float maxAngle = maxViewCast.angle;
		ViewCastInfo minPoint = new ViewCastInfo ();
		ViewCastInfo maxPoint = new ViewCastInfo ();

		for (int i = 0; i < edgeResolveIterations; ++i) {
			float angle = (minAngle + maxAngle) / 2f;
			ViewCastInfo newViewCast = ViewCast (angle);

			bool edgeDistanceThresholdExceeded = Mathf.Abs (minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;

			if (newViewCast.hit == minViewCast.hit && !edgeDistanceThresholdExceeded) {
				minAngle = angle;
				minPoint = newViewCast;
			} else {
				maxAngle = angle;
				maxPoint = newViewCast;
			}
		}

		return new EdgeInfo (ViewCastToOtherSideOfObject(minPoint).point, ViewCastToOtherSideOfObject(maxPoint).point);
	}

	ViewCastInfo ViewCast(float globalAngle) {
		Vector3 direction = DirectionFromAngle (globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast (transform.position, direction, out hit, viewRadius, obstacleMask)) {
			return new ViewCastInfo (true, hit.point, hit.distance, globalAngle, hit.normal, hit.collider);
		}

		return new ViewCastInfo (false, transform.position + direction * viewRadius, viewRadius, globalAngle, Vector3.up, null);
	}

	ViewCastInfo ViewCastToOtherSideOfObject(ViewCastInfo oldViewCast) {
		if (!oldViewCast.hit)
			return oldViewCast;

		RaycastHit hit;

		Vector3 direction = DirectionFromAngle (oldViewCast.angle, true);
		Vector3 newOrigin = oldViewCast.point + direction * oldViewCast.collider.bounds.size.sqrMagnitude;

		Ray ray = new Ray (newOrigin, -direction);
		if (oldViewCast.collider.Raycast(ray, out hit, oldViewCast.collider.bounds.size.sqrMagnitude * 2f)) {
			float distance = (hit.point - transform.position).magnitude;
			Vector3 hitPoint = hit.point;
			float adjustedDistance = distance - viewRadius;

			if (adjustedDistance < 0)
				adjustedDistance = 0;
			else {
				distance -= adjustedDistance;
				hitPoint = hitPoint - direction * adjustedDistance;
			}

			return new ViewCastInfo (true, hitPoint, distance, oldViewCast.angle, hit.normal, hit.collider);
		}

		return oldViewCast;
	}

	public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal)
			angleInDegrees -= transform.eulerAngles.z;
		
		return new Vector3 (Mathf.Sin (angleInDegrees * Mathf.Deg2Rad), Mathf.Cos (angleInDegrees * Mathf.Deg2Rad), 0);
	}

	public float DirectionToAngle(Vector3 direction, bool angleIsGlobal) {
		float angleInDegrees = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;

		if (!angleIsGlobal)
			angleInDegrees -= transform.eulerAngles.z;

		return angleInDegrees;
	}

	public struct ViewCastInfo {
		public bool hit;
		public Vector3 point;
		public float distance;
		public float angle;
		public Vector3 normal;
		public Collider collider;

		public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle, Vector3 _normal, Collider _collider) {
			hit = _hit;
			point = _point;
			distance = _distance;
			angle = _angle;
			normal = _normal;
			collider = _collider;
		}
	}

	public struct EdgeInfo {
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
			pointA = _pointA;
			pointB = _pointB;
		}
	}
}
