using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour {

	public static MultiplayerManager Obj;

	public PlayerController[] playerList;



	/**
	 * 
	 * TEMPORARY CONTROL SCHEME (KEYBOARD ONLY, NO GAMEPAD)
	 * 
	 * PLAYER 1
	 * ARROW KEYS + SPACE
	 * 
	 * PLAYER 2
	 * WSAD + E
	 * 
	 **/

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

	void Start () {



		// Set up split screen for multiple players
		/**
		if (playerList.Length == 2) {
			playerList [0].cam.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (0.5f, 1.0f));
			playerList [1].cam.rect = new Rect (new Vector2 (0.5f, 0f), new Vector2 (0.5f, 1.0f));
		} else if (playerList.Length == 3) {
			playerList [0].cam.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (0.333f, 1.0f));
			playerList [1].cam.rect = new Rect (new Vector2 (0.333f, 0f), new Vector2 (0.333f, 1.0f));
			playerList [2].cam.rect = new Rect (new Vector2 (0.333f, 0f), new Vector2 (0.333f, 1.0f));
		} else if (playerList.Length == 4) {
			playerList [0].cam.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (0.5f, 0.5f));
			playerList [1].cam.rect = new Rect (new Vector2 (0.5f, 0f), new Vector2 (0.5f, 0.5f));
			playerList [2].cam.rect = new Rect (new Vector2 (0f, 0.5f), new Vector2 (0.5f, 0.5f));
			playerList [3].cam.rect = new Rect (new Vector2 (0.5f, 0.5f), new Vector2 (0.5f, 0.5f));
		} else {
			playerList [0].cam.rect = new Rect (new Vector2 (0f, 0f), new Vector2 (1.0f, 1.0f));
		}
		*/

	}
		


	private void Setup () {

		for (int i = 0; i < playerList.Length; i++) {

			PlayerController player = playerList [i];
			player.playerNumber = i + 1;
			player.controls = new ControlScheme ("Horizontal_P" + (i+1), "Vertical_P" + (i+1));

		}

	}

	void Update () {


	}


}
