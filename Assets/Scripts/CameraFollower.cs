using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {
	public GameObject target;
	public Vector3 difference;

	// Eventually it would be nice for this script to be a little less naive (i.e., stay static within
	// a room, but follow when no longer in a room) but for now this just stays a constant distance
	// away from the target. 

	void Start () {
		difference = transform.position - target.transform.position;
	}

	void Update () {
		transform.position = target.transform.position + difference;
	}
}
