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

	void Start ()
	{
		int i = 1;
		print (Input.GetJoystickNames ().Length + " joysticks connected: ");
		foreach (string joystick in Input.GetJoystickNames()) {
			print (joystick + " is connected as Joystick " + i + ".");
			i++;
		}
	}

	void Update ()
	{
		// Iterate through all 4 joysticks
		for (int i = 1; i < 5; i++) {
			// If the button is down but has not already been used
			if (Input.GetKeyDown ("joystick " + i + " button 2") && !usedJoysticks.Contains(i)) {
				pList.AddPlayer (new Player (new ControlScheme (i), numPlayers + 1));
				usedJoysticks.Add (i);
				textboxes[numPlayers].text = "Player " + (numPlayers + 1)  + " is using Joystick " + i + "."; 
				numPlayers++;
			}
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
