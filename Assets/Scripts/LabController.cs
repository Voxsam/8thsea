using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabController : MonoBehaviour {

	public static LabController Obj;

	public Camera labCamera;
	public List<GameObject> playersInLab;


	void Start ()
	{
		if (Obj == null) {
			Obj = this;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) {
			playersInLab.Add (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {

		if (other.CompareTag ("Player")) {
			playersInLab.Remove (other.gameObject);
		}
	}

}
