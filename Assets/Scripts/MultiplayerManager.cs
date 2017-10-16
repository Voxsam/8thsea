using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour {

	public static MultiplayerManager Obj;

	// The list of players, which SHOULD be determined in a previous screen before the game starts and
	// SHOULDNT change at any point during the game.
	public PlayerController[] playerList;


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

		Setup ();

	}


	private void Setup () {

		for (int i = 0; i < playerList.Length; i++) {

			PlayerController player = playerList [i];
			player.playerNumber = i + 1;
			player.controls = new ControlScheme ("Horizontal_P" + (i+1), "Vertical_P" + (i+1));	
		}

		SetCameras ();

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


}
