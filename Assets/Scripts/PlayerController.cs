﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 10;
	public float turnSpeed = 7;

    private GameController.ControlType controlMode;
    private PlayerInteractionController interactionController;
    public CameraController cameraController;

	[SerializeField] public Rigidbody rb;

	void Start () {
        //rb = this.GetComponent<Rigidbody> (); // Assigned in editor
        ControlMode = GameController.ControlType.CHARACTER;
        interactionController = GetComponentInChildren<PlayerInteractionController>();
    }

    public GameController.ControlType ControlMode
    {
        get { return controlMode; }
        private set { controlMode = value; }
    }

    #region Control handlers
    /// <summary>
    /// For other objects to request the player change its control to a certain object type.
    /// </summary>
    /// <param name="changeTo">The control type that the caller wishes the Player to change to</param>
    public void RequestControlChange(GameController.ControlType changeTo)
    {
        // if there are no problems (aka insert checks here), change to that control type
        ControlMode = changeTo;
    }

    public void ReturnControlToCharacter()
    {
        ControlMode = GameController.ControlType.CHARACTER;
    }

    public void AssignCameraToPlayer(CameraController cc)
    {
        cameraController = cc;
        ReattachCameraToPlayer();
    }

    public void ReattachCameraToPlayer()
    {
        cameraController.SetCameraToObject(this.gameObject);
    }
    #endregion

    public void GameUpdate () {

		
		if (ControlMode == GameController.ControlType.CHARACTER) {
			Vector3 direction = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
            
            if (direction != Vector3.zero) {
				transform.rotation = Quaternion.Slerp (
					transform.rotation,
					Quaternion.LookRotation (direction),
					Time.deltaTime * turnSpeed
				);

				transform.Translate (new Vector3 (0, 0, movementSpeed / 100f));
			}
            
		}
	}

}
