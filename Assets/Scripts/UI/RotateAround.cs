using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

	public float rotateSpeed = 1f;
	public bool isRotating = true;
	private Quaternion initRotation;

	// Use this for initialization
	void Start () {
		initRotation = transform.rotation;
	}
	

	void Update () {
		if (isRotating) {
			transform.Rotate (0, rotateSpeed * -1f, 0);
		}
	}

	public void StartRotating () {
		isRotating = true;
	}

	public void StopRotating () {
		isRotating = false;
		transform.rotation = initRotation;
	}


}
