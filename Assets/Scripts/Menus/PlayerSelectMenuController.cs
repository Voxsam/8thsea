	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectMenuController : MonoBehaviour {

	public Text[] textboxes;

	public int numPlayers = 0;

	public PlayerList pList;

	private List<int> usedJoysticks = new List<int> ();
	private List<KeyCode> keys = new List<KeyCode> ();

	void Start ()
	{
		int i = 1;
		print (Input.GetJoystickNames ().Length + " joysticks connected: ");
		foreach (string joystick in Input.GetJoystickNames()) {
			print (joystick + " is connected as Joystick " + i + ".");
			i++;
		}

		keys.Add (KeyCode.UpArrow, KeyCode.W);

	}

	void Update ()
	{
		// Iterate through all 4 joysticks
		for (int i = 1; i < 5; i++) {
			// If the button is down but has not already been used
			if (Input.GetKeyDown ("joystick " + i + " button 2") && !usedJoysticks.Contains(i)) {
				pList.AddPlayer (new Player (new ControlScheme (i, false), numPlayers + 1));
				usedJoysticks.Add (i);
				textboxes[numPlayers].text = "Player " + (numPlayers + 1)  + " is using Joystick " + i + "."; 
				numPlayers++;
			}
		}

		foreach (KeyCode key in keys) {
			if (Input.GetKeyDown (key)) {
			}
		}

		// Check keyboard "Up" keys (W and Up Arrow for 2 players)
		if (Input.GetKeyDown (KeyCode.W)) {
			pList.AddPlayer (new Player (new ControlScheme (1, true), numPlayers + 1));
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			pList.AddPlayer (new Player (new ControlScheme (2, true), numPlayers + 1));
		}
	}

	public void AdvanceToGame ()
	{
		if (numPlayers > 0) {
			// Load the main game scene
			SceneManager.LoadScene (1);
		}
	}

}
