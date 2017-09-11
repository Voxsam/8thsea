using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	public float turnSpeed;

	public bool canMove;

	private Rigidbody rb;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}

	void Update () {

		if (canMove) {
			Vector3 direction = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
			transform.Translate (direction * movementSpeed / 100f);
			if (direction != Vector3.zero) {
				transform.rotation = Quaternion.Slerp (
					transform.rotation,
					Quaternion.LookRotation (direction),
					Time.deltaTime * turnSpeed
				);
			}
		}
	}

}
