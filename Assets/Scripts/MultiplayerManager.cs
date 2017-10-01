using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour {

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
	 * 
	**/


	void Start () {

		// Set up split screen for multiple players
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

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
