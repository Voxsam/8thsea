using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelContainerController : MonoBehaviour {

	public Text levelName;

	public int currentlySelectedItem = 0;

	public LevelContainerItem[] levels;

	public float distanceBetweenImages = 400f;
	public Vector3 newScale = new Vector3 (1.2f, 1.2f, 1f);
	public float slideTime = 0.3f;

	private bool canChangeLevel = true;
	private Vector3 initPos;

	void Start () {
		initPos = transform.localPosition;
		RefreshSelectedItem ();
	}

	void Update () {

		if (canChangeLevel) {

			if (PlayerList.Obj.IsRightButtonPressedByAnyPlayer ()) {

				if (currentlySelectedItem < levels.Length - 1) {
					ResetSelectedItem ();
					currentlySelectedItem++;
					RefreshSelectedItem ();
					MainMenuAudioManager.Obj.PlayMoveNoise ();
					StartCoroutine (SmoothlyMoveContainer (distanceBetweenImages * -1f));
				}

			} else if (PlayerList.Obj.IsLeftButtonPressedByAnyPlayer ()) {

				if (currentlySelectedItem > 0) {
					ResetSelectedItem ();
					currentlySelectedItem--;
					RefreshSelectedItem ();
					MainMenuAudioManager.Obj.PlayMoveNoise ();
					StartCoroutine (SmoothlyMoveContainer (distanceBetweenImages));
				}

			}
		}

		if (PlayerList.Obj.IsActionButtonPressedByAnyPlayer ()) {

			if (currentlySelectedItem == 0) {
				AdvanceToTutorial ();
			} else {
				AdvanceToLevel (levels [currentlySelectedItem].sceneName);
			}

		}

	}
		

	IEnumerator SmoothlyMoveContainer (float distance) {
		canChangeLevel = false;
		float elapsedTime = 0;
		Vector3 originalPos = transform.localPosition;
		while (transform.localPosition.x != originalPos.x + distance) {
			transform.localPosition = Vector3.Lerp (originalPos, new Vector3 (originalPos.x + distance, originalPos.y, originalPos.z), elapsedTime / slideTime);
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		canChangeLevel = true;
	}

	private void ResetSelectedItem () {
		levels [currentlySelectedItem].rectTransform.localScale = Vector3.one;
		levels [currentlySelectedItem].floatController.enabled = false;
	}

	private void RefreshSelectedItem () {
		levels [currentlySelectedItem].rectTransform.localScale = newScale;
		levels [currentlySelectedItem].floatController.enabled = true;
		levelName.text = levels [currentlySelectedItem].levelName;
	}


	public void AdvanceToLevel (string levelName)
	{
			if (GameController.Obj != null)
			{
				GameController.Obj.isTutorial = false;
			}

			// Load the main game scene
			SceneManager.LoadScene(levelName);
	}

	public void AdvanceToTutorial()
	{
		if (GameController.Obj != null) {
			GameController.Obj.isTutorial = true;
		}
		SceneManager.LoadScene ("tutorial");
	}

}
