using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 10;

	private Vector3 destination;
	private bool moving;

	void Update ()
	{
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

		// Move the unit:
		if (moving) {
			this.transform.position = Vector3.MoveTowards(this.transform.position, destination, speed * Time.deltaTime);
			// Stop moving if we reach the destination:
			if (this.transform.position == destination)
				moving = false;
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
}
