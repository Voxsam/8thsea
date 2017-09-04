using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tubeController : MonoBehaviour {

	public float rotationSpeed;
	public float forwardSpeed;
	public float attractionForce;
	public bool easyMode;
	public bool isActivated;
	public float radius;
	public GameObject anchorPoint;
	//public GameObject playerCharacter;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isActivated) {
			Debug.Log ("ANCHOR PIONT: "+anchorPoint.transform.position);
			if (!easyMode) {
				float x = Input.GetAxis ("Horizontal") * Time.deltaTime * rotationSpeed;
				float y = Input.GetAxis ("Vertical") * Time.deltaTime * forwardSpeed;
				transform.Rotate (0, 0, -x);
				Vector3 newLocation = transform.position+transform.TransformDirection(y,0,0); 
				Debug.Log ("current pos = " + transform.position + " new pos = " + newLocation);
				float distance = Vector3.Distance(newLocation, anchorPoint.transform.position);
				Debug.Log(distance);
				Debug.Log (distance > radius);
				if (distance > radius) {
					Vector3 fromAnchorToObject = newLocation - anchorPoint.transform.position;
					fromAnchorToObject *= radius / distance;
					newLocation = anchorPoint.transform.position + fromAnchorToObject;
					transform.position = newLocation;
				} else {
					transform.Translate (y, 0, 0);
				}
			
			} else {
				float x = Input.GetAxis ("Horizontal") * Time.deltaTime * forwardSpeed;
				float y = Input.GetAxis ("Vertical") * Time.deltaTime * forwardSpeed;
				Vector3 newLocation = transform.position+transform.TransformDirection(x,y,0); 
				float distance = Vector3.Distance(newLocation, anchorPoint.transform.position);
				if (distance > radius) {
					Vector3 fromAnchorToObject = newLocation - anchorPoint.transform.position;
					fromAnchorToObject *= radius / distance;
					newLocation = anchorPoint.transform.position + fromAnchorToObject;
					transform.position = newLocation;

				} else {
					transform.Translate (x, y, 0,Space.World);
					if (x != 0) {
						Vector3 lookatPos = new Vector3 (0, 0, x);
						transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (lookatPos), 0.5f);
					}
				}
				/*
				float x = Input.GetAxisRaw ("Horizontal")* forwardSpeed * Time.deltaTime;
				float y = Input.GetAxisRaw ("Vertical")* forwardSpeed * Time.deltaTime;
				Debug.Log ("Y = " + y+" x = "+x);
				Vector3 newLocation = transform.position+transform.TransformDirection(x,y,0);
				Debug.Log (newLocation);
				if (x != 0) {
					Vector3 lookatPos = new Vector3 (0, 0, x);
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (lookatPos), 0.5f);
				}
					
					float distance = Vector3.Distance(newLocation, anchorPoint.transform.position);
				Debug.Log(distance);
				Debug.Log (distance > radius);
				if (distance > radius) {
					//Vector3 fromOriginToObject = newLocation - anchorPoint.transform.position;
					//fromOriginToObject *= radius / distance;
					//newLocation = anchorPoint.transform.position + fromOriginToObject;
					//transform.position = newLocation;
				} else {
					//ransform.Translate (newLocation, Space.World);
				}*/
			}
		}
	}

	void OnCollisionEnter(Collision other) {
		if (isActivated) {
			Debug.Log (other.gameObject.tag);
			if (other.gameObject.tag.Equals ("Fish"))
				Destroy (other.gameObject);
		}
	}

	void OnTriggerStay (Collider other) {
		if (isActivated) {
			Debug.Log (other.gameObject.tag);
			if (other.tag.Equals ("Fish") && other.attachedRigidbody) {
				Vector3 dir = transform.position - other.transform.position;
				dir = dir.normalized;
				other.attachedRigidbody.AddForce ((dir) * attractionForce);
			}
		}
	}
}
