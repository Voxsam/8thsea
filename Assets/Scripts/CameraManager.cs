using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public GameObject lab;
	public GameObject sub;

	private RoomController labRoomCtrl;
	private RoomController subRoomCtrl;

	public Camera labCamera;
	public Camera subCamera;

	private CameraController labCamCtrl;
	private CameraController subCamCtrl;

	void Start () {
		labRoomCtrl = lab.GetComponent<RoomController> ();
		subRoomCtrl = sub.GetComponent<RoomController> ();
		labCamCtrl = labCamera.GetComponent<CameraController> ();
		subCamCtrl = subCamera.GetComponent<CameraController> ();
	}
	
	void Update () {

		if (subRoomCtrl.playersInRoom.Count <= 0) {
			subCamera.enabled = false;
		} else {
			subCamera.enabled = true;
		}
		if (labRoomCtrl.playersInRoom.Count <= 0) {
			labCamera.enabled = false;
		} else {
			labCamera.enabled = true;
		}

	}
}
