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
	private List<int> joysticks = new List<int> ();
	private List<KeyCode> keys = new List<KeyCode> ();

	void Start ()
	{
		int i = 1;
		print (Input.GetJoystickNames ().Length + " joysticks connected: ");
		foreach (string joystick in Input.GetJoystickNames()) {
			print (joystick + " is connected as Joystick " + i + ".");
			i++;
		}

		joysticks.AddRange(new int[] {1, 2, 3, 4});
		keys.AddRange (new KeyCode[] {KeyCode.W, KeyCode.UpArrow});

	}

	void Update ()
	{
		// Iterate through all 4 joysticks
		foreach (int j in joysticks) {
			if (Input.GetKeyDown ("joystick " + j + " button 2")) {
				pList.AddPlayer (new Player(new ControlScheme(j, false, KeyCode.A), numPlayers + 1));
				joysticks.Remove (j);
				textboxes[numPlayers].text = "Player " + (numPlayers + 1)  + " is using Joystick " + j + "."; 
				numPlayers++;
			}
		}

		// Iterate through all key configurations
		foreach (KeyCode key in keys) {
			if (Input.GetKeyDown (key)) {
				pList.AddPlayer (new Player (new ControlScheme(0, true, key), numPlayers + 1));
				keys.Remove (key);
				textboxes[numPlayers].text = "Player " + (numPlayers + 1)  + " is using " + key.ToString() + ".";
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
