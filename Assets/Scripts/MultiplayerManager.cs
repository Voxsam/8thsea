using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour {

	public static MultiplayerManager Obj;

	// The list of players, which SHOULDN'T change at any point during the game.
	public List<Player> playerList;

	// The player prefab that should have a PlayerController component attached to it.
	public GameObject playerPrefab;

	// The PlayerCamera prefab that should have a PlayerCameraController component attached to it.
	public GameObject playerCameraPrefab;

	// The first 2 should be lab spawn points, and the last 2 should be sub spawn points.
	public GameObject[] spawnPoints;

	void Awake () {

		if (playerList == null) {
			playerList = new List<Player> ();
		}

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

		int i = 0;
		foreach (Player player in playerList) {

			GameObject newPlayer = Instantiate (playerPrefab);
			newPlayer.transform.position = new Vector3 (-6.8f + 2f * (float) i, 3.15f, 0f);
			i++;
		}

		/*
		for (int i = 0; i < playerList.Length; i++) {

			PlayerController player = playerList [i];
			player.playerNumber = i + 1;
			player.controls = new ControlScheme (player.joystickNumber);	

		}
		*/

		// SetCameras ();

	}



	/*
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
	*/

	public void AddPlayer (Player p)
	{
		playerList.Add (p);
	}



}
