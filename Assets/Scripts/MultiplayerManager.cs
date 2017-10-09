using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour {

	public PlayerController[] playerList;

	public enum PlayerNumber {
		PlayerOne, PlayerTwo, PlayerThree, PlayerFour
	}

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
		/*
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


	// Adds a player to the list of currently active players.
	public void AddPlayer(PlayerController player) {

		switch (playerList.Length) {

		case 1:
			player.playerNumber = PlayerNumber.PlayerTwo;

		case 2:
			player.playerNumber = PlayerNumber.PlayerThree;

		case 3:
			player.playerNumber = PlayerNumber.PlayerFour;

		default:
			player.playerNumber = PlayerNumber.PlayerOne;

		}

	}

	// Called once when a player is added.
	public ControlScheme getControlScheme(PlayerNumber playerNum) {

		switch (playerNum) {

		case PlayerNumber.PlayerOne:
			return new ControlScheme ("Horizontal_P1", "Vertical_P1");

		case PlayerNumber.PlayerTwo:
			return new ControlScheme ("Horizontal_P2", "Vertical_P2");

		case PlayerNumber.PlayerThree:
			return new ControlScheme ("Horizontal_P3", "Vertical_P3");

		case PlayerNumber.PlayerFour:
			return new ControlScheme ("Horizontal_P4", "Vertical_P4");

		default:
			return new ControlScheme ("Horizontal_P1", "Vertical_P1");

		}

	}



}
