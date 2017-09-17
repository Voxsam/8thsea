using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {
	
	public static RoomController Obj;

	public Camera cam;
	public List<GameObject> playersInRoom;


	void Start ()
	{
		if (Obj == null) {
			Obj = this;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) {
			playersInRoom.Add (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {

		if (other.CompareTag ("Player")) {
			playersInRoom.Remove (other.gameObject);
		}
	}
}
