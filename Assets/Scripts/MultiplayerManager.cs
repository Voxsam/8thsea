using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour {

	public static MultiplayerManager Obj;

	// The list of players, which SHOULD be determined in a previous screen before the game starts and
	// SHOULDNT change at any point during the game.
	public PlayerController[] playerList;

	// The PlayerCamera prefab that should have a PlayerCameraController component attached to it.
	public GameObject playerCameraPrefab;


	void Awake () {
		
		if (Obj == null)
		{
			Obj = this;
		}
		else
		{
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this.gameObject);

		print (Input.GetJoystickNames ().Length + " joysticks connected: ");
		foreach (string joystick in Input.GetJoystickNames()) {
			print (joystick + " is connected");
		}



	}


	public void Setup () {

		for (int i = 0; i < playerList.Length; i++) {

			PlayerController player = playerList [i];
			player.playerNumber = i + 1;
			player.controls = new ControlScheme (player.joystickNumber);	

		}

		// SetCameras ();

	}

	private void SetCameras() {

		switch (playerList.Length) {

		case 1:
			playerList [0].cameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (1.0f, 1.0f));
			break;

		case 2:
			playerList [0].cameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (0.5f, 1.0f));
			playerList [1].cameraController.GetCamera.rect = new Rect (new Vector2 (0.5f, 0f), new Vector2 (0.5f, 1.0f));
			break;

		case 3: 
			playerList [0].cameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (0.333f, 1.0f));
			playerList [1].cameraController.GetCamera.rect = new Rect (new Vector2 (0.333f, 0f), new Vector2 (0.333f, 1.0f));
			playerList [2].cameraController.GetCamera.rect = new Rect (new Vector2 (0.333f, 0f), new Vector2 (0.333f, 1.0f));
			break;

		case 4:
			playerList [0].cameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (0.5f, 0.5f));
			playerList [1].cameraController.GetCamera.rect = new Rect (new Vector2 (0.5f, 0f), new Vector2 (0.5f, 0.5f));
			playerList [2].cameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0.5f), new Vector2 (0.5f, 0.5f));
			playerList [3].cameraController.GetCamera.rect = new Rect (new Vector2 (0.5f, 0.5f), new Vector2 (0.5f, 0.5f));
			break;

		default:
			playerList [0].cameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (1.0f, 1.0f));
			break;

		}
			

	}

	void Update ()
	{
		/*
		print ("J1: " + Input.GetAxis ("Horizontal_J1"));
		print ("J2: " + Input.GetAxis ("Horizontal_J2"));
		print ("J3: " + Input.GetAxis ("Horizontal_J3"));
		print ("J4: " + Input.GetAxis ("Horizontal_J4"));
		*/
		print ("J1: " + Input.GetAxis ("Horizontal_J1"));
		print ("J2: " + Input.GetAxis ("Horizontal_J2"));
	}



}
