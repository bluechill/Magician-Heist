using System;
using UnityEngine;

public class Camera2DFollowMultiple : MonoBehaviour
{
    public Transform[] targets;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;

	private Camera cam;

	private Vector3 averageTargetPosition()
	{
		Vector3 average = Vector3.zero;

		foreach (Transform target in targets)
			average += target.position;

		average /= targets.Length;

		return average;
	}

    // Use this for initialization
    private void Start()
    {
		cam = GetComponent<Camera> ();

		m_LastTargetPosition = averageTargetPosition();
		m_OffsetZ = (transform.position - averageTargetPosition()).z;
        transform.parent = null;
    }

	private float orthoSize()
	{
		float xMin = 99999f;
		float xMax = -99999f;
		float yMin = 99999f;
		float yMax = -99999f;
		
		foreach (Transform target in targets) {
			var pos = target.position;
			var extents = target.GetComponent<Collider> ().bounds.extents;

			Vector3 left = pos - new Vector3 (extents.x, 0f);
			Vector3 right = pos + new Vector3 (extents.x, 0f);

			Vector3 bottom = pos - new Vector3 (0f, extents.y);
			Vector3 top = pos + new Vector3 (0f, extents.y);

			xMin = Mathf.Min (xMin, left.x);
			xMax = Mathf.Max (xMax, right.x);
			yMin = Mathf.Min (yMin, bottom.y);
			yMax = Mathf.Max (yMax, top.y);
		}

		float horz = (xMax - xMin) * Screen.height / Screen.width + 10f;
		float vert = yMax - yMin + 2f;

		return Mathf.Max(Mathf.Max (horz, vert) / 2f, 3f);
	}


    // Update is called once per frame
    private void Update()
    {
		cam.orthographicSize = orthoSize ();

        // only update lookahead pos if accelerating or changed direction
		float xMoveDelta = (averageTargetPosition() - m_LastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
        }
        else
        {
            m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
        }

		Vector3 aheadTargetPos = averageTargetPosition() + m_LookAheadPos + Vector3.forward*m_OffsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

        transform.position = newPos;

		m_LastTargetPosition = averageTargetPosition();
    }
}