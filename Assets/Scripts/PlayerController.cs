using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	public float turnSpeed;

	private Rigidbody rb;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}

	void Update () {

		transform.Rotate (new Vector3 (0, Input.GetAxis("Horizontal"), 0) * turnSpeed);
		transform.Translate (new Vector3 (Input.GetAxis("Vertical"),0, 0) * movementSpeed / 100f);

	}


}
