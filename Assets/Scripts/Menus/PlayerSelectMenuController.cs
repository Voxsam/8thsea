	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectMenuController : MonoBehaviour {

	public int numPlayers = 0;

	public PlayerList pList;

	public GameObject levelSelectInterface;

	public PlayerSelectElement[] playerSelectElements;

	private List<int> joysticks = new List<int> ();
	private List<KeyCode> keys = new List<KeyCode> ();

	private List<int> usedJoysticks = new List<int> ();
	private List<KeyCode> usedKeys = new List<KeyCode> ();
    private const KeyCode START_GAME_BUTTON = KeyCode.Space;
    private const KeyCode START_TUTORIAL_BUTTON = KeyCode.T;

	private bool canMoveToLevelSelect = false;

    void Start ()
	{
        if (GameController.Obj != null)
        {
            Destroy(GameController.Obj.gameObject);
        }

		int i = 1;
		print (Input.GetJoystickNames ().Length + " joysticks connected: ");
		foreach (string joystick in Input.GetJoystickNames()) {
			print (joystick + " is connected as Joystick " + i + ".");
			i++;
		}

		joysticks.AddRange(new int[] { 1, 2, 3, 4 });
		keys.AddRange (new KeyCode[] { KeyCode.W, KeyCode.UpArrow });

	}

	void LateUpdate ()
	{

		// Iterate through all 4 joysticks
		foreach (int j in joysticks) {
			if (!usedJoysticks.Contains(j) && Input.GetKeyDown ("joystick " + j + " button 0")) {
				pList.AddPlayer (new Player(new ControlScheme(j, false), numPlayers + 1));
				usedJoysticks.Add (j);
				MainMenuAudioManager.Obj.PlayConfirmNoise ();
				playerSelectElements [numPlayers].Activate ();
				numPlayers++;
            }

            KeyCode actionKey = ControlScheme.GetKeyCode(j, ControlScheme.Controller.Action);
            // Iterate through all key configurations
            if (!usedKeys.Contains(actionKey) && Input.GetKeyDown(actionKey))
            {
                pList.AddPlayer(new Player(new ControlScheme(j, true), numPlayers + 1));
                usedKeys.Add(actionKey);
				MainMenuAudioManager.Obj.PlayConfirmNoise ();
				playerSelectElements [numPlayers].Activate ();
                numPlayers++;
            }
        }

		if (numPlayers > 0 && numPlayers != 3) {
			if (pList.IsMenuButtonPressedByAnyPlayer ()) {
				MainMenuAudioManager.Obj.PlayConfirmNoise2 ();
				StartCoroutine (ShowLevelSelectAfterDelay (1.2f));

			}
		}
			
    }

	IEnumerator ShowLevelSelectAfterDelay (float seconds) {
		yield return new WaitForSecondsRealtime (seconds);
		levelSelectInterface.SetActive (true);
		gameObject.SetActive (false);
	}



}
