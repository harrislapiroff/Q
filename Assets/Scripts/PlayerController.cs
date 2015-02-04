using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 10;
	public GameObject spawn;

	private Vector3 destination;
	private bool moving;

	void Update ()
	{
		// Handle inputs:
		handleRestartInput ();
		handleMovementInput ();

		// Move the unit according to `destination` and `moving`:
		if (moving) {
			this.transform.position = Vector3.MoveTowards(this.transform.position, destination, speed * Time.deltaTime);
			// Stop moving if we reach the destination:
			if (this.transform.position == destination)
				moving = false;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		// If player hits a checkpoint, relocate the spawn to it:
		if (other.tag == "checkpoint")
			spawn.transform.position = other.gameObject.transform.position;
	}

	void handleRestartInput () {
		if (Input.GetButton ("Restart"))
			restart ();
	}

	void handleMovementInput () {
		Vector3 moveVector = Vector3.zero;

		// Set the move vector
		if (Input.GetButton ("Left")) {
			moveVector = Vector3.left;
		} else if (Input.GetButton ("Right")) {
			moveVector = Vector3.right;
		} else if (Input.GetButton ("Up")) {
			moveVector = Vector3.forward;
		} else if (Input.GetButton ("Down")) {
			moveVector = Vector3.back;
		}
		
		if (moveVector != Vector3.zero && !moving) {
			move(moveVector);
		}
	}

	void move (Vector3 direction)
	{
		// Make sure there's a wall somewhere and save the collision to hit.
		RaycastHit hit;
		Physics.Raycast (transform.position, direction, out hit, Mathf.Infinity);
		
		// If the ray hits something that isn't a wall, do it again from the new position:
		while (hit.collider.tag != "wall") {
			Physics.Raycast (hit.collider.transform.position, direction, out hit, Mathf.Infinity);
		}

		// Once we find a wall, make it (minus half of the player's width) our destination:
		// (Currently assuming player size is 1.)
		destination = hit.point - (direction.normalized * .5f);
		moving = true;
	}

	void restart ()
	{
		// Restart by immediately repositioning to the spawn location:
		transform.position = spawn.transform.position;
		moving = false;
		destination = spawn.transform.position;
	}
}
