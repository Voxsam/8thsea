using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {

	// The player this Camera is attached to
	public PlayerController player;
	public Vector3 initialOffset;

	void Update () {
		this.transform.position = player.transform.position + initialOffset;
	}


}
