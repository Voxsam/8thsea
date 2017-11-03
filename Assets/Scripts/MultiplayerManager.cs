using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour {

	public static MultiplayerManager Obj;

	[Tooltip("Set to false if coming from the character select screen.")]
	public bool isDebug = false;
	public GameObject playerListPrefab;

	// The list of players, which SHOULDN'T change at any point during the game.
	public List<PlayerController> playerControllerList;

	// The player prefab that should have a PlayerController component attached to it.
	public GameObject player1Prefab;
    public GameObject player2Prefab;

	// The PlayerCamera prefab that should have a PlayerCameraController component attached to it.
	public GameObject playerCameraPrefab;
	public GameObject playerCanvasPrefab;
	public GameObject playerPanelPrefab;

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

		if (isDebug) {
			PlayerList pList = Instantiate (playerListPrefab).GetComponent<PlayerList>();
			pList.AddPlayer (new Player (new ControlScheme (0, true, KeyCode.W), 1));
			pList.AddPlayer (new Player (new ControlScheme (0, true, KeyCode.UpArrow), 2));
			pList.numPlayers = 2;
		}

		for (int i = 0; i < PlayerList.Obj.numPlayers; i++) {

          
			GameObject player = (i % 2 == 0) ?  Instantiate (player1Prefab) : Instantiate(player2Prefab);
			GameObject camera = Instantiate (playerCameraPrefab);
            if (i > 0)
            {
                Destroy(camera.GetComponent<AudioListener>());
            }

			player.transform.position = spawnPoints [i].transform.position;
			PlayerController pCtrl = player.GetComponent<PlayerController> ();
			GameObject canvas = Instantiate (playerCanvasPrefab);

			pCtrl.Setup (PlayerList.Obj.playerList [i], camera.GetComponent<PlayerCameraController> (), canvas.GetComponent<PlayerCanvasController> ());

			playerControllerList.Add (pCtrl);

		}

		GameController.Obj.SetPlayers (playerControllerList);
		SetCameras ();

		Obj = this;

	}

	private void SetCameras() {

		switch (playerControllerList.Count) {

		case 1:
			//playerControllerList [0].pCameraController.GetCamera.
			playerControllerList [0].pCameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (1.0f, 1.0f));
			break;

		case 2:
			playerControllerList [0].pCameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (0.5f, 1.0f));
			playerControllerList [1].pCameraController.GetCamera.rect = new Rect (new Vector2 (0.5f, 0f), new Vector2 (0.5f, 1.0f));
			break;

		case 3: 
			playerControllerList [0].pCameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (0.333f, 1.0f));
			playerControllerList [1].pCameraController.GetCamera.rect = new Rect (new Vector2 (0.333f, 0f), new Vector2 (0.333f, 1.0f));
			playerControllerList [2].pCameraController.GetCamera.rect = new Rect (new Vector2 (0.333f, 0f), new Vector2 (0.333f, 1.0f));
			break;

		case 4:
			playerControllerList [0].pCameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0.5f), new Vector2 (0.5f, 0.5f));
			playerControllerList [1].pCameraController.GetCamera.rect = new Rect (new Vector2 (0.5f, 0.5f), new Vector2 (0.5f, 0.5f));
			playerControllerList [2].pCameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (0.5f, 0.5f));
			playerControllerList [3].pCameraController.GetCamera.rect = new Rect (new Vector2 (0.5f, 0f), new Vector2 (0.5f, 0.5f));
			break;

		default:
			playerControllerList [0].pCameraController.GetCamera.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (1.0f, 1.0f));
			break;

		}
			

	}




}
