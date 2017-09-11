using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	// public float turnSpeed;

	private Rigidbody rb;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}

	void Update () {

		transform.Translate (new Vector3 (Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * movementSpeed / 100f);

	}


}
