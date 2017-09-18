using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

    public CameraController cameraController;

	public Camera cam;
	public List<GameObject> playersInRoom;


	void Start ()
	{
        cameraController = GetComponentInChildren<CameraController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) {
            cameraController.AssignCameraToObject(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {

		if (other.CompareTag ("Player"))
        {
            cameraController.RemoveCameraFromObject(other.gameObject);
        }
	}
}
