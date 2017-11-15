	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectMenuController : MonoBehaviour {

	public Text[] textboxes;

	public int numPlayers = 0;

	public PlayerList pList;

    [SerializeField] private Text StartGameText;

	private List<int> joysticks = new List<int> ();
	private List<KeyCode> keys = new List<KeyCode> ();

	private List<int> usedJoysticks = new List<int> ();
	private List<KeyCode> usedKeys = new List<KeyCode> ();
    private const KeyCode START_GAME_BUTTON = KeyCode.Space;
    private const KeyCode START_TUTORIAL_BUTTON = KeyCode.T;

    void Start ()
	{
		int i = 1;
		print (Input.GetJoystickNames ().Length + " joysticks connected: ");
		foreach (string joystick in Input.GetJoystickNames()) {
			print (joystick + " is connected as Joystick " + i + ".");
			i++;
		}

		joysticks.AddRange(new int[] { 1, 2, 3, 4 });
		keys.AddRange (new KeyCode[] { KeyCode.W, KeyCode.UpArrow });
        StartGameText.enabled = false;

	}

	void LateUpdate ()
	{
		// Iterate through all 4 joysticks
		foreach (int j in joysticks) {
			if (!usedJoysticks.Contains(j) && Input.GetKeyDown ("joystick " + j + " button 0")) {
				pList.AddPlayer (new Player(new ControlScheme(j, false), numPlayers + 1));
				usedJoysticks.Add (j);
				textboxes[numPlayers].text = "Player " + (numPlayers + 1)  + " is using Joystick " + j + "."; 
				numPlayers++;
            }

            KeyCode upKey = ControlScheme.GetKeyCode(j, ControlScheme.Controller.Up);
            // Iterate through all key configurations
            if (!usedKeys.Contains(upKey) && Input.GetKeyDown(upKey))
            {
                pList.AddPlayer(new Player(new ControlScheme(j, true), numPlayers + 1));
                usedKeys.Add(upKey);
                textboxes[numPlayers].text = "Player " + (numPlayers + 1) + " is using " + upKey.ToString() + ".";
                numPlayers++;
            }
        }

        if (numPlayers > 1 && !StartGameText.isActiveAndEnabled)
        {
            StartGameText.enabled = true;
        }
        else if (numPlayers == 0 && StartGameText.isActiveAndEnabled)
        {
            StartGameText.enabled = false;
        }

        if (Input.GetKeyDown(START_GAME_BUTTON))
        {
            AdvanceToGame();
        }
        else if (Input.GetKeyDown(START_TUTORIAL_BUTTON))
        {
            AdvanceToTutorial();
        }

        /*
		// Iterate through all key configurations
		foreach (KeyCode key in keys) {
			if (!usedKeys.Contains(key) && Input.GetKeyDown (key)) {
				pList.AddPlayer (new Player (new ControlScheme(0, true, key), numPlayers + 1));
				usedKeys.Add (key);
				textboxes[numPlayers].text = "Player " + (numPlayers + 1)  + " is using " + key.ToString() + ".";
				numPlayers++;
			}
		}
        //*/
    }

	public void AdvanceToGame ()
	{
		if (numPlayers > 0)
        {
            if (GameController.Obj != null)
            {
                GameController.Obj.isTutorial = false;
            }

            // Load the main game scene
            SceneManager.LoadScene("main_merged");
		}
	}

    public void AdvanceToTutorial()
    {
        if (numPlayers > 0)
        {
            if (GameController.Obj != null)
            {
                GameController.Obj.isTutorial = true;
            }
            // Load the main tutorial scene
            SceneManager.LoadScene("tutorial");
        }

    }

}
