using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public enum CameraType
	{
		LAB, SUB
	};

	public CameraType cameraType;

	public GameObject room;
	private RoomController roomCtrl;

	private float yOffset;
	public float zOffset;

	public float minFOV; // minimum FOV of camera
	public float xDifference;

	public Camera cam;


	private Vector3 initCamPos;
	private Vector3 offset;

	private Vector3 maxBounds;
	private Vector3 minBounds;
	private Vector3 cameraCenter;


	void Start () {
		cam = GetComponent<Camera> ();
		initCamPos = transform.position;
		offset = room.transform.position - initCamPos;
		roomCtrl = room.GetComponent<RoomController> ();
	}

	// Update is called once per frame
	public void Update () {

		//GetPlayerBounds ();


		gameObject.transform.position = roomCtrl.playersInRoom[0].transform.position - offset;

		/*
		if (maxBounds.x - minBounds.x > xDifference) {
			cam.fieldOfView = (maxBounds.x - minBounds.x) / xDifference * minFOV;
		} else {
			cam.fieldOfView = minFOV;
		}
		*/

	}

	void GetPlayerBounds () {

		Vector3 curr = roomCtrl.playersInRoom[0].transform.position;
		maxBounds = minBounds = curr;

		for (int i = 1; i < roomCtrl.playersInRoom.Count; i++) {
			curr = roomCtrl.playersInRoom [i].transform.position;
			maxBounds = new Vector3 (Mathf.Max (maxBounds.x, curr.x), Mathf.Max (maxBounds.y, curr.y), Mathf.Max (maxBounds.z, curr.z));
			minBounds = new Vector3 (Mathf.Min (minBounds.x, curr.x), Mathf.Min (minBounds.y, curr.y), Mathf.Min (minBounds.z, curr.z));
		}
			
		cameraCenter = new Vector3 (minBounds.x + (maxBounds.x - minBounds.x) / 2, minBounds.y + (maxBounds.y - minBounds.y) / 2 + yOffset, minBounds.z + (maxBounds.z - minBounds.z) / 2);//+ zOffset);

	}
		
}
