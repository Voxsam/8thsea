using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour {

	public static MultiplayerManager Obj;

	// The list of players, which SHOULDN'T change at any point during the game.
	public List<PlayerController> playerControllerList;

	// The player prefab that should have a PlayerController component attached to it.
	public GameObject playerPrefab;

	// The PlayerCamera prefab that should have a PlayerCameraController component attached to it.
	public GameObject playerCameraPrefab;

	// Should be in the order of preference of spawning, depending on # of players.
	public GameObject[] spawnPoints;

	void Awake ()
	{

		if (Obj == null)
		{
			Obj = this;
			playerControllerList = new List<PlayerController> ();
		}
		else
		{
			Destroy(this.gameObject);
		}
	
		DontDestroyOnLoad(this.gameObject);

	}


	public void Setup () {

		for (int i = 0; i < PlayerList.Obj.numPlayers; i++) {

			GameObject player = Instantiate (playerPrefab);
			GameObject camera = Instantiate (playerCameraPrefab);
			player.transform.position = spawnPoints [i].transform.position;
			PlayerController pCtrl = player.GetComponent<PlayerController> ();
			pCtrl.player = PlayerList.Obj.playerList [i];
			pCtrl.cameraController = camera.GetComponent<CameraController> ();
			playerControllerList.Add (pCtrl);

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



}
