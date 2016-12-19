using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{	
	public Transform target;
	public float damping = 1, lookAheadFactor = 3, lookAheadReturnSpeed = 0.5f, lookAheadMoveThreshold = 0.1f, yPosRestriction = -1;
	
	float offsetZ, nextTimeToSearch = 0;
	Vector3 lastTargetPosition, currentVelocity, lookAheadPos;
	
	// Use this for initialization
	void Start ()
    {
		lastTargetPosition = target.position;
		offsetZ = (transform.position - target.position).z;
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (target == null)
        {
			FindPlayer ();
			return;
		}

		// only update lookahead pos if accelerating or changed direction
		float xMoveDelta = (target.position - lastTargetPosition).x;

	    bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

		if (updateLookAheadTarget)
            lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
		else
            lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);	
		
		Vector3 aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ, newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

		newPos = new Vector3 (newPos.x, Mathf.Clamp (newPos.y, yPosRestriction, Mathf.Infinity), newPos.z);

		transform.position = newPos;
		
		lastTargetPosition = target.position;		
	}

	void FindPlayer ()
    {
		if (nextTimeToSearch <= Time.time) {
			GameObject searchResult = GameObject.FindGameObjectWithTag ("Player");
			if (searchResult != null)
				target = searchResult.transform;
			nextTimeToSearch = Time.time + 0.5f;
		}
	}
}
